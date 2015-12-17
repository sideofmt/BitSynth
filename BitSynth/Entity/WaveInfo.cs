using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitSynth
{
    class WaveInfo
    {
        public static readonly int Sine = 0;
        public static readonly int Saw = 1;
        public static readonly int Pulse = 2;
        public static readonly int Triangle = 3;
        public static readonly int Noise = 4;

        public double sampleRate = 44100;
        public double frequency = 880;
        public static readonly int OscillatorNum = 3;

        public static float MasterVolume;

        // 何サンプル分の長さか(Sustainのみ0～1の音量)
        public static float Attack = 0;
        public static float Decay = 0;
        public static float Sustain = 1.0f;
        public static float Release = 0;

        public static int[] waveType = new int[OscillatorNum];
        public static float[] volume = new float[OscillatorNum];
        public static float[] detune = new float[OscillatorNum];
        public static int[] coarse = new int[OscillatorNum];

        public WaveInfo()
        {
            for (int i = 0; i < OscillatorNum; i++)
            {
                waveType[i] = Sine;
                volume[i] = 1.0f;
                detune[i] = 0.0f;
                coarse[i] = 0;
            }
            MasterVolume = 1.0f;
        }

        public int getOscillatorNum()
        {
            return OscillatorNum;
        }
    }
}
