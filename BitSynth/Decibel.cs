using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitSynth
{
    class Decibel
    {
        public static double ValueToDecibel(double value)
        {
            return 20 * Math.Log10(value);
        }   

        public static double DecibelToValue(double dB)
        {
            // dBは0～1の値
            if (dB <= -60) return 0.0;
            return Math.Pow(10, dB / (double)20);
        }
    }
}
