//
// © 2016-2021 miloush.net. All rights reserved.
//

namespace UAM.Optics.ColorScience
{
    using System.Windows;
    using System.Windows.Media;

    partial class ConvertColor
    {
        public static uint ToUInt32(this Color c)
        {
            return unchecked((uint)
            (
                (c.A << 24) |
                (c.R << 16) |
                (c.G << 8) |
                c.B
            ));
        }

        public static int ToInt32(this Color c)
        {
            return (c.A << 24) |
                   (c.R << 16) |
                   (c.G << 8) |
                    c.B;
        }

        public static Point FastXYZ2uv(float X, float Y, float Z)
        {
            float u, v;
            FastXYZ2uv(X, Y, Z, out u, out v);
            return new Point(u, v);
        }

        public static Point Fastxy2uv(float x, float y)
        {
            float u, v;
            Fastxy2uv(x, y, out u, out v);
            return new Point(u, v);
        }

        public static Point Fastuv2xy(float u, float v)
        {
            float x, y;
            Fastuv2xy(u, v, out x, out y);
            return new Point(x, y);
        }
    }
}
