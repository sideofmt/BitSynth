using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitSynth
{
    class Delay
    {
        private double[] buf;
        private double sampleRate;
        private int count;
        public static int delayFlame;
        public static float mix;
        public static float feedback;

        public Delay()
        {
            sampleRate = 44100;
            buf = new double[(int)sampleRate*5];
            for (int i = 0; i < (int)sampleRate * 5; i++)
            {
                buf[i] = 0.0;
            }

            count = 0;
            delayFlame = 44100;
            mix = 0.5f;
            feedback = 0.5f;

        }
        ~Delay()
        {

        }
        public double delayProcess(double Sig)
        {
            double output;

            if (count >= (int)sampleRate * 5) count = 0;
            int d = count + delayFlame;
            if (d >= (int)sampleRate * 5) d -= (int)sampleRate * 5;


            output = Sig + buf[count] * mix;
            buf[d] = Sig + buf[count] * feedback;

            count++;
            return output;
        }

        public bool IsGainZero()
        {
            return mix <= 0;
        }

        //public void setFeedback(float feedback)
        //{
        //    this.feedback = feedback;
        //}
        //public void setMix(float mix)
        //{
        //    this.mix = mix;
        //}
        //public void setDelayFlame(int delayFlame)
        //{
        //    this.delayFlame = delayFlame;
        //}

    }
}
