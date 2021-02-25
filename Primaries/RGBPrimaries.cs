//
// © 2016-2021 miloush.net. All rights reserved.
//

namespace UAM.Optics.ColorScience
{
    using System;

    [Serializable]
    public struct RGBPrimaries<TColor>
    {
        public override string ToString() => $"Red={{{Red}}}, Green={{{Green}}}, Blue={{{Blue}}}";

        public TColor Red;
        public TColor Green;
        public TColor Blue;

        public RGBPrimaries(TColor red, TColor green, TColor blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }
    }
}
