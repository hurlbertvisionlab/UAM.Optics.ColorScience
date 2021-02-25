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
    [DebuggerDisplay("R={R}, G={G}, B={B}")]
    public partial struct ColorRGB : IEquatable<ColorRGB>
    {
        /// <summary>
        /// R
        /// </summary>
        public float R;
        /// <summary>
        /// G
        /// </summary>
        public float G;
        /// <summary>
        /// B
        /// </summary>
        public float B;

        public bool IsInGamut => R >= 0 && G >= 0 && B >= 0 && R <= 1 && G <= 1 && B <= 1;

        public ColorRGB(float R, float G, float B)
        {
            this.R = R;
            this.G = G;
            this.B = B;
        }

        public static ColorRGB operator +(ColorRGB p, ColorRGB q) => new ColorRGB(p.R + q.R, p.G + q.G, p.B + q.B);
        public static ColorRGB operator -(ColorRGB p, ColorRGB q) => new ColorRGB(p.R - q.R, p.G - q.G, p.B - q.B);
        public static ColorRGB operator *(ColorRGB p, ColorRGB q) => new ColorRGB(p.R * q.R, p.G * q.G, p.B * q.B);
        public static ColorRGB operator /(ColorRGB p, ColorRGB q) => new ColorRGB(p.R / q.R, p.G / q.G, p.B / q.B);
        public static ColorRGB operator *(ColorRGB c, float m) => new ColorRGB(c.R * m, c.G * m, c.B * m);
        public static ColorRGB operator /(ColorRGB c, float d) => new ColorRGB(c.R / d, c.G / d, c.B / d);
        public static bool operator ==(ColorRGB left, ColorRGB right) => left.Equals(right);
        public static bool operator !=(ColorRGB left, ColorRGB right) => !(left == right);

        public override string ToString() => $"{R},{G},{B}";
        public string ToString(string format) => string.Join(",", R.ToString(format), G.ToString(format), B.ToString(format));
        public float[] ToArray() => new[] { R, G, B };

        public void Normalize()
        {
            float max = Math.Max(R, Math.Max(G, B));
            R /= max;
            G /= max;
            B /= max;
        }
        public void Clip()
        {
            R = Math.Max(0, Math.Min(R, 1));
            G = Math.Max(0, Math.Min(G, 1));
            B = Math.Max(0, Math.Min(B, 1));
        }

        public override bool Equals(object obj) => obj is ColorRGB other && Equals(other);
        public bool Equals(ColorRGB other) => this.R == other.R && this.G == other.G && this.B == other.B;
        public override int GetHashCode() => R.GetHashCode() ^ G.GetHashCode() ^ B.GetHashCode();
        public void Deconstruct(out float R, out float G, out float B) { R = this.R; G = this.G; B = this.B; }
    }
}
