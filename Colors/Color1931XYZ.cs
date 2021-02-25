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
    [DebuggerDisplay("X={X}, Y={Y}, Z={Z}")]
    public partial struct Color1931XYZ : IEquatable<Color1931XYZ>
    {
        /// <summary>
        /// X
        /// </summary>
        public float X;
        /// <summary>
        /// Y
        /// </summary>
        public float Y;
        /// <summary>
        /// Z
        /// </summary>
        public float Z;

        public Color1931XYZ(float X, float Y, float Z)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }

        public void Normalize()
        {
            float max = Math.Max(X, Math.Max(Y, Z));
            if (max > 1)
            {
                X /= max;
                Y /= max;
                Z /= max;
            }
        }

        public static explicit operator Color1931xyY(Color1931XYZ c) => ConvertColor.ToxyY(c);

        public static Color1931XYZ operator +(Color1931XYZ a, Color1931XYZ b) => new Color1931XYZ(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        public static Color1931XYZ operator -(Color1931XYZ a, Color1931XYZ b) => new Color1931XYZ(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        public static Color1931XYZ operator *(Color1931XYZ c, float m) => new Color1931XYZ(c.X * m, c.Y * m, c.Z * m);
        public static Color1931XYZ operator /(Color1931XYZ c, float d) => new Color1931XYZ(c.X / d, c.Y / d, c.Z / d);
        public static bool operator ==(Color1931XYZ left, Color1931XYZ right) => left.Equals(right);
        public static bool operator !=(Color1931XYZ left, Color1931XYZ right) => !(left == right);

        public override string ToString() => $"{X},{Y},{Z}";
        public string ToString(string format) => string.Join(",", X.ToString(format), Y.ToString(format), Z.ToString(format));

        public override bool Equals(object obj) => obj is Color1931XYZ other && Equals(other);
        public bool Equals(Color1931XYZ other) => this.X == other.X && this.Y == other.Y && this.Z == other.Z;
        public override int GetHashCode() => X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
        public void Deconstruct(out float X, out float Y, out float Z) { X = this.X; Y = this.Y; Z = this.Z; }
    }
}
