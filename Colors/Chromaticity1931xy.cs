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
    [DebuggerDisplay("x={x}, y={y}")]
    public partial struct Chromaticity1931xy : IEquatable<Chromaticity1931xy>
    {
        public static readonly Chromaticity1931xy EqualEnergyWhite = new Chromaticity1931xy(1 / 3f, 1 / 3f);

        /// <summary>
        /// x
        /// </summary>
        public float x;
        /// <summary>
        /// y
        /// </summary>
        public float y;

        public Chromaticity1931xy(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public static explicit operator Chromaticity1976uv(Chromaticity1931xy xy) => ConvertColor.Touv(xy);

        public static Chromaticity1931xy operator +(Chromaticity1931xy a, Chromaticity1931xy b) => new Chromaticity1931xy(a.x + b.x, a.y + b.y);
        public static Chromaticity1931xy operator -(Chromaticity1931xy a, Chromaticity1931xy b) => new Chromaticity1931xy(a.x - b.x, a.y - b.y);
        public static Chromaticity1931xy operator *(Chromaticity1931xy c, float m) => new Chromaticity1931xy(c.x * m, c.y * m);
        public static Chromaticity1931xy operator /(Chromaticity1931xy c, float d) => new Chromaticity1931xy(c.x / d, c.y / d);
        public static bool operator ==(Chromaticity1931xy left, Chromaticity1931xy right) => left.Equals(right);
        public static bool operator !=(Chromaticity1931xy left, Chromaticity1931xy right) => !(left == right);

        public override string ToString() => $"{x},{y}";
        public string ToString(string format) => string.Join(",", x.ToString(format), y.ToString(format));
        public float[] ToArray() => new[] { x, y };

        public override bool Equals(object obj) => obj is Chromaticity1931xy other && Equals(other);
        public bool Equals(Chromaticity1931xy other) => this.x == other.x && this.y == other.y;
        public override int GetHashCode() => x.GetHashCode() ^ y.GetHashCode();
        public void Deconstruct(out float x, out float y) { x = this.x; y = this.y; }
    }
}
