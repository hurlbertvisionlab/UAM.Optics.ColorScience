//
// © 2016-2021 miloush.net. All rights reserved.
//

namespace UAM.Optics.ColorScience
{
    using System.Windows;
    using System.Windows.Media;

    partial struct Chromaticity1931xy
    {
        public static implicit operator Point(Chromaticity1931xy xy) => new Point(xy.x, xy.y);
        public static explicit operator Chromaticity1931xy(Point xy) => new Chromaticity1931xy((float)xy.X, (float)xy.Y);
    }

    partial struct Chromaticity1960uv
    {
        public static implicit operator Point(Chromaticity1960uv uv) => new Point(uv.u, uv.v);
        public static explicit operator Chromaticity1960uv(Point uv) => new Chromaticity1960uv((float)uv.X, (float)uv.Y);
    }

    partial struct Chromaticity1976uv
    {
        public static implicit operator Point(Chromaticity1976uv uv) => new Point(uv.u, uv.v);
        public static explicit operator Chromaticity1976uv(Point uv) => new Chromaticity1976uv((float)uv.X, (float)uv.Y);
    }

    partial struct Color1976Lab
    {
        public static explicit operator Color1976LCh(Color1976Lab Lab) => new Color1976LCh(Lab.L, Lab.C, Lab.h);
        public static explicit operator Point(Color1976Lab Lab) => new Point(Lab.a, Lab.b);
    }

    partial struct Color1976Luv
    {
        public static explicit operator Color1976LCh(Color1976Luv Luv) => new Color1976LCh(Luv.L, Luv.C, Luv.h);
        public static explicit operator Point(Color1976Luv Luv) => new Point(Luv.u, Luv.v);
    }

    partial struct ColorRGB
    {
        public static explicit operator ColorRGB(Color c) => new ColorRGB(c.ScR, c.ScG, c.ScB);
        public static explicit operator Color(ColorRGB c) => Color.FromScRgb(1f, c.R, c.G, c.B);
    }
}