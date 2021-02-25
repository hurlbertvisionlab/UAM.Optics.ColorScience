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
    [DebuggerDisplay("L+M={Isochromatic}, L-M={RGisoluminant}, S-(L+M)={Sisoluminant}")]
    public partial struct ColorDKL : IEquatable<ColorDKL>
    {
        /// <summary>
        /// L+M / Luminance / achromatic / isochromatic modulation.
        /// </summary>
        public float Isochromatic;
        /// <summary>
        /// L-M / L-M opponent / constant-B / constant-S / red-green isoluminant modulation. 
        /// </summary>
        public float RGisoluminant;
        /// <summary>
        /// S-(L+M) / S-Lum opponent / constant R &amp; G / constant L &amp; M / S-cone isoluminant modulation.
        /// </summary>
        public float Sisoluminant;

        public float Azimuth => (float)Math.Atan2(-Sisoluminant, RGisoluminant);
        public float IsoluminantLengthSquared => RGisoluminant * RGisoluminant + Sisoluminant * Sisoluminant;
        public float IsoluminantLength => (float)Math.Sqrt(IsoluminantLengthSquared);
        public float Elevation => (float)Math.Atan2(Isochromatic, IsoluminantLength);

        public float L => Isochromatic + RGisoluminant;
        public float M => (Isochromatic - RGisoluminant) / 2;
        public float S => Sisoluminant - Isochromatic;

        public float LengthSquared => Isochromatic * Isochromatic + IsoluminantLengthSquared;
        public float Length => (float)Math.Sqrt(LengthSquared);

        public ColorDKL(float isochromatic, float rgIsoluminant, float sIsoluminant)
        {
            Isochromatic = isochromatic;
            RGisoluminant = rgIsoluminant;
            Sisoluminant = sIsoluminant;
        }

        public static ColorDKL operator +(ColorDKL a, ColorDKL b) => new ColorDKL(a.Isochromatic + b.Isochromatic, a.RGisoluminant + b.RGisoluminant, a.Sisoluminant + b.Sisoluminant);
        public static ColorDKL operator -(ColorDKL a, ColorDKL b) => new ColorDKL(a.Isochromatic - b.Isochromatic, a.RGisoluminant - b.RGisoluminant, a.Sisoluminant - b.Sisoluminant);
        public static ColorDKL operator *(ColorDKL a, ColorDKL b) => new ColorDKL(a.Isochromatic * b.Isochromatic, a.RGisoluminant * b.RGisoluminant, a.Sisoluminant * b.Sisoluminant);
        public static ColorDKL operator /(ColorDKL a, ColorDKL b) => new ColorDKL(a.Isochromatic / b.Isochromatic, a.RGisoluminant / b.RGisoluminant, a.Sisoluminant / b.Sisoluminant);
        public static ColorDKL operator *(ColorDKL c, float m) => new ColorDKL(c.Isochromatic * m, c.RGisoluminant * m, c.Sisoluminant * m);
        public static ColorDKL operator /(ColorDKL c, float d) => new ColorDKL(c.Isochromatic / d, c.RGisoluminant / d, c.Sisoluminant / d);
        public static bool operator ==(ColorDKL left, ColorDKL right) => left.Equals(right);
        public static bool operator !=(ColorDKL left, ColorDKL right) => !(left == right);

        public override string ToString() => $"{Isochromatic},{RGisoluminant},{Sisoluminant}";
        public string ToString(string format) => string.Join(",", Isochromatic.ToString(format), RGisoluminant.ToString(format), Sisoluminant.ToString(format));
        public float[] ToArray() => new[] { Isochromatic, RGisoluminant, Sisoluminant };

        public override bool Equals(object obj) => obj is ColorDKL other && Equals(other);
        public bool Equals(ColorDKL other) => this.Isochromatic == other.Isochromatic && this.RGisoluminant == other.RGisoluminant && this.Sisoluminant == other.Sisoluminant;
        public override int GetHashCode() => Isochromatic.GetHashCode() ^ RGisoluminant.GetHashCode() ^ Sisoluminant.GetHashCode();
        public void Deconstruct(out float isochromatic, out float rgIsoluminant, out float sIsoluminant) { isochromatic = Isochromatic; rgIsoluminant = RGisoluminant; sIsoluminant = Sisoluminant; }
    }
}
