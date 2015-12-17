using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BitSynth
{
    class Proragm
    {

        /// <summary>
        /// SharpDX XAudio2 sample. Plays a generated sound with some reverb.
        /// </summary>
        [MTAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new PlayForm());
        }
    }
}
