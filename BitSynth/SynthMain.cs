using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Midi;

namespace BitSynth
{
    class SynthMain
    {
        private double m_Sig;
        private WaveInfo waveinfo;
        NoteTable noteTable;
        Delay delay;

        // Module
        //public Oscillator[] oscillator { get; set; }
        Mixer[] mixer;

        // Oscillator


        public SynthMain()
        {

            waveinfo = new WaveInfo();
            mixer = new Mixer[128];
            for(int i=0;i< 128; i++)
            {
                mixer[i] = new Mixer();
            }
            noteTable = new NoteTable();
            key = new KeyPressed();
            delay = new Delay();
        }

        public void synthProcess(ref float right,ref float left)
        {

            /*
            Oscillator
            */

            pitchPressed = key.getKeyPressed();
            m_Sig = 0.0;
            //int keyPressedNum = 0;

            for (int i=0; i<128;i++)
            {
                    m_Sig += mixer[i].mixerProcess(waveinfo, i, pitchPressed[i]);
                    //if (pitchPressed[i]) keyPressedNum++;
            }
            //m_Sig /= keyPressedNum;
            //m_Sig = delay.delayProcess(m_Sig);

            m_Sig *= (double)WaveInfo.MasterVolume;

            if (m_Sig > 1)
            {
                m_Sig = 1;
            }
            if (m_Sig < -1)
            {
                m_Sig = -1;
            }

            right = (float)m_Sig;
            left = (float)m_Sig;
        }

        KeyPressed key;
        bool[] pitchPressed;

    }
}
