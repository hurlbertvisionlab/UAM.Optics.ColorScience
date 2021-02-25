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
    [DebuggerDisplay("L*={L}, a*={a}, b*={b}")]
    public partial struct Color1976Lab : IEquatable<Color1976Lab>
    {
        public static readonly Color1976Lab White = new Color1976Lab(100, 0, 0);
        public static readonly Color1976Lab Black = new Color1976Lab(0, 0, 0);

        public float L;
        public float a;
        public float b;

        public float C => (float)Math.Sqrt(a * a + b * b);
        public float h => (float)Math.Atan2(b, a);
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

        public Color1976Lab(float L, float a, float b)
        {
            this.L = L;
            this.a = a;
            this.b = b;
        }

        public static Color1976Lab operator +(Color1976Lab p, Color1976Lab q) => new Color1976Lab(p.L + q.L, p.a + q.a, p.b + q.b);
        public static Color1976Lab operator -(Color1976Lab p, Color1976Lab q) => new Color1976Lab(p.L - q.L, p.a - q.a, p.b - q.b);
        public static Color1976Lab operator *(Color1976Lab c, float m) => new Color1976Lab(c.L * m, c.a * m, c.b * m);
        public static Color1976Lab operator /(Color1976Lab c, float d) => new Color1976Lab(c.L / d, c.a / d, c.b / d);
        public static bool operator ==(Color1976Lab left, Color1976Lab right) => left.Equals(right);
        public static bool operator !=(Color1976Lab left, Color1976Lab right) => !(left == right);

        public override string ToString() => $"{L},{a},{b}";
        public string ToString(string format) => string.Join(",", L.ToString(format), a.ToString(format), b.ToString(format));
        public float[] ToArray() => new[] { L, a, b };

        public double DistanceTo(Color1976Lab other, bool useL = true)
        {
            float dL = (this.L - other.L) * (this.L - other.L);
            float da = (this.a - other.a) * (this.a - other.a);
            float db = (this.b - other.b) * (this.b - other.b);

            if (!useL)
                dL = 0f;

            return Math.Sqrt(dL + da + db);
        }

        public override bool Equals(object obj) => obj is Color1976Lab other && Equals(other);
        public bool Equals(Color1976Lab other) => this.L == other.L && this.a == other.a && this.b == other.b;
        public override int GetHashCode() => L.GetHashCode() ^ a.GetHashCode() ^ b.GetHashCode();
        public void Deconstruct(out float L, out float a, out float b) { L = this.L; a = this.a; b = this.b; }
    }
}
