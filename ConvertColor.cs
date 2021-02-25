//
// © 2016-2021 miloush.net. All rights reserved.
//

namespace UAM.Optics.ColorScience
{
    using System;
    using System.Runtime.CompilerServices;

    // http://www.brucelindbloom.com/
    // Measuring Color by RWG. Hunt, M.R. Pointer

    public static partial class ConvertColor
    {
        public const float ε = 216f / 24389f;
        public const float κ = 24389f / 27f;

        public static Color1931XYZ ToXYZ(int nm)
        {
            Color1931XYZ XYZ = default(Color1931XYZ);

            if (nm < ColorMatchingFunctions.Cie1931XYZ.FirstNanometers || nm > ColorMatchingFunctions.Cie1931XYZ.LastNanometers)
                return XYZ;

            ColorMatchingFunctions.Cie1931XYZ.GetValuesAt(nm, out XYZ.X, out XYZ.Y, out XYZ.Z);
            return XYZ;
        }
        public static Color1931XYZ ToXYZ(this Color1931xyY xyY)
        {
            Color1931XYZ XYZ = default(Color1931XYZ);
            XYZ.Y = xyY.Y;
            FastxyY2XYZ(xyY.x, xyY.y, xyY.Y, out XYZ.X, out XYZ.Z);
            return XYZ;
        }
        public static Color1931XYZ ToXYZ(this Color1976Luv Luv, Color1931XYZ whiteLuv)
        {
            Color1931XYZ XYZ = default(Color1931XYZ);
            FastLuv2XYZ(Luv.L, Luv.u, Luv.v, whiteLuv.X, whiteLuv.Y, whiteLuv.Z, out XYZ.X, out XYZ.Y, out XYZ.Z);
            return XYZ;
        }
        public static Color1931XYZ ToXYZ(this Color1976Luv Luv, Chromaticity1976uv whiteLuv)
        {
            Color1931XYZ XYZ = default(Color1931XYZ);
            FastLuv2XYZ(Luv.L, Luv.u, Luv.v, whiteLuv.u, whiteLuv.v, out XYZ.X, out XYZ.Y, out XYZ.Z);
            return XYZ;
        }
        public static Color1931XYZ ToXYZ(this Color1976Lab Lab, Color1931XYZ whiteLab)
        {
            Color1931XYZ XYZ = default(Color1931XYZ);
            FastLab2XYZ(Lab.L, Lab.a, Lab.b, whiteLab.X, whiteLab.Y, whiteLab.Z, out XYZ.X, out XYZ.Y, out XYZ.Z);
            return XYZ;
        }
        public static Color1931XYZ ToXYZ(this Color1976LCh LCh, Color1931XYZ whiteLab)
        {
            return ToXYZ((Color1976Lab)LCh, whiteLab);
        }
        public static Color1931XYZ ToXYZ(this ColorRGB RGB, ColorTransformMatrix RGBtoXYZ)
        {
            Color1931XYZ XYZ = default(Color1931XYZ);
            RGBtoXYZ.Transform(RGB.R, RGB.G, RGB.B, out XYZ.X, out XYZ.Y, out XYZ.Z);
            return XYZ;
        }
        public static Color1931XYZ ToXYZ(this ColorHSV HSV, ColorTransformMatrix RGBtoXYZ)
        {
            ColorRGB RGB = ToRGB(HSV);
            return ToXYZ(HSV, RGBtoXYZ);
        }
        public static Color1931XYZ ToXYZ(this Chromaticity1931xy xy, float Y)
        {
            Color1931xyY xyY = new Color1931xyY(xy.x, xy.y, Y);
            return ToXYZ(xyY);
        }
        public static Color1931XYZ ToXYZ(this Chromaticity1976uv uv, float Y)
        {
            Chromaticity1931xy xy = Toxy(uv);
            return ToXYZ(xy, Y);
        }
        public static Color1931XYZ ToXYZ(this Chromaticity1976uv uv, float L, Chromaticity1976uv whiteLuv)
        {
            Color1976Luv Luv = ToLuv(uv, L, whiteLuv);
            return ToXYZ(Luv, whiteLuv);
        }

        public static Color1931xyY ToxyY(int nm)
        {
            Color1931XYZ XYZ = ToXYZ(nm);
            return ToxyY(XYZ);
        }
        public static Color1931xyY ToxyY(this Color1931XYZ XYZ)
        {
            Color1931xyY xyY = default(Color1931xyY);
            xyY.Y = XYZ.Y;
            FastXYZ2xy(XYZ.X, XYZ.Y, XYZ.Z, out xyY.x, out xyY.y);
            return xyY;
        }
        public static Color1931xyY ToxyY(this Color1976Luv Luv, Color1931XYZ whiteLuv)
        {
            Color1931XYZ XYZ = ToXYZ(Luv, whiteLuv);
            return ToxyY(XYZ);
        }
        public static Color1931xyY ToxyY(this Color1976Luv Luv, Chromaticity1976uv whiteLuv)
        {
            Color1931XYZ XYZ = ToXYZ(Luv, whiteLuv);
            return ToxyY(XYZ);
        }
        public static Color1931xyY ToxyY(this Color1976Lab Lab, Color1931XYZ whiteLab)
        {
            Color1931xyY xyY = default(Color1931xyY);
            FastLab2XYZ(Lab.L, Lab.a, Lab.b, whiteLab.X, whiteLab.Y, whiteLab.Z, out float X, out xyY.Y, out float Z);
            FastXYZ2xy(X, xyY.Y, Z, out xyY.x, out xyY.y);
            return xyY;
        }
        public static Color1931xyY ToxyY(this Color1976LCh LCh, Color1931XYZ whiteLab)
        {
            return ToxyY((Color1976Lab)LCh, whiteLab);
        }
        public static Color1931xyY ToxyY(this ColorRGB RGB, ColorTransformMatrix RGBtoXYZ)
        {
            Color1931XYZ XYZ = ToXYZ(RGB, RGBtoXYZ);
            return ToxyY(XYZ);
        }
        public static Color1931xyY ToxyY(this ColorHSV HSV, ColorTransformMatrix RGBtoXYZ)
        {
            ColorRGB RGB = ToRGB(HSV);
            return ToxyY(HSV, RGBtoXYZ);
        }
        public static Color1931xyY ToxyY(this Chromaticity1931xy xy, float Y)
        {
            return new Color1931xyY(xy.x, xy.y, Y);
        }
        public static Color1931xyY ToxyY(this Chromaticity1976uv uv, float Y)
        {
            Chromaticity1931xy xy = Toxy(uv);
            return ToxyY(xy, Y);
        }
        public static Color1931xyY ToxyY(this Chromaticity1976uv uv, float L, Chromaticity1976uv whiteLuv)
        {
            Color1976Luv Luv = ToLuv(uv, L, whiteLuv);
            return ToxyY(Luv, whiteLuv);
        }

        public static Color1976Luv ToLuv(int nm, Color1931XYZ whiteLuv)
        {
            Color1931XYZ XYZ = ToXYZ(nm);
            return ToLuv(XYZ, whiteLuv);
        }
        public static Color1976Luv ToLuv(this Color1931XYZ XYZ, Color1931XYZ whiteLuv)
        {
            Color1976Luv Luv = default(Color1976Luv);
            FastXYZ2Luv(XYZ.X, XYZ.Y, XYZ.Z, whiteLuv.X, whiteLuv.Y, whiteLuv.Z, out Luv.L, out Luv.u, out Luv.v);
            return Luv;
        }
        public static Color1976Luv ToLuv(this Color1931xyY xyY, Color1931XYZ whiteLuv)
        {
            Color1931XYZ XYZ = ToXYZ(xyY);
            return ToLuv(XYZ, whiteLuv);
        }
        public static Color1976Luv ToLuv(this ColorRGB RGB, ColorTransformMatrix RGBtoXYZ, Color1931XYZ whiteLuv)
        {
            Color1931XYZ XYZ = ToXYZ(RGB, RGBtoXYZ);
            return ToLuv(XYZ, whiteLuv);
        }
        public static Color1976Luv ToLuv(this ColorHSV HSV, ColorTransformMatrix RGBtoXYZ, Color1931XYZ whiteLuv)
        {
            Color1931XYZ XYZ = ToXYZ(HSV, RGBtoXYZ);
            return ToLuv(XYZ, whiteLuv);
        }
        public static Color1976Luv ToLuv(this Chromaticity1931xy xy, float Y, Color1931XYZ whiteLuv)
        {
            Color1931XYZ XYZ = ToXYZ(xy, Y);
            return ToLuv(XYZ, whiteLuv);
        }
        public static Color1976Luv ToLuv(this Chromaticity1976uv uv, float L, Chromaticity1976uv whiteLuv)
        {
            Color1976Luv Luv = default(Color1976Luv);
            Luv.L = L;
            Luv.u = 13 * L * (uv.u - whiteLuv.u);
            Luv.v = 13 * L * (uv.u - whiteLuv.u);
            return Luv;
        }

        public static Color1976Lab ToLab(int nm, Color1931XYZ whiteLab)
        {
            Color1931XYZ XYZ = ToXYZ(nm);
            return ToLab(XYZ, whiteLab);
        }
        public static Color1976Lab ToLab(this Color1931XYZ XYZ, Color1931XYZ whiteLab)
        {
            Color1976Lab Lab = default(Color1976Lab);
            FastXYZ2Lab(XYZ.X, XYZ.Y, XYZ.Z, whiteLab.X, whiteLab.Y, whiteLab.Z, out Lab.L, out Lab.a, out Lab.b);
            return Lab;
        }
        public static Color1976Lab ToLab(this Color1931xyY xyY, Color1931XYZ whiteLab)
        {
            Color1931XYZ XYZ = ToXYZ(xyY);
            return ToLab(XYZ, whiteLab);
        }
        public static Color1976Lab ToLab(this Color1976Luv Luv, Color1931XYZ whiteLab)
        {
            Color1931XYZ XYZ = ToXYZ(Luv, whiteLab);
            return ToLab(XYZ, whiteLab);
        }
        public static Color1976Lab ToLab(this Color1976LCh LCh)
        {
            return (Color1976Lab)LCh;
        }
        public static Color1976Lab ToLab(this ColorRGB RGB, ColorTransformMatrix RGBtoXYZ, Color1931XYZ whiteLab)
        {
            Color1931XYZ XYZ = ToXYZ(RGB, RGBtoXYZ);
            return ToLab(XYZ, whiteLab);
        }
        public static Color1976Lab ToLab(this ColorHSV HSV, ColorTransformMatrix RGBtoXYZ, Color1931XYZ whiteLab)
        {
            Color1931XYZ XYZ = ToXYZ(HSV, RGBtoXYZ);
            return ToLab(XYZ, whiteLab);
        }
        public static Color1976Lab ToLab(this Chromaticity1931xy xy, float Y, Color1931XYZ whiteLab)
        {
            Color1931XYZ XYZ = ToXYZ(xy, Y);
            return ToLab(XYZ, whiteLab);
        }
        public static Color1976Lab ToLab(this Chromaticity1976uv uv, float L, Chromaticity1976uv whiteLuv, Color1931XYZ whiteLab)
        {
            Color1931XYZ XYZ = ToXYZ(uv, L, whiteLuv);
            return ToLab(XYZ, whiteLab);
        }

        internal static ColorLMS ToLMS(this Color1931XYZ XYZ, ref ColorTransformMatrix XYZtoLMS)
        {
            ColorLMS LMS;
            XYZtoLMS.Transform(XYZ.X, XYZ.Y, XYZ.Z, out LMS.L, out LMS.M, out LMS.S);
            return LMS;
        }
        internal static ColorLMS ToLMS(this Color1931xyY xyY, ref ColorTransformMatrix XYZtoLMS) => xyY.ToXYZ().ToLMS(ref XYZtoLMS);

        public static Chromaticity1931xy Toxy(int nm)
        {
            Color1931XYZ XYZ = ToXYZ(nm);
            return Toxy(XYZ);
        }
        public static Chromaticity1931xy Toxy(this Color1931XYZ XYZ)
        {
            Chromaticity1931xy xy = default(Chromaticity1931xy);
            FastXYZ2xy(XYZ.X, XYZ.Y, XYZ.Z, out xy.x, out xy.y);
            return xy;
        }
        public static Chromaticity1931xy Toxy(this Color1931xyY xyY)
        {
            return new Chromaticity1931xy(xyY.x, xyY.y);
        }
        public static Chromaticity1931xy Toxy(this Color1976Luv Luv, Chromaticity1976uv white)
        {
            Chromaticity1976uv uv = Touv(Luv, white);
            return Toxy(uv);
        }
        public static Chromaticity1931xy Toxy(this Color1976Lab Lab, Color1931XYZ white)
        {
            Chromaticity1931xy xy = default(Chromaticity1931xy);
            FastLab2XYZ(Lab.L, Lab.a, Lab.b, white.X, white.Y, white.Z, out float X, out float Y, out float Z);
            FastXYZ2xy(X, Y, Z, out xy.x, out xy.y);
            return xy;
        }
        public static Chromaticity1931xy Toxy(this Color1976LCh LCh, Color1931XYZ white)
        {
            return Toxy((Color1976Lab)LCh, white);
        }
        public static Chromaticity1931xy Toxy(this ColorRGB RGB, ColorTransformMatrix RGBtoXYZ)
        {
            Color1931XYZ XYZ = ToXYZ(RGB, RGBtoXYZ);
            return Toxy(XYZ);
        }
        public static Chromaticity1931xy Toxy(this ColorHSV HSV, ColorTransformMatrix RGBtoXYZ)
        {
            ColorRGB RGB = ToRGB(HSV);
            return Toxy(RGB, RGBtoXYZ);
        }
        public static Chromaticity1931xy Toxy(this Chromaticity1976uv uv)
        {
            Chromaticity1931xy xy = default(Chromaticity1931xy);
            Fastuv2xy(uv.u, uv.v, out xy.x, out xy.y);
            return xy;
        }

        public static Chromaticity1976uv Touv(int nm)
        {
            Color1931XYZ XYZ = ToXYZ(nm);
            return Touv(XYZ);
        }
        public static Chromaticity1976uv Touv(this Color1931XYZ XYZ)
        {
            Chromaticity1976uv uv = default(Chromaticity1976uv);
            FastXYZ2uv(XYZ.X, XYZ.Y, XYZ.Z, out uv.u, out uv.v);
            return uv;
        }
        public static Chromaticity1976uv Touv(this Color1931xyY xyY)
        {
            Chromaticity1976uv uv = default(Chromaticity1976uv);
            Fastxy2uv(xyY.x, xyY.y, out uv.u, out uv.v);
            return uv;
        }
        public static Chromaticity1976uv Touv(this Color1976Luv Luv, Chromaticity1976uv white)
        {
            Chromaticity1976uv uv = default(Chromaticity1976uv);
            FastLuv2uv(Luv.L, Luv.u, Luv.v, white.u, white.v, out uv.u, out uv.v);
            return uv;
        }
        public static Chromaticity1976uv Touv(this Color1976Lab Lab, Color1931XYZ white)
        {
            Chromaticity1976uv uv = default(Chromaticity1976uv);
            FastLab2XYZ(Lab.L, Lab.a, Lab.b, white.X, white.Y, white.Z, out float X, out float Y, out float Z);
            FastXYZ2uv(X, Y, Z, out uv.u, out uv.v);
            return uv;
        }
        public static Chromaticity1976uv Touv(this Color1976LCh LCh, Color1931XYZ white)
        {
            return Touv((Color1976Lab)LCh, white);
        }
        public static Chromaticity1976uv Touv(this ColorRGB RGB, ColorTransformMatrix RGBtoXYZ)
        {
            Color1931XYZ XYZ = ToXYZ(RGB, RGBtoXYZ);
            return Touv(XYZ);
        }
        public static Chromaticity1976uv Touv(this ColorHSV HSV, ColorTransformMatrix RGBtoXYZ)
        {
            Color1931XYZ XYZ = ToXYZ(HSV, RGBtoXYZ);
            return Touv(XYZ);
        }
        public static Chromaticity1976uv Touv(this Chromaticity1931xy xy)
        {
            Chromaticity1976uv uv = default(Chromaticity1976uv);
            Fastxy2uv(xy.x, xy.y, out uv.u, out uv.v);
            return uv;
        }

        public static ColorRGB ToRGB(int nm, ColorTransformMatrix XYZtoRGB)
        {
            Color1931XYZ XYZ = ToXYZ(nm);
            return ToRGB(XYZ, XYZtoRGB);
        }
        public static ColorRGB ToRGB(this Color1931XYZ XYZ, ColorTransformMatrix XYZtoRGB)
        {
            ColorRGB RGB = default(ColorRGB);
            XYZtoRGB.Transform(XYZ.X, XYZ.Y, XYZ.Z, out RGB.R, out RGB.G, out RGB.B);
            return RGB;
        }
        public static ColorRGB ToRGB(this Color1931xyY xyY, ColorTransformMatrix XYZtoRGB)
        {
            Color1931XYZ XYZ = ToXYZ(xyY);
            return ToRGB(XYZ, XYZtoRGB);
        }
        public static ColorRGB ToRGB(this Color1976Luv Luv, ColorTransformMatrix XYZtoRGB, Color1931XYZ white)
        {
            Color1931XYZ XYZ = ToXYZ(Luv, white);
            return ToRGB(XYZ, XYZtoRGB);
        }
        public static ColorRGB ToRGB(this Color1976Lab Lab, ColorTransformMatrix XYZtoRGB, Color1931XYZ white)
        {
            Color1931XYZ XYZ = ToXYZ(Lab, white);
            return ToRGB(XYZ, XYZtoRGB);
        }
        public static ColorRGB ToRGB(this Color1976LCh LCh, ColorTransformMatrix XYZtoRGB, Color1931XYZ white)
        {
            return ToRGB((Color1976Lab)LCh, XYZtoRGB, white);
        }
        public static ColorRGB ToRGB(this ColorHSV c)
        {
            ColorRGB RGB = default(ColorRGB);
            FastHSV2RGB(c.H, c.S, c.V, out RGB.R, out RGB.G, out RGB.B);
            return RGB;
        }
        public static ColorRGB ToRGB(this Chromaticity1931xy xy, ColorTransformMatrix XYZtoRGB)
        {
            Color1931XYZ XYZ = ToXYZ(xy, 1f);
            return ToRGB(XYZ, XYZtoRGB);
        }
        public static ColorRGB ToRGB(this Chromaticity1976uv uv, ColorTransformMatrix XYZtoRGB)
        {
            Chromaticity1931xy xy = Toxy(uv);
            return ToRGB(xy, XYZtoRGB);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ToUInt32(byte r, byte g, byte b)
        {
            return unchecked((uint)
            (
                (r << 16) |
                (g << 8) |
                b
            ));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ToUInt32(byte a, byte r, byte g, byte b)
        {
            return unchecked((uint)
            (
                (a << 24) |
                (r << 16) |
                (g << 8) |
                b
            ));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ToUInt32(int r, int g, int b)
        {
            return unchecked((uint)
            (
                (r << 16) |
                (g << 8) |
                b
            ));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ToUInt32(int a, int r, int g, int b)
        {
            return unchecked((uint)
            (
                (a << 24) |
                (r << 16) |
                (g << 8) |
                b
            ));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ToInt32(byte r, byte g, byte b)
        {
            return (r << 16) |
                   (g << 8) |
                    b;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ToInt32(byte a, byte r, byte g, byte b)
        {
            return (a << 24) |
                   (r << 16) |
                   (g << 8) |
                    b;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ToInt32(int r, int g, int b)
        {
            return (r << 16) |
                   (g << 8) |
                    b;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ToInt32(int a, int r, int g, int b)
        {
            return (a << 24) |
                   (r << 16) |
                   (g << 8) |
                    b;
        }

        public static ColorRGB TosRGB(this ColorRGB scRGB)
        {
            return new ColorRGB
            (
                scRGBtosRGB(scRGB.R),
                scRGBtosRGB(scRGB.G),
                scRGBtosRGB(scRGB.B)
            );
        }
        public static ColorRGB ToscRGB(this ColorRGB sRGB)
        {
            return new ColorRGB
            (
                sRGBtoscRGB(sRGB.R),
                sRGBtoscRGB(sRGB.G),
                sRGBtoscRGB(sRGB.B)
            );
        }

        public static Color1931XYZ ToXYZ(float[] spectrum, int nmFirst, int nmStep, ColorMatchingFunctions.XYZ matchingFunctions)
        {
            if (nmFirst < 0 || nmStep < 0)
                throw new ArgumentOutOfRangeException();

            Color1931XYZ XYZ = default(Color1931XYZ);

            for (int i = 0, nm = nmFirst; i < spectrum.Length; i++, nm += nmStep)
            {
                float x̄, ȳ, z̄;
                matchingFunctions.GetValuesAt(nm, out x̄, out ȳ, out z̄);

                XYZ.X += x̄ * spectrum[i];
                XYZ.Y += ȳ * spectrum[i];
                XYZ.Z += z̄ * spectrum[i];
            }

            return XYZ;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void FastXYZ2xy(float X, float Y, float Z, out float x, out float y)
        {
            x = y = 0;

            float norm = X + Y + Z;
            if (norm == 0)
                return;

            x = X / norm;
            y = Y / norm;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void FastXYZ2Luv(float X, float Y, float Z, float Xr, float Yr, float Zr, out float L, out float u, out float v)
        {
            float yr = Y / Yr;
            float ur, vr;
            FastXYZ2uv(Xr, Yr, Zr, out ur, out vr);
            FastXYZ2uv(X, Y, Z, out u, out v);

            if (yr > ε)
                L = (float)(116 * Math.Pow(yr, 1 / 3.0) - 16);
            else
                L = κ * yr;

            u = 13 * L * (u - ur);
            v = 13 * L * (v - vr);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void FastXYZ2Lab(float X, float Y, float Z, float Xr, float Yr, float Zr, out float L, out float a, out float b)
        {
            float xr = X / Xr;
            float yr = Y / Yr;
            float zr = Z / Zr;

            float fx, fy, fz;

            if (xr > ε)
                fx = (float)Math.Pow(xr, 1 / 3.0);
            else
                fx = (κ * xr + 16) / 116;

            if (yr > ε)
                fy = (float)Math.Pow(yr, 1 / 3.0);
            else
                fy = (κ * yr + 16) / 116;

            if (zr > ε)
                fz = (float)Math.Pow(zr, 1 / 3.0);
            else
                fz = (κ * zr + 16) / 116;

            L = 116 * fy - 16;
            a = 500 * (fx - fy);
            b = 200 * (fy - fz);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void FastXYZ2uv(float X, float Y, float Z, out float u, out float v)
        {
            u = 4 * X / (X + 15 * Y + 3 * Z);
            v = 9 * Y / (X + 15 * Y + 3 * Z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void FastxyY2XYZ(float x, float y, float Y, out float X, out float Z)
        {
            X = Z = 0;

            if (y == 0)
                return;

            X = x * Y / y;
            Z = (1f - x - y) * Y / y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void FastLuv2XYZ(float L, float u, float v, float Xr, float Yr, float Zr, out float X, out float Y, out float Z)
        {
            if (L > κ * ε)
            {
                float root = (L + 16) / 116;
                Y = root * root * root;
            }
            else
            {
                Y = L / κ;
            }

            float ur, vr;
            FastXYZ2uv(Xr, Yr, Zr, out ur, out vr);

            float a = (52 * L / (u + 13 * L * ur) - 1) / 3;
            float b = -5 * Y;
            float c = -1 / 3f;
            float d = Y * (39 * L / (v + 13 * L * vr) - 5);

            X = (d - b) / (a - c);
            Z = X * a + b;

            X *= Xr;
            Y *= Yr;
            Z *= Zr;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void FastLuv2XYZ(float L, float u, float v, float ur, float vr, out float X, out float Y, out float Z)
        {
            if (L > κ * ε)
            {
                float root = (L + 16) / 116;
                Y = root * root * root;
            }
            else
            {
                Y = L / κ;
            }

            float a = (52 * L / (u + 13 * L * ur) - 1) / 3;
            float b = -5 * Y;
            float c = -1 / 3f;
            float d = Y * (39 * L / (v + 13 * L * vr) - 5);

            X = (d - b) / (a - c);
            Z = X * a + b;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void FastLuv2uv(float L, float u, float v, float ur, float vr, out float uc, out float vc)
        {
            uc = u / (13 * L) + ur;
            vc = v / (13 * L) + vr;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void FastLab2XYZ(float L, float a, float b, float Xr, float Yr, float Zr, out float X, out float Y, out float Z)
        {
            float fy = (L + 16) / 116;
            float fx = a / 500 + fy;
            float fz = fy - b / 200;

            float fy3 = fy * fy * fy;
            float fx3 = fx * fx * fx;
            float fz3 = fz * fz * fz;

            float xr = fx3 > ε ? fx3 : (116 * fx - 16) / κ;
            float yr = L > κ * ε ? fy3 : L / κ;
            float zr = fz3 > ε ? fz3 : (116 * fz - 16) / κ;

            X = xr * Xr;
            Y = yr * Yr;
            Z = zr * Zr;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Fastxy2uv(float x, float y, out float u, out float v)
        {
            u = 4 * x / (-2 * x + 12 * y + 3);
            v = 9 * y / (-2 * x + 12 * y + 3);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Fastuv2xy(float u, float v, out float x, out float y)
        {
            x = 9 * u / (6 * u - 16 * v + 12);
            y = 4 * v / (6 * u - 16 * v + 12);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void Fastuv2xy1960(float u, float v, out float x, out float y)
        {
            x = 3 * u / (2 * u - 8 * v + 4);
            y = 2 * v / (2 * u - 8 * v + 4);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void FastHSV2RGB(float H, float S, float V, out float R, out float G, out float B)
        {
            R = G = B = 0;

            int i = (int)(H * 6);
            float f = H * 6 - i;
            float p = V * (1 - S);
            float q = V * (1 - f * S);
            float t = V * (1 - (1 - f) * S);

            switch (i % 6)
            {
                case 0: R = V; G = t; B = p; break;
                case 1: R = q; G = V; B = p; break;
                case 2: R = p; G = V; B = t; break;
                case 3: R = p; G = q; B = V; break;
                case 4: R = t; G = p; B = V; break;
                case 5: R = V; G = p; B = q; break;
            }
        }

        internal static void XYZtoDKL(Color1931XYZ stimulus, ColorLMS backgroundLMS, ref ColorTransformMatrix XYZtoLMS, ref ColorTransformMatrix LMStoDKL, out ColorDKL dkl)
        {
            ColorLMS stimulusLMS;
            XYZtoLMS.Transform(stimulus.X, stimulus.Y, stimulus.Z, out stimulusLMS.L, out stimulusLMS.M, out stimulusLMS.S);

            ColorLMS deltaLMS = stimulusLMS - backgroundLMS;
            LMStoDKL.Transform(deltaLMS.L, deltaLMS.M, deltaLMS.S, out dkl.Isochromatic, out dkl.RGisoluminant, out dkl.Sisoluminant);
        }
        internal static void DKLtoXYZ(ColorDKL dkl, ColorLMS backgroundLMS, ref ColorTransformMatrix DKLtoLMS, ref ColorTransformMatrix LMStoXYZ, out Color1931XYZ xyz)
        {
            ColorLMS diffLMS;
            DKLtoLMS.Transform(dkl.Isochromatic, dkl.RGisoluminant, dkl.Sisoluminant, out diffLMS.L, out diffLMS.M, out diffLMS.S);

            ColorLMS stimulusLMS = diffLMS + backgroundLMS;
            LMStoXYZ.Transform(stimulusLMS.L, stimulusLMS.M, stimulusLMS.S, out xyz.X, out xyz.Y, out xyz.Z);
        }

        public static Color1931XYZ TemperatureToXYZEmissive(int cct)
        {
            const double c1 = 2.0 * Math.PI * 6.626176 * 2.99792458 * 2.99792458; // first radiation constant
            const double c2 = (6.626176 * 2.99792458) / 1.380662;                 // second radiation constant

            int i = 0;
            double X = 0.0;
            double Y = 0.0;
            double Z = 0.0;

            for (int nm = ColorMatchingFunctions.Cie1931XYZ.FirstNanometers; nm <= ColorMatchingFunctions.Cie1931XYZ.LastNanometers; nm += 5)
            {
                double dWavelengthM = nm * 1e-3;
                double dWavelengthM5 = dWavelengthM * dWavelengthM * dWavelengthM * dWavelengthM * dWavelengthM;
                double blackbody = c1 / (dWavelengthM5 * 1e-12 * (Math.Exp(c2 / (cct * dWavelengthM * 1e-3)) - 1));
                X += (blackbody * ColorMatchingFunctions._Cie1931XYZ.X[nm - ColorMatchingFunctions.Cie1931XYZ.FirstNanometers]);
                Y += (blackbody * ColorMatchingFunctions._Cie1931XYZ.Y[nm - ColorMatchingFunctions.Cie1931XYZ.FirstNanometers]);
                Z += (blackbody * ColorMatchingFunctions._Cie1931XYZ.Z[nm - ColorMatchingFunctions.Cie1931XYZ.FirstNanometers]);
                i++;
            }

            return new Color1931XYZ((float)X, (float)Y, (float)Z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float scRGBtosRGB(float value)
        {
            if (value >= 1f) return 1f;

            return value <= 0.0031308f ? 12.92f * value : 1.055f * (float)Math.Pow(value, 1 / 2.4) - 0.055f;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float sRGBtoscRGB(float value)
        {
            if (value >= 1f) return 1f;

            return value <= 0.04045 ? value / 12.92f : (float)Math.Pow((value + 0.055) / 1.055, 2.4);
        }
    }
}
