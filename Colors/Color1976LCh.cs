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
    [DebuggerDisplay("L={L}, C={C}, h={h}")]
    public partial struct Color1976LCh : IEquatable<Color1976LCh>
    {
        /// <summary>
        /// L*
        /// </summary>
        public float L;
        /// <summary>
        /// C*
        /// </summary>
        public float C;
        /// <summary>
        /// h*
        /// </summary>
        public float h;

        public float u => a;
        public float v => b;
        public float a => C * (float)Math.Cos(h);
        public float b => C * (float)Math.Sin(h);
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

        public Color1976LCh(float L, float C, float h)
        {
            this.L = L;
            this.C = C;
            this.h = h;
        }

        public static explicit operator Color1976Luv(Color1976LCh LCh) => new Color1976Luv(LCh.L, LCh.u, LCh.v);
        public static explicit operator Color1976Lab(Color1976LCh LCh) => new Color1976Lab(LCh.L, LCh.a, LCh.b);

        public static Color1976LCh operator +(Color1976LCh p, Color1976LCh q) => new Color1976LCh(p.L + q.L, p.C + q.C, p.h + q.h);
        public static Color1976LCh operator -(Color1976LCh p, Color1976LCh q) => new Color1976LCh(p.L - q.L, p.C - q.C, p.h - q.h);
        public static Color1976LCh operator *(Color1976LCh c, float m) => new Color1976LCh(c.L * m, c.C * m, c.h * m);
        public static Color1976LCh operator /(Color1976LCh c, float d) => new Color1976LCh(c.L / d, c.C / d, c.h / d);
        public static bool operator ==(Color1976LCh left, Color1976LCh right) => left.Equals(right);
        public static bool operator !=(Color1976LCh left, Color1976LCh right) => !(left == right);

        public override string ToString() => $"{L},{C},{h}";
        public string ToString(string format) => string.Join(",", L.ToString(format), C.ToString(format), h.ToString(format));
        public float[] ToArray() => new[] { L, C, h };

        public override bool Equals(object obj) => obj is Color1976LCh other && Equals(other);
        public bool Equals(Color1976LCh other) => this.L == other.L && this.C == other.C && this.h == other.h;
        public override int GetHashCode() => L.GetHashCode() ^ C.GetHashCode() ^ h.GetHashCode();
        public void Deconstruct(out float L, out float C, out float h) { L = this.L; C = this.u; h = this.v; }
    }
}
