//
// © 2016-2021 miloush.net. All rights reserved.
//

namespace UAM.Optics.ColorScience
{
    using System;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    public static class ChromaticityDiagram
    {
        public static unsafe ImageSource RenderLinearGamutLuv(int width, int height, RGBPrimaries<Color1931xyY> primaries, Color1931XYZ white, float leftU, float rightU, float topV, float bottomV)
        {
            ColorTransformMatrix RGBtoXYZ = ColorTransformMatrix.GetRGBtoXYZ((Color1931XYZ)primaries.Red, (Color1931XYZ)primaries.Green, (Color1931XYZ)primaries.Blue, white);
            ColorTransformMatrix XYZtoRGB = RGBtoXYZ.Invert();

            return RenderLinearGamutLuv(width, height, XYZtoRGB, leftU, rightU, topV, bottomV);
        }
        public static unsafe ImageSource RenderLinearGamutLuv(int width, int height, ColorTransformMatrix XYZtoRGB, float leftU, float rightU, float topV, float bottomV)
        {
            if (width == 0 || height == 0)
                return null;

            const float minVisibleU = 0.001423f;
            const float maxVisibleU = 0.6234f;

            const float minVisibleV = 0.01592f;
            const float maxVisibleV = 0.5868f;

            float widthU = rightU - leftU;
            float heightV = bottomV - topV;

            float uPerPixel = widthU / width;
            float vPerPixel = heightV / height;

            int pxMinVisibleU = 0; if (uPerPixel != 0) pxMinVisibleU = Math.Max(0, (int)((minVisibleU - leftU) / uPerPixel));
            int pxMaxVisibleU = width; if (uPerPixel != 0) pxMaxVisibleU = Math.Min(width, (int)((maxVisibleU - leftU) / uPerPixel));

            int pxMinVisibleV = height; if (vPerPixel != 0) pxMinVisibleV = Math.Min(height, (int)((minVisibleV - topV) / vPerPixel));
            int pxMaxVisibleV = 0; if (uPerPixel != 0) pxMaxVisibleV = Math.Max(0, (int)((maxVisibleV - topV) / vPerPixel));

            byte[] data = new byte[width * height * 4];

            float R, G, B;
            float u, v, x, y;
            float X, Y = 1f, Z;

            fixed (byte* pData = data)
            {
                for (int pxV = pxMaxVisibleV; pxV <= pxMinVisibleV; pxV++)
                    for (int pxU = pxMinVisibleU; pxU <= pxMaxVisibleU; pxU++)
                    {
                        u = leftU + pxU * uPerPixel;
                        v = topV + pxV * vPerPixel;

                        ConvertColor.Fastuv2xy(u, v, out x, out y);
                        ConvertColor.FastxyY2XYZ(x, y, Y, out X, out Z);

                        XYZtoRGB.Transform(X, Y, Z, out R, out G, out B);

                        if (R >= 0 && G >= 0 && B >= 0)
                        {
                            byte* pBGR = pData + (pxV * width + pxU) * 4;

                            pBGR[0] = (byte)Math.Min(255, ConvertColor.scRGBtosRGB(B) * 255); // TODO: handle negative
                            pBGR[1] = (byte)Math.Min(255, ConvertColor.scRGBtosRGB(G) * 255);
                            pBGR[2] = (byte)Math.Min(255, ConvertColor.scRGBtosRGB(R) * 255);
                            pBGR[3] = 255;
                        }

                    }
            }

            return BitmapSource.Create(width, height, 96, 96, PixelFormats.Pbgra32, null, data, width * 4);
        }
    }
}
