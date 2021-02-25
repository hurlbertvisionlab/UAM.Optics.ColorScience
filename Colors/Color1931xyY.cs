//
// © 2016-2021 miloush.net. All rights reserved.
//

namespace UAM.Optics.ColorScience
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    [DebuggerDisplay("x={x}, y={y}, Y={Y}")]
    public partial struct Color1931xyY : IEquatable<Color1931xyY>
    {
        /// <summary>
        /// x
        /// </summary>
        public float x;
        /// <summary>
        /// y
        /// </summary>
        public float y;
        /// <summary>
        /// Y
        /// </summary>
        public float Y;

        public Color1931xyY(float x, float y, float Y)
        {
            this.x = x;
            this.y = y;
            this.Y = Y;
        }

        public static explicit operator Color1931XYZ(Color1931xyY c) => ConvertColor.ToXYZ(c);
        public static explicit operator Chromaticity1931xy(Color1931xyY c) => new Chromaticity1931xy(c.x, c.y);

        public static Color1931xyY operator +(Color1931xyY a, Color1931xyY b) => new Color1931xyY(a.x + b.x, a.y + b.y, a.Y + b.Y);
        public static Color1931xyY operator -(Color1931xyY a, Color1931xyY b) => new Color1931xyY(a.x - b.x, a.y - b.y, a.Y - b.Y);
        public static Color1931xyY operator *(Color1931xyY c, float m) => new Color1931xyY(c.x * m, c.y * m, c.Y * m);
        public static Color1931xyY operator /(Color1931xyY c, float d) => new Color1931xyY(c.x / d, c.y / d, c.Y / d);
        public static bool operator ==(Color1931xyY left, Color1931xyY right) => left.Equals(right);
        public static bool operator !=(Color1931xyY left, Color1931xyY right) => !(left == right);

        public override string ToString() => $"{x},{y},{Y}";
        public string ToString(string format) => string.Join(",", x.ToString(format), y.ToString(format), Y.ToString(format));
        public float[] ToArray() => new[] { x, y, Y };

        public override bool Equals(object obj) => obj is Color1931xyY other && Equals(other);
        public bool Equals(Color1931xyY other) => this.x == other.x && this.y == other.y && this.Y == other.Y;
        public override int GetHashCode() => x.GetHashCode() ^ y.GetHashCode() ^ Y.GetHashCode();
        public void Deconstruct(out float x, out float y, out float Y) { x = this.x; y = this.y; Y = this.Y; }
    }
}
