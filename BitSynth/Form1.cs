using System;
using System.Windows.Forms;
using SharpDX;
using SharpDX.Multimedia;
using SharpDX.XAudio2;
using SharpDX.XAudio2.Fx;

namespace BitSynth
{
    public partial class PlayForm : Form
    {
        private XAudio2 xaudio2;
        private MasteringVoice masteringVoice;
        private SourceVoice sourceVoice;
        private AudioBuffer audioBuffer;
        private EffectDescriptor modulatorDescriptor;
        private Reverb reverb;
        private EffectDescriptor effectDescriptor;

        WaveInfo waveinfo;
        KeyPressed key;

       private  double sampleRate;

        public PlayForm()
        {
            InitializeComponent();

            // Initalize XAudio2
            xaudio2 = new XAudio2(XAudio2Flags.None, ProcessorSpecifier.DefaultProcessor);
            masteringVoice = new MasteringVoice(xaudio2);

            var waveFormat = new WaveFormat(44100, 32, 2);
            sourceVoice = new SourceVoice(xaudio2, waveFormat);

            int bufferSize = waveFormat.ConvertLatencyToByteSize(60000);
            DataStream dataStream = new DataStream(bufferSize, true, true);

            // Prepare the initial sound to modulate

            int numberOfSamples = bufferSize / waveFormat.BlockAlign;
            for (int i = 0; i < numberOfSamples; i++)
            {
                float value = (float)(Math.Cos(2 * Math.PI * 220.0 * i / waveFormat.SampleRate) * 0.5);
                dataStream.Write(value);
                dataStream.Write(value);
            }
            dataStream.Position = 0;

            // My Writing
            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(Form1_keyDown);
            this.KeyUp += new KeyEventHandler(Form1_keyUp);


            audioBuffer = new AudioBuffer
            {
                Stream = dataStream,
                Flags = BufferFlags.EndOfStream,
                AudioBytes = bufferSize,
                LoopBegin = 0,
                LoopLength = numberOfSamples,
                LoopCount = AudioBuffer.LoopInfinite
            };

            // Set the effect on the source
            ModulatorEffect = new ModulatorEffect();
            modulatorDescriptor = new EffectDescriptor(ModulatorEffect);
            reverb = new Reverb(/*xaudio2*/);
            effectDescriptor = new EffectDescriptor(reverb);
            //sourceVoice.SetEffectChain(modulatorDescriptor, effectDescriptor);
            sourceVoice.SetEffectChain(modulatorDescriptor);
            //sourceVoice.EnableEffect(0);

            this.Closed += new EventHandler(PlayForm_Closed);

            // My Writing
            sampleRate = waveFormat.SampleRate;
            ModulatorEffect.setSampleRate(sampleRate);

            waveinfo = new WaveInfo();
            midiInput = new MidiMessageInput();
            midiInput.addItem(listBox1);
            midiDeviceDecided = false;

            key = new KeyPressed();

            this.sine.Checked = true;
            this.sine2.Checked = true;
            this.sine3.Checked = true;

            sourceVoice.SubmitSourceBuffer(audioBuffer, null);
            sourceVoice.Start();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                    components.Dispose();
                if (masteringVoice != null)
                    masteringVoice.Dispose();
                if (xaudio2 != null)
                    xaudio2.Dispose();
            }
            base.Dispose(disposing);
        }

        void PlayForm_Closed(object sender, EventArgs e)
        {
            sourceVoice.Stop(PlayFlags.None, 0);
            xaudio2.StopEngine();
        }

        public ModulatorEffect ModulatorEffect { get; set; }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            ModulatorEffect.m_Intensity = trackBar1.Value / 100.0f;
        }



        void Form1_keyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Q:
                    key.setKeyPressed(60, true);
                    break;
                case Keys.D2:
                    key.setKeyPressed(61, true);
                    break;
                case Keys.W:
                    key.setKeyPressed(62, true);
                    break;
                case Keys.D3:
                    key.setKeyPressed(63, true);
                    break;
                case Keys.E:
                    key.setKeyPressed(64, true);
                    break;
                case Keys.R:
                    key.setKeyPressed(65, true);
                    break;
                case Keys.D5:
                    key.setKeyPressed(66, true);
                    break;
                case Keys.T:
                    key.setKeyPressed(67, true);
                    break;
                case Keys.D6:
                    key.setKeyPressed(68, true);
                    break;
                case Keys.Y:
                    key.setKeyPressed(69, true);
                    break;
                case Keys.D7:
                    key.setKeyPressed(70, true);
                    break;
                case Keys.U:
                    key.setKeyPressed(71, true);
                    break;
                case Keys.I:
                    key.setKeyPressed(72, true);
                    break;

                case Keys.Z:
                    key.setKeyPressed(72, true);
                    break;
                case Keys.S:
                    key.setKeyPressed(73, true);
                    break;
                case Keys.X:
                    key.setKeyPressed(74, true);
                    break;
                case Keys.D:
                    key.setKeyPressed(75, true);
                    break;
                case Keys.C:
                    key.setKeyPressed(76, true);
                    break;
                case Keys.V:
                    key.setKeyPressed(77, true);
                    break;
                case Keys.G:
                    key.setKeyPressed(78, true);
                    break;
                case Keys.B:
                    key.setKeyPressed(79, true);
                    break;
                case Keys.H:
                    key.setKeyPressed(80, true);
                    break;
                case Keys.N:
                    key.setKeyPressed(81, true);
                    break;
                case Keys.J:
                    key.setKeyPressed(82, true);
                    break;
                case Keys.M:
                    key.setKeyPressed(83, true);
                    break;
                case Keys.Oemcomma:
                    key.setKeyPressed(84, true);
                    break;
            }
        }

        void Form1_keyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Q:
                    key.setKeyPressed(60, false);
                    break;
                case Keys.D2:
                    key.setKeyPressed(61, false);
                    break;
                case Keys.W:
                    key.setKeyPressed(62, false);
                    break;
                case Keys.D3:
                    key.setKeyPressed(63, false);
                    break;
                case Keys.E:
                    key.setKeyPressed(64, false);
                    break;
                case Keys.R:
                    key.setKeyPressed(65, false);
                    break;
                case Keys.D5:
                    key.setKeyPressed(66, false);
                    break;
                case Keys.T:
                    key.setKeyPressed(67, false);
                    break;
                case Keys.D6:
                    key.setKeyPressed(68, false);
                    break;
                case Keys.Y:
                    key.setKeyPressed(69, false);
                    break;
                case Keys.D7:
                    key.setKeyPressed(70, false);
                    break;
                case Keys.U:
                    key.setKeyPressed(71, false);
                    break;
                case Keys.I:
                    key.setKeyPressed(72, false);
                    break;

                case Keys.Z:
                    key.setKeyPressed(72, false);
                    break;
                case Keys.S:
                    key.setKeyPressed(73, false);
                    break;
                case Keys.X:
                    key.setKeyPressed(74, false);
                    break;
                case Keys.D:
                    key.setKeyPressed(75, false);
                    break;
                case Keys.C:
                    key.setKeyPressed(76, false);
                    break;
                case Keys.V:
                    key.setKeyPressed(77, false);
                    break;
                case Keys.G:
                    key.setKeyPressed(78, false);
                    break;
                case Keys.B:
                    key.setKeyPressed(79, false);
                    break;
                case Keys.H:
                    key.setKeyPressed(80, false);
                    break;
                case Keys.N:
                    key.setKeyPressed(81, false);
                    break;
                case Keys.J:
                    key.setKeyPressed(82, false);
                    break;
                case Keys.M:
                    key.setKeyPressed(83, false);
                    break;
                case Keys.Oemcomma:
                    key.setKeyPressed(84, false);
                    break;
            }
        }

        private void OscillatorBtn_Click(object sender, EventArgs e)
        {
            if (sine.Checked)
            {
                WaveInfo.waveType[0] = WaveInfo.Sine;
            }
            else if (saw.Checked)
            {
                WaveInfo.waveType[0] = WaveInfo.Saw;
            }
            else if (pulse.Checked)
            {
                WaveInfo.waveType[0] = WaveInfo.Pulse;
            }
            else if (triangle.Checked)
            {
                WaveInfo.waveType[0] = WaveInfo.Triangle;
            }
            else
            {
                WaveInfo.waveType[0] = WaveInfo.Noise;
            }

            if (sine2.Checked)
            {
                WaveInfo.waveType[1] = WaveInfo.Sine;
            }
            else if (saw2.Checked)
            {
                WaveInfo.waveType[1] = WaveInfo.Saw;
            }
            else if (pulse2.Checked)
            {
                WaveInfo.waveType[1] = WaveInfo.Pulse;
            }
            else if (triangle2.Checked)
            {
                WaveInfo.waveType[1] = WaveInfo.Triangle;
            }
            else
            {
                WaveInfo.waveType[1] = WaveInfo.Noise;
            }

            if (sine3.Checked)
            {
                WaveInfo.waveType[2] = WaveInfo.Sine;
            }
            else if (saw3.Checked)
            {
                WaveInfo.waveType[2] = WaveInfo.Saw;
            }
            else if (pulse3.Checked)
            {
                WaveInfo.waveType[2] = WaveInfo.Pulse;
            }
            else if (triangle3.Checked)
            {
                WaveInfo.waveType[2] = WaveInfo.Triangle;
            }
            else
            {
                WaveInfo.waveType[2] = WaveInfo.Noise;
            }
        }

        private MidiMessageInput midiInput;
        private bool midiDeviceDecided;

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            midiDeviceDecided = midiInput.setInputDeviceAndOpen(listBox1.SelectedIndex);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            midiInput.addItem(listBox1);
        }

        private void trackBar6_Scroll(object sender, EventArgs e)
        {
            WaveInfo.volume[0] = (float)Decibel.DecibelToValue((double)this.trackBar6.Value);
        }

        private void trackBar7_Scroll(object sender, EventArgs e)
        {
            WaveInfo.volume[1] = (float)Decibel.DecibelToValue((double)this.trackBar7.Value);
        }

        private void trackBar8_Scroll(object sender, EventArgs e)
        {
            WaveInfo.volume[2] = (float)Decibel.DecibelToValue((double)this.trackBar8.Value);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            WaveInfo.MasterVolume = (float)Decibel.DecibelToValue((double)this.trackBar1.Value);
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            WaveInfo.Attack = (float)((float)this.trackBar2.Value/5000 * sampleRate);
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            WaveInfo.Decay = (float)((float)this.trackBar3.Value / 5000 * sampleRate);
        }

        private void trackBar5_Scroll(object sender, EventArgs e)
        {
            WaveInfo.Release = (float)((float)this.trackBar5.Value / 5000 * sampleRate);
        }

        private void trackBar4_Scroll(object sender, EventArgs e)
        {
            WaveInfo.Sustain = (float)Decibel.DecibelToValue((double)this.trackBar4.Value);
        }

        private void trackBar9_Scroll(object sender, EventArgs e)
        {
            WaveInfo.detune[0] = (float)this.trackBar9.Value/10;
        }

        private void trackBar10_Scroll(object sender, EventArgs e)
        {
            WaveInfo.detune[1] = (float)this.trackBar10.Value / 10;
        }

        private void trackBar11_Scroll(object sender, EventArgs e)
        {
            WaveInfo.detune[2] = (float)this.trackBar11.Value / 10;
        }

        private void trackBar12_Scroll(object sender, EventArgs e)
        {
            WaveInfo.coarse[0] = this.trackBar12.Value;
        }

        private void trackBar13_Scroll(object sender, EventArgs e)
        {
            WaveInfo.coarse[1] = this.trackBar13.Value;
        }

        private void trackBar14_Scroll(object sender, EventArgs e)
        {
            WaveInfo.coarse[2] = this.trackBar14.Value;
        }

        //private void trackBar15_Scroll(object sender, EventArgs e)
        //{
        //    Delay.delayFlame = (int)(this.trackBar15.Value / 5000 * sampleRate);
        //}

        //private void trackBar16_Scroll(object sender, EventArgs e)
        //{
        //    Delay.mix = (float)Decibel.DecibelToValue((double)this.trackBar16.Value);
        //}

        //private void trackBar17_Scroll(object sender, EventArgs e)
        //{
        //    Delay.feedback = (float)this.trackBar17.Value / 100.0f;
        //}
    }
}
