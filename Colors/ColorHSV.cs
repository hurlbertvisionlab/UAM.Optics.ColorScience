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
    [DebuggerDisplay("H={H}, S={S}, V={V}")]
    public partial struct ColorHSV : IEquatable<ColorHSV>
    {
        /// <summary>
        /// H
        /// </summary>
        public float H;
        /// <summary>
        /// S
        /// </summary>
        public float S;
        /// <summary>
        /// V
        /// </summary>
        public float V;

        public ColorHSV(float H, float S, float V)
        {
            this.H = H;
            this.S = S;
            this.V = V;
        }

        public static explicit operator ColorRGB(ColorHSV c) => ConvertColor.ToRGB(c);

        public static ColorHSV operator +(ColorHSV a, ColorHSV b) => new ColorHSV(a.H + b.H, a.S + b.S, a.V + b.V);
        public static ColorHSV operator -(ColorHSV a, ColorHSV b) => new ColorHSV(a.H - b.H, a.S - b.S, a.V - b.V);
        public static ColorHSV operator *(ColorHSV a, ColorHSV b) => new ColorHSV(a.H * b.H, a.S * b.S, a.V * b.V);
        public static ColorHSV operator /(ColorHSV a, ColorHSV b) => new ColorHSV(a.H / b.H, a.S / b.S, a.V / b.V);
        public static ColorHSV operator *(ColorHSV c, float m) => new ColorHSV(c.H * m, c.S * m, c.V * m);
        public static ColorHSV operator /(ColorHSV c, float d) => new ColorHSV(c.H / d, c.S / d, c.V / d);
        public static bool operator ==(ColorHSV left, ColorHSV right) => left.Equals(right);
        public static bool operator !=(ColorHSV left, ColorHSV right) => !(left == right);

        public override string ToString() => $"{H},{S},{V}";
        public string ToString(string format) => string.Join(",", H.ToString(format), S.ToString(format), V.ToString(format));
        public float[] ToArray() => new[] { H, S, V };

        public override bool Equals(object obj) => obj is ColorHSV other && Equals(other);
        public bool Equals(ColorHSV other) => this.H == other.H && this.S == other.S && this.V == other.V;
        public override int GetHashCode() => H.GetHashCode() ^ S.GetHashCode() ^ V.GetHashCode();
        public void Deconstruct(out float H, out float S, out float V) { H = this.H; S = this.S; V = this.V; }
    }
}
