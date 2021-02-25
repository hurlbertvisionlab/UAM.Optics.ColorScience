//
// © 2016-2021 miloush.net. All rights reserved.
//

namespace UAM.Optics.ColorScience
{
    using System;

    [Serializable]
    public struct ColorTransformMatrix
    {
        public override string ToString() => $"{{{a}, {b}, {c}}}, {{{d}, {e}, {f}}}, {{{g}, {h}, {i}}}";

        private float a, b, c;
        private float d, e, f;
        private float g, h, i;

        public ColorTransformMatrix(float a, float b, float c, float d, float e, float f, float g, float h, float i)
        {
            this.a = a; this.b = b; this.c = c;
            this.d = d; this.e = e; this.f = f;
            this.g = g; this.h = h; this.i = i;
        }
        public ColorTransformMatrix(float[] matrix, int index = 0)
        {
            if (matrix == null)
                throw new ArgumentNullException(nameof(matrix));
            if (index + 9 < matrix.Length)
                throw new ArgumentException("9 elements required.");

            a = matrix[0]; b = matrix[1]; c = matrix[2];
            d = matrix[3]; e = matrix[4]; f = matrix[5];
            g = matrix[6]; h = matrix[7]; i = matrix[8];
        }
        internal ColorTransformMatrix(Color1931XYZ col1, Color1931XYZ col2, Color1931XYZ col3)
        {
            a = col1.X; b = col2.X; c = col3.X;
            d = col1.Y; e = col2.Y; f = col3.Y;
            g = col1.Z; h = col2.Z; i = col3.Z;
        }

        public ColorTransformMatrix Invert()
        {
            float det = Determinant;

            if (det == 0)
                throw new InvalidOperationException("This transform is not invertible.");

            return new ColorTransformMatrix
            (
                (e * i - f * h) / det, (c * h - b * i) / det, (b * f - c * e) / det,
                (f * g - d * i) / det, (a * i - c * g) / det, (c * d - a * f) / det,
                (d * h - e * g) / det, (b * g - a * h) / det, (a * e - b * d) / det
            );
        }
        public ColorTransformMatrix Invert(int iterations)
        {
            ColorTransformMatrix inverse = Invert();

            for (int i = 0; i < iterations; i++)
                inverse = inverse * (2 * Identity - this * inverse);

            return inverse;
        }

        public void Transform(float x1, float x2, float x3, out float y1, out float y2, out float y3)
        {
            y1 = a * x1 + b * x2 + c * x3;
            y2 = d * x1 + e * x2 + f * x3;
            y3 = g * x1 + h * x2 + i * x3;
        }
        internal ColorRGB Transform(Color1931XYZ xyz)
        {
            ColorRGB rgb;
            Transform(xyz.X, xyz.Y, xyz.Z, out rgb.R, out rgb.G, out rgb.B);
            return rgb;
        }
        public void TranslateTransform(float x1, float x2, float x3, out float y1, out float y2, out float y3)
        {
            y1 = a * x1 + d * x2 + g * x3;
            y2 = b * x1 + e * x2 + h * x3;
            y3 = c * x1 + f * x2 + i * x3;
        }

        public static ColorTransformMatrix GetRGBtoXYZ(RGBPrimaries<Color1931XYZ> primaries, Color1931XYZ white)
        {
            return GetRGBtoXYZ(primaries.Red, primaries.Green, primaries.Blue, white);
        }
        public static ColorTransformMatrix GetRGBtoXYZ(RGBPrimaries<Color1931xyY> primaries, Color1931XYZ white)
        {
            return GetRGBtoXYZ(primaries.Red.ToXYZ(), primaries.Green.ToXYZ(), primaries.Blue.ToXYZ(), white);
        }
        public static ColorTransformMatrix GetRGBtoXYZ(Color1931XYZ red, Color1931XYZ green, Color1931XYZ blue, Color1931XYZ white)
        {
            ColorTransformMatrix XYZ = new ColorTransformMatrix(red, green, blue);
            ColorTransformMatrix XYZinv = XYZ.Invert();

            float Sr, Sg, Sb;
            XYZinv.Transform(white.X, white.Y, white.Z, out Sr, out Sg, out Sb);
            ColorTransformMatrix M = XYZ;
            M.a *= Sr; M.b *= Sg; M.c *= Sb;
            M.d *= Sr; M.e *= Sg; M.f *= Sb;
            M.g *= Sr; M.h *= Sg; M.i *= Sb;

            return M;
        }
        public static ColorTransformMatrix GetRGBtoXYZ(Color1931XYZ xyz1, Color1931XYZ xyz2, Color1931XYZ xyz3, ColorRGB rgb1, ColorRGB rgb2, ColorRGB rgb3)
        {
            float a, b, c;
            float d, e, f;
            float g, h, i;

            Solve(xyz1.X, xyz2.X, xyz3.X, rgb1, rgb2, rgb3, out a, out b, out c);
            Solve(xyz1.Y, xyz2.Y, xyz3.Y, rgb1, rgb2, rgb3, out d, out e, out f);
            Solve(xyz1.Z, xyz2.Z, xyz3.Z, rgb1, rgb2, rgb3, out g, out h, out i);

            return new ColorTransformMatrix(a, b, c, d, e, f, g, h, i);
        }
        private static void Solve(float x1, float x2, float x3, ColorRGB rgb1, ColorRGB rgb2, ColorRGB rgb3, out float m1, out float m2, out float m3)
        {
            // Cramer's rule

            ColorTransformMatrix A = new ColorTransformMatrix
            (
                rgb1.R, rgb1.G, rgb1.B,
                rgb2.R, rgb2.G, rgb2.B,
                rgb3.R, rgb3.G, rgb3.B
            );

            ColorTransformMatrix A1 = new ColorTransformMatrix
            (
                x1, rgb1.G, rgb1.B,
                x2, rgb2.G, rgb2.B,
                x3, rgb3.G, rgb3.B
            );
            ColorTransformMatrix A2 = new ColorTransformMatrix
            (
                rgb1.R, x1, rgb1.B,
                rgb2.R, x2, rgb2.B,
                rgb3.R, x3, rgb3.B
            );
            ColorTransformMatrix A3 = new ColorTransformMatrix
            (
                rgb1.R, rgb1.G, x1,
                rgb2.R, rgb2.G, x2,
                rgb3.R, rgb3.G, x3
            );


            float det = A.Determinant;

            m1 = A1.Determinant / det;
            m2 = A2.Determinant / det;
            m3 = A3.Determinant / det;
        }

        public static ColorTransformMatrix Get‌XYZtoRGB(RGBPrimaries<Color1931XYZ> primaries, Color1931XYZ white)
        {
            return GetRGBtoXYZ(primaries, white).Invert();
        }
        public static ColorTransformMatrix GetXYZtoRGB(Color1931XYZ red, Color1931XYZ green, Color1931XYZ blue, Color1931XYZ white)
        {
            return GetRGBtoXYZ(red, green, blue, white).Invert();
        }
        public static ColorTransformMatrix GetXYZtoRGB(Color1931XYZ xyz1, Color1931XYZ xyz2, Color1931XYZ xyz3, ColorRGB rgb1, ColorRGB rgb2, ColorRGB rgb3)
        {
            return GetRGBtoXYZ(xyz1, xyz2, xyz3, rgb1, rgb2, rgb3).Invert();
        }

        public float Determinant
        {
            get { return a * (e * i - f * h) - b * (d * i - f * g) + c * (d * h - e * g); }
        }

        internal void GetColumn(int index, out float r1, out float r2, out float r3)
        {
            switch (index)
            {
                case 0: r1 = a; r2 = d; r3 = g; return;
                case 1: r1 = b; r2 = e; r3 = h; return;
                case 2: r1 = c; r2 = f; r3 = i; return;
                default: throw new ArgumentOutOfRangeException(nameof(index));
            }
        }
        internal void GetRow(int index, out float c1, out float c2, out float c3)
        {
            switch (index)
            {
                case 0: c1 = a; c2 = b; c3 = c; return;
                case 1: c1 = d; c2 = e; c3 = f; return;
                case 2: c1 = g; c2 = h; c3 = i; return;
                default: throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        public static ColorTransformMatrix operator +(ColorTransformMatrix m1, ColorTransformMatrix m2)
        {
            return new ColorTransformMatrix(m1.a + m2.a, m1.b + m2.b, m1.c + m2.c,
                                            m1.d + m2.d, m1.e + m2.e, m1.f + m2.f,
                                            m1.g + m2.g, m1.h + m2.h, m1.i + m2.i);
        }
        public static ColorTransformMatrix operator -(ColorTransformMatrix m1, ColorTransformMatrix m2)
        {
            return new ColorTransformMatrix(m1.a - m2.a, m1.b - m2.b, m1.c - m2.c,
                                            m1.d - m2.d, m1.e - m2.e, m1.f - m2.f,
                                            m1.g - m2.g, m1.h - m2.h, m1.i - m2.i);
        }
        public static ColorTransformMatrix operator *(ColorTransformMatrix m1, ColorTransformMatrix m2)
        {
            return new ColorTransformMatrix(m1.a * m2.a + m1.b * m2.d + m1.c * m2.g, m1.a * m2.b + m1.b * m2.e + m1.c * m2.h, m1.a * m2.c + m1.b * m2.f + m1.c * m2.i,
                                            m1.d * m2.a + m1.e * m2.d + m1.f * m2.g, m1.d * m2.b + m1.e * m2.e + m1.f * m2.h, m1.d * m2.c + m1.e * m2.f + m1.f * m2.i,
                                            m1.g * m2.a + m1.h * m2.d + m1.i * m2.g, m1.g * m2.b + m1.h * m2.e + m1.i * m2.h, m1.g * m2.c + m1.h * m2.f + m1.i * m2.i);
        }
        public static ColorTransformMatrix operator *(float v, ColorTransformMatrix m) => m * v;
        public static ColorTransformMatrix operator *(ColorTransformMatrix m, float v)
        {
            return new ColorTransformMatrix(m.a * v, m.b * v, m.c * v,
                                            m.d * v, m.e * v, m.f * v,
                                            m.g * v, m.h * v, m.i * v);
        }
        public static ColorTransformMatrix operator /(ColorTransformMatrix m, float v)
        {
            return new ColorTransformMatrix(m.a / v, m.b / v, m.c / v,
                                            m.d / v, m.e / v, m.f / v,
                                            m.g / v, m.h / v, m.i / v);
        }

        private static readonly ColorTransformMatrix Identity = new ColorTransformMatrix
        (
                1,0,0,
                0,1,0,
                0,0,1
        );
        public static readonly ColorTransformMatrix AdobeRGB1998toXYZatD65 = new ColorTransformMatrix
        (
                0.5767309f, 0.1855540f, 0.1881852f,
                0.2973769f, 0.6273491f, 0.0752741f,
                0.0270343f, 0.0706872f, 0.9911085f
        );
        public static readonly ColorTransformMatrix AppleRGBtoXYZatD65 = new ColorTransformMatrix
        (
                0.4497288f, 0.3162486f, 0.1844926f,
                0.2446525f, 0.6720283f, 0.0833192f,
                0.0251848f, 0.1411824f, 0.9224628f
        );
        public static readonly ColorTransformMatrix BestRGBtoXYZatD50 = new ColorTransformMatrix
        (
                0.6326696f, 0.2045558f, 0.1269946f,
                0.2284569f, 0.7373523f, 0.0341908f,
                0.0000000f, 0.0095142f, 0.8156958f
        );
        public static readonly ColorTransformMatrix BetaRGBtoXYZatD50 = new ColorTransformMatrix
        (
                0.6712537f, 0.1745834f, 0.1183829f,
                0.3032726f, 0.6637861f, 0.0329413f,
                0.0000000f, 0.0407010f, 0.7845090f
        );
        public static readonly ColorTransformMatrix BruceRGBtoXYZatD65 = new ColorTransformMatrix
        (
                0.4674162f, 0.2944512f, 0.1886026f,
                0.2410115f, 0.6835475f, 0.0754410f,
                0.0219101f, 0.0736128f, 0.9933071f
        );
        public static readonly ColorTransformMatrix CIERGBtoXYZatE = new ColorTransformMatrix
        (
                0.4887180f, 0.3106803f, 0.2006017f,
                0.1762044f, 0.8129847f, 0.0108109f,
                0.0000000f, 0.0102048f, 0.9897952f
        );
        public static readonly ColorTransformMatrix ColorMatchRGBtoXYZatD50 = new ColorTransformMatrix
        (
                0.5093439f, 0.3209071f, 0.1339691f,
                0.2748840f, 0.6581315f, 0.0669845f,
                0.0242545f, 0.1087821f, 0.6921735f
        );
        public static readonly ColorTransformMatrix DonRGB4toXYZatD50 = new ColorTransformMatrix
        (
                0.6457711f, 0.1933511f, 0.1250978f,
                0.2783496f, 0.6879702f, 0.0336802f,
                0.0037113f, 0.0179861f, 0.8035125f
        );
        public static readonly ColorTransformMatrix ECIRGBtoXYZatD50 = new ColorTransformMatrix
        (
                0.6502043f, 0.1780774f, 0.1359384f,
                0.3202499f, 0.6020711f, 0.0776791f,
                0.0000000f, 0.0678390f, 0.7573710f
        );
        public static readonly ColorTransformMatrix EktaSpacePS5toXYZatD50 = new ColorTransformMatrix
        (
                0.5938914f, 0.2729801f, 0.0973485f,
                0.2606286f, 0.7349465f, 0.0044249f,
                0.0000000f, 0.0419969f, 0.7832131f
        );
        public static readonly ColorTransformMatrix NtscRGBtoXYZatC = new ColorTransformMatrix
        (
                0.6068909f, 0.1735011f, 0.2003480f,
                0.2989164f, 0.5865990f, 0.1144845f,
                0.0000000f, 0.0660957f, 1.1162243f
        );
        public static readonly ColorTransformMatrix PalSecamRGBtoXYZatD65 = new ColorTransformMatrix
        (
                0.4306190f, 0.3415419f, 0.1783091f,
                0.2220379f, 0.7066384f, 0.0713236f,
                0.0201853f, 0.1295504f, 0.9390944f
        );
        public static readonly ColorTransformMatrix ProPhotoRGBtoXYZatD50 = new ColorTransformMatrix
        (
                0.7976749f, 0.1351917f, 0.0313534f,
                0.2880402f, 0.7118741f, 0.0000857f,
                0.0000000f, 0.0000000f, 0.8252100f
        );
        public static readonly ColorTransformMatrix SMPTECtoXYZatD65 = new ColorTransformMatrix
        (
                0.3935891f, 0.3652497f, 0.1916313f,
                0.2124132f, 0.7010437f, 0.0865432f,
                0.0187423f, 0.1119313f, 0.9581563f
        );
        public static readonly ColorTransformMatrix sRGBtoXYZatD65 = new ColorTransformMatrix
        (
                0.4124564f, 0.3575761f, 0.1804375f,
                0.2126729f, 0.7151522f, 0.0721750f,
                0.0193339f, 0.1191920f, 0.9503041f
        );
        public static readonly ColorTransformMatrix WideGamutRGBtoXYZatD50 = new ColorTransformMatrix
        (
                0.7161046f, 0.1009296f, 0.1471858f,
                0.2581874f, 0.7249378f, 0.0168748f,
                0.0000000f, 0.0517813f, 0.7734287f
        );

        // Kaiser, Bonton (1996). Human Color Vision (Second Edition), p. 557 (A.3.14)
        //   slight difference from the original paper attributed to calculation error
        public static readonly ColorTransformMatrix SmithPokornyXYZtoLMS = new ColorTransformMatrix
        (            
                0.15516f, 0.54308f, -0.03287f,
               -0.15516f, 0.45692f,  0.03287f,
                0.00000f, 0.00000f,  0.01608f
        );

        // Kaiser, Bonton (1996). Human Color Vision (Second Edition), Appendix, Part IV
        public static ColorTransformMatrix GetScaledLMStoDKL(ColorLMS background)
        {
            ColorTransformMatrix LMStoDKL = new ColorTransformMatrix
            (
                1, 1, 0,
                1, -background.L / background.M, 0,
                -1, -1, (background.L + background.M) / background.S
            );

            ColorTransformMatrix DKLtoLMS = LMStoDKL.Invert();

            ColorLMS isolatingIso = new ColorLMS(DKLtoLMS.a, DKLtoLMS.d, DKLtoLMS.g);
            float    pooledConeContrastIso = (isolatingIso / background).Length;
            ColorLMS normalizedIso = isolatingIso / pooledConeContrastIso;

            ColorLMS isolatingRGiso = new ColorLMS(DKLtoLMS.b, DKLtoLMS.e, DKLtoLMS.h);
            float    pooledConeCotnrastRGiso = (isolatingRGiso / background).Length;
            ColorLMS normalizedRGiso = isolatingRGiso / pooledConeCotnrastRGiso;

            ColorLMS isolatingSiso = new ColorLMS(DKLtoLMS.c, DKLtoLMS.f, DKLtoLMS.i);
            float    pooledConeContrastSiso = (isolatingSiso / background).Length;
            ColorLMS normalizedSiso = isolatingSiso / pooledConeContrastSiso;

            ColorDKL unitResponse;
            LMStoDKL.Transform(normalizedIso.L, normalizedIso.M, normalizedIso.S, out unitResponse.Isochromatic, out _, out _);
            LMStoDKL.Transform(normalizedRGiso.L, normalizedRGiso.M, normalizedRGiso.S, out _, out unitResponse.RGisoluminant, out _);
            LMStoDKL.Transform(normalizedSiso.L, normalizedSiso.M, normalizedSiso.S, out _, out _, out unitResponse.Sisoluminant);

            ColorTransformMatrix rescaler = new ColorTransformMatrix
            (
                1 / unitResponse.Isochromatic, 0, 0,
                0, 1 / unitResponse.RGisoluminant, 0,
                0, 0, 1 / unitResponse.Sisoluminant
            );

            return rescaler * LMStoDKL;
        }
    }
}
