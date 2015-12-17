using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Midi;

namespace BitSynth
{
    class KeyPressed
    {
        private static bool[] pitchesPressed;

        public KeyPressed()
        {
            pitchesPressed = new bool[128];
            for(int i = 0; i < 128; i++)
            {
                pitchesPressed[i] = false;
            }
        }
        ~KeyPressed()
        {

        }

        public void setKeyPressed(int key, bool isPressed)
        {
            KeyPressed.pitchesPressed[key] = isPressed;
        }
        public void setKeyPressed(bool[] pitches)
        {
            KeyPressed.pitchesPressed = pitches;
        }

        public bool[] getKeyPressed()
        {
            return pitchesPressed;
        }
    }
}
