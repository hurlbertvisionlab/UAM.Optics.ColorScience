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
    [DebuggerDisplay("L={L}, M={M}, S={S}")]
    public partial struct ColorLMS : IEquatable<ColorLMS>
    {
        /// <summary>
        /// Long wavelength cones
        /// </summary>
        public float L;
        /// <summary>
        /// Medium wavelength cones
        /// </summary>
        public float M;
        /// <summary>
        /// Short wavelength cones
        /// </summary>
        public float S;

        public ColorLMS(float L, float M, float S)
        {
            this.L = L;
            this.M = M;
            this.S = S;
        }

        public float LengthSquared => L * L + M * M + S * S;
        public float Length => (float)Math.Sqrt(LengthSquared);

        public static ColorLMS operator +(ColorLMS a, ColorLMS b) => new ColorLMS(a.L + b.L, a.M + b.M, a.S + b.S);
        public static ColorLMS operator -(ColorLMS a, ColorLMS b) => new ColorLMS(a.L - b.L, a.M - b.M, a.S - b.S);
        public static ColorLMS operator *(ColorLMS a, ColorLMS b) => new ColorLMS(a.L * b.L, a.M * b.M, a.S * b.S);
        public static ColorLMS operator /(ColorLMS a, ColorLMS b) => new ColorLMS(a.L / b.L, a.M / b.M, a.S / b.S);
        public static ColorLMS operator *(ColorLMS c, float m) => new ColorLMS(c.L * m, c.M * m, c.S * m);
        public static ColorLMS operator /(ColorLMS c, float d) => new ColorLMS(c.L / d, c.M / d, c.S / d);
        public static bool operator ==(ColorLMS left, ColorLMS right) => left.Equals(right);
        public static bool operator !=(ColorLMS left, ColorLMS right) => !(left == right);

        public override string ToString() => $"{L},{M},{S}";
        public string ToString(string format) => string.Join(",", L.ToString(format), M.ToString(format), S.ToString(format));
        public float[] ToArray() => new[] { L, M, S };

        public override bool Equals(object obj) => obj is ColorLMS other && Equals(other);
        public bool Equals(ColorLMS other) => this.L == other.L && this.M == other.M && this.S == other.S;
        public override int GetHashCode() => L.GetHashCode() ^ M.GetHashCode() ^ S.GetHashCode();
        public void Deconstruct(out float L, out float M, out float S) { L = this.L; M = this.M; S = this.S; }
    }
}
