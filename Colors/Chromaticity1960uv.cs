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
    [Obsolete]
    [DebuggerDisplay("u={u}, v={v}")]
    public partial struct Chromaticity1960uv : IEquatable<Chromaticity1960uv>
    {
        /// <summary>
        /// u
        /// </summary>
        public float u;
        /// <summary>
        /// v
        /// </summary>
        public float v;

        public Chromaticity1960uv(float u, float v)
        {
            this.u = u;
            this.v = v;
        }

        public static explicit operator Chromaticity1976uv(Chromaticity1960uv uv) => new Chromaticity1976uv(uv.u, uv.v * 1.5f);
        public static explicit operator Chromaticity1960uv(Chromaticity1976uv uv) => new Chromaticity1960uv(uv.u, uv.v / 1.5f);

        public static Chromaticity1960uv operator +(Chromaticity1960uv a, Chromaticity1960uv b) => new Chromaticity1960uv(a.u + b.u, a.v + b.v);
        public static Chromaticity1960uv operator -(Chromaticity1960uv a, Chromaticity1960uv b) => new Chromaticity1960uv(a.u - b.u, a.v - b.v);
        public static Chromaticity1960uv operator *(Chromaticity1960uv c, float m) => new Chromaticity1960uv(c.u * m, c.v * m);
        public static Chromaticity1960uv operator /(Chromaticity1960uv c, float d) => new Chromaticity1960uv(c.u / d, c.v / d);
        public static bool operator ==(Chromaticity1960uv left, Chromaticity1960uv right) => left.Equals(right);
        public static bool operator !=(Chromaticity1960uv left, Chromaticity1960uv right) => !(left == right);

        public override string ToString() => $"{u},{v}";
        public string ToString(string format) => string.Join(",", u.ToString(format), v.ToString(format));
        public float[] ToArray() => new[] { u, v };

        public override bool Equals(object obj) => obj is Chromaticity1960uv other && Equals(other);
        public bool Equals(Chromaticity1960uv other) => this.u == other.u && this.v == other.v;
        public override int GetHashCode() => u.GetHashCode() ^ v.GetHashCode();
        public void Deconstruct(out float u, out float v) { u = this.u; v = this.v; }
    }
}
