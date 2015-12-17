using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitSynth
{
    class ADSR
    {
        private enum envState : int
        {
            env_idle,
            env_attack,
            env_decay,
            env_sustain,
            env_release
        };

        protected int state;
        protected float output;
        static protected float attackRate;
        protected float decayRate;
        protected float releaseRate;
        static protected float attackCoef;
        protected float decayCoef;
        protected float releaseCoef;
        protected float sustainLevel;
        protected float targetRatioA;
        protected float targetRatioDR;
        static protected float attackBase;
        protected float decayBase;
        protected float releaseBase;

        public ADSR()
        {
            reset();
            setAttackRate(0);
            setDecayRate(0);
            setReleaseRate(0);
            setSustainLevel(1.0f);
            setTargetRatioA(0.3f);
            //setTargetRatioDR(0.0001f);
            setTargetRatioDR(0.1f);
        }
        ~ADSR()
        {

        }
	    public float process()
        {
            switch (state)
            {
                case (int)envState.env_idle:
                    break;
                case (int)envState.env_attack:
                    output = attackBase + output * attackCoef;
                    if (output >= 1.0f)
                    {
                        output = 1.0f;
                        state = (int)envState.env_decay;
                    }
                    break;
                case (int)envState.env_decay:
                    output = decayBase + output * decayCoef;
                    if (output <= sustainLevel)
                    {
                        output = sustainLevel;
                        state = (int)envState.env_sustain;
                    }
                    break;
                case (int)envState.env_sustain:
                    break;
                case (int)envState.env_release:
                    output = releaseBase + output * releaseCoef;

                    if (output <= 0.0f)
                    {
                        output = 0.0f;
                        state = (int)envState.env_idle;
                    }
                    break;
            }
            return output;
        }
        public float getOutput()
        {
            return output;
        }
        public int getState()
        {
            return state;
        }
        public void gate(bool on)
        {
            if (on)
                state = (int)envState.env_attack;
            else if (state != (int)envState.env_idle)
                state = (int)envState.env_release;
        }
        public void setAttackRate(float rate)
        {
            attackRate = rate;
            attackCoef = calcCoef(rate, targetRatioA);
            attackBase = (1.0f + targetRatioA) * (1.0f - attackCoef);
        }
        public void setDecayRate(float rate)
        {
            decayRate = rate;
            decayCoef = calcCoef(rate, targetRatioDR);
            decayBase = (sustainLevel - targetRatioDR) * (1.0f - decayCoef);
        }
        public void setReleaseRate(float rate)
        {
            releaseRate = rate;
            releaseCoef = calcCoef(rate, targetRatioDR);
            releaseBase = -targetRatioDR * (1.0f - releaseCoef);
        }
        public void setSustainLevel(float level)
        {
            sustainLevel = level;
            decayBase = (sustainLevel - targetRatioDR) * (1.0f - decayCoef);
        }
        public void setTargetRatioA(float targetRatio)
        {
            if (targetRatio < 0.000000001f)
                targetRatio = 0.000000001f;  // -180 dB
            targetRatioA = targetRatio;
            attackBase = (1.0f + targetRatioA) * (1.0f - attackCoef);
        }
        public void setTargetRatioDR(float targetRatio)
        {
            if (targetRatio < 0.000000001f)
                targetRatio = 0.000000001f;  // -180 dB
            targetRatioDR = targetRatio;
            decayBase = (sustainLevel - targetRatioDR) * (1.0f - decayCoef);
            releaseBase = -targetRatioDR * (1.0f - releaseCoef);
        }
        public bool IsOutputZero()
        {
            return output <= 0.0f;
        }

        // private

        private void reset()
        {
            state = (int)envState.env_idle;
            output = 0.0f;
        }

        private float calcCoef(float rate, float targetRatio)
        {
            return (float)Math.Exp(-Math.Log((1.0 + targetRatio) / targetRatio) / rate);
        }


    }
}


/*
// create ADSR env
ADSR *env = new ADSR();

// initialize settings
env->setAttackRate(.1 * sampleRate);  // .1 second
env->setDecayRate(.3 * sampleRate);
env->setReleaseRate(5 * sampleRate);
env->setSustainLevel(.8);
…
// at some point, by MIDI perhaps, the envelope is gated "on"
env->gate(true);
…
// and some time later, it's gated "off"
env->gate(false)

// env->process() to generate and return the ADSR output...
outBuf[idx] = filter->process(osc->getOutput()) * env->process();

*/
