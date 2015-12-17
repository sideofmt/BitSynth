using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitSynth
{
    class NoteTable
    {
        private float[] table;
        private static readonly float baseFrequency = 440.0f;


        public NoteTable()
        {
            table = new float[128];
            for(int i = 0; i < 128; i++)
            {
                table[i] = (float)(baseFrequency * Math.Pow(2, (float)(i - 70) / (float)12));
            }
        }

        public float MtoF(int noteNumber)
        {
            return table[noteNumber];
        }
    }
}
