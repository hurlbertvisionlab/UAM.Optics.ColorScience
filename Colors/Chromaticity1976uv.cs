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
    [DebuggerDisplay("u'={u}, v'={v}")]
    public partial struct Chromaticity1976uv : IEquatable<Chromaticity1976uv>
    {
        /// <summary>
        /// u'
        /// </summary>
        public float u;
        /// <summary>
        /// v'
        /// </summary>
        public float v;

        public Chromaticity1976uv(float u, float v)
        {
            this.u = u;
            this.v = v;
        }

        public static explicit operator Chromaticity1931xy(Chromaticity1976uv uv) => ConvertColor.Toxy(uv);

        public static Chromaticity1976uv operator +(Chromaticity1976uv a, Chromaticity1976uv b) => new Chromaticity1976uv(a.u + b.u, a.v + b.v);
        public static Chromaticity1976uv operator -(Chromaticity1976uv a, Chromaticity1976uv b) => new Chromaticity1976uv(a.u - b.u, a.v - b.v);
        public static Chromaticity1976uv operator *(Chromaticity1976uv c, float m) => new Chromaticity1976uv(c.u * m, c.v * m);
        public static Chromaticity1976uv operator /(Chromaticity1976uv c, float d) => new Chromaticity1976uv(c.u / d, c.v / d);
        public static bool operator ==(Chromaticity1976uv left, Chromaticity1976uv right) => left.Equals(right);
        public static bool operator !=(Chromaticity1976uv left, Chromaticity1976uv right) => !(left == right);

        public override string ToString() => $"{u},{v}";
        public string ToString(string format) => string.Join(",", u.ToString(format), v.ToString(format));
        public float[] ToArray() => new[] { u, v };

        public override bool Equals(object obj) => obj is Chromaticity1976uv other && Equals(other);
        public bool Equals(Chromaticity1976uv other) => this.u == other.u && this.v == other.v;
        public override int GetHashCode() => u.GetHashCode() ^ v.GetHashCode();
        public void Deconstruct(out float u, out float v) { u = this.u; v = this.v; }
    }
}
