//
// © 2016-2021 miloush.net. All rights reserved.
//

namespace UAM.Optics.ColorScience
{
    using System;

    public class ColorSpectrum
    {
        public int FirstNanometers { get; }
        public int LastNanometers => FirstNanometers + Samples.Length * StepNanometers; // technically the last measurement integrates the last step
        public int StepNanometers { get; }

        public float[] Samples { get; }

        public ColorSpectrum(int nmFirst, int nmStep, params float[] spectrum)
        {
            if (spectrum == null)
                throw new ArgumentNullException(nameof(spectrum));

            FirstNanometers = nmFirst;
            StepNanometers = nmStep;
            Samples = spectrum;
        }

        public bool GetValueAt(int nm, out float value)
        {
            if (nm < FirstNanometers || nm > LastNanometers) // no linear interpolation to zereos
            {
                value = 0;
                return false;
            }

            int i = (nm - FirstNanometers) / StepNanometers;
            value = Samples[i];

            int m = nm % StepNanometers;
            if (m == 0)
                return true;

            i++;
            float high = Samples[i];
            float w = (1f / StepNanometers) * m;

            value = (1f - w) * value + w * high;
            return true;
        }

        public Color1931XYZ ToXYZ(ColorMatchingFunctions.XYZ cmf)
        {
            return ConvertColor.ToXYZ(Samples, FirstNanometers, StepNanometers, cmf);
        }
    }
}
