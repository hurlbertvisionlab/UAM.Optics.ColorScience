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
    [DebuggerDisplay("L*={L}, u*={u}, v*={v}")]
    public partial struct Color1976Luv : IEquatable<Color1976Luv>
    {
        /// <summary>
        /// L*
        /// </summary>
        public float L;
        /// <summary>
        /// u*
        /// </summary>
        public float u;
        /// <summary>
        /// v*
        /// </summary>
        public float v;

        public float C => (float)Math.Sqrt(u * u + v * v);
        public float h => (float)Math.Atan2(v, u);
        public float s => C / L;

        public float hDeg
        {
            get
            {
                double angle = h * 180 / Math.PI;
                if (angle > 0)
                    return (float)angle;

                return (float)(360 + angle);
            }
        }

        public Color1976Luv(float L, float u, float v)
        {
            this.L = L;
            this.u = u;
            this.v = v;
        }

        public static Color1976Luv operator +(Color1976Luv p, Color1976Luv q) => new Color1976Luv(p.L + q.L, p.u + q.u, p.v + q.v);
        public static Color1976Luv operator -(Color1976Luv p, Color1976Luv q) => new Color1976Luv(p.L - q.L, p.u - q.u, p.v - q.v);
        public static Color1976Luv operator *(Color1976Luv c, float m) => new Color1976Luv(c.L * m, c.u * m, c.v * m);
        public static Color1976Luv operator /(Color1976Luv c, float d) => new Color1976Luv(c.L / d, c.u / d, c.v / d);

        public static bool operator ==(Color1976Luv left, Color1976Luv right) => left.Equals(right);
        public static bool operator !=(Color1976Luv left, Color1976Luv right) => !(left == right);

        public override string ToString() => $"{L},{u},{v}";
        public string ToString(string format) => string.Join(",", L.ToString(format), u.ToString(format), v.ToString(format));
        public float[] ToArray() => new[] { L, u, v };

        public double DistanceTo(Color1976Luv other, bool useL = true)
        {
            float dL = (this.L - other.L) * (this.L - other.L);
            float du = (this.u - other.u) * (this.u - other.u);
            float dv = (this.v - other.v) * (this.v - other.v);

            if (!useL)
                dL = 0f;

            return Math.Sqrt(dL + du + dv);
        }

        public override bool Equals(object obj) => obj is Color1976Luv other && Equals(other);
        public bool Equals(Color1976Luv other) => this.L == other.L && this.u == other.u && this.v == other.v;
        public override int GetHashCode() => L.GetHashCode() ^ u.GetHashCode() ^ v.GetHashCode();
        public void Deconstruct(out float L, out float u, out float v) { L = this.L; u = this.u; v = this.v; }
    }
}
