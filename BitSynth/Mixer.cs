using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitSynth
{
    class Mixer
    {

        public Oscillator[] oscillator { get; set; }
        ADSR env;
        bool lastKeyPressed;
        NoteTable noteTable;

        public Mixer()
        {
            oscillator = new Oscillator[WaveInfo.OscillatorNum];
            for (int i = 0; i < WaveInfo.OscillatorNum; i++)
            {
                oscillator[i] = new Oscillator();
            }
            env = new ADSR();
            lastKeyPressed = false;
            noteTable = new NoteTable();
        }
        ~Mixer()
        {

        }

        public double Mix(double[] sigs, double[] vals, int num)
        {
            // Sig    -1.0～1.0
            // valume 0.0～1.0

            double sum = 0.0;
            for(int i=0;i<num; i++)
            {
                sum += sigs[i]*vals[i];
            }
            return sum / (double)num;
        }

        public double mixerProcess(WaveInfo waveinfo, int key, bool keyPressed)
        {
            double Sig = 0.0f;

            if (!lastKeyPressed && keyPressed)
            {
                env.gate(true);
            }
            if (lastKeyPressed && !keyPressed)
            {
                env.gate(false);
            }
            lastKeyPressed = keyPressed;



            float envelope = env.process();

            if (env.IsOutputZero()) return 0;

            env.setAttackRate(WaveInfo.Attack);
            env.setDecayRate(WaveInfo.Decay);
            env.setSustainLevel(WaveInfo.Sustain);
            env.setReleaseRate(WaveInfo.Release);

            double frequency = 0.0f;

            for (int i = 0; i < waveinfo.getOscillatorNum(); i++)
            {
                int midikey = key + WaveInfo.coarse[i];
                if (midikey < 0) midikey = 0;
                if (midikey > 127) midikey = 127;
                frequency = noteTable.MtoF(midikey);
                oscillator[i].setWaveType(WaveInfo.waveType[i]);
                oscillator[i].setVol(WaveInfo.volume[i]);
                double OutputFrequency = frequency + WaveInfo.detune[i];
                if (OutputFrequency < 1) OutputFrequency = 1;
                Sig += oscillator[i].oscillatorProcess(OutputFrequency) * oscillator[i].getVol(); // Volume 0～1
            }
            return Sig / (double)waveinfo.getOscillatorNum() * envelope;
        }
    }
}
