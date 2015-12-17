using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using SharpDX;
using SharpDX.XAPO;


namespace BitSynth
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ModulatorParam
    {
    }

    /// <summary>
    /// A simple Ring Modulator Effect
    /// </summary>
    public class ModulatorEffect : AudioProcessorBase<ModulatorParam>
    {
        private Stopwatch timer;

        private double m_SampleRate;

        private SynthMain synth;
        private WaveInfo waveinfo;

        //MidiMessageInput midi;


        public ModulatorEffect()
        {
            RegistrationProperties = new RegistrationProperties()
            {
                Clsid = Utilities.GetGuidFromType(typeof(ModulatorEffect)),
                CopyrightInfo = "Copyright",
                FriendlyName = "Modulator",
                MaxInputBufferCount = 1,
                MaxOutputBufferCount = 1,
                MinInputBufferCount = 1,
                MinOutputBufferCount = 1,
                Flags = PropertyFlags.Default
            };

            m_SampleRate = 44100.0;
            synth = new SynthMain();
            waveinfo = new WaveInfo();
            //midi = new MidiMessageInput(f);

            timer = new Stopwatch();
            timer.Start();
        }

        public void setSampleRate(double sampleRate)
        {
            this.m_SampleRate = sampleRate;
        }

        private int _counter;
        public override void Process(BufferParameters[] inputProcessParameters, BufferParameters[] outputProcessParameters, bool isEnabled)
        {
            int frameCount = inputProcessParameters[0].ValidFrameCount;
            DataStream input = new DataStream(inputProcessParameters[0].Buffer, frameCount * InputFormatLocked.BlockAlign, true, true);
            DataStream output = new DataStream(inputProcessParameters[0].Buffer, frameCount * InputFormatLocked.BlockAlign, true, true);

            //Console.WriteLine("Process is called every: " + timer.ElapsedMilliseconds);
            timer.Reset(); timer.Start();

            // Use a linear ramp on intensity in order to avoir too much glitches
            float nextIntensity = m_Intensity;
            for (int i = 0; i < frameCount; i++, _counter++)
            {
                float left = input.Read<float>();
                float right = input.Read<float>();


                waveinfo.sampleRate = m_SampleRate;
                synth.synthProcess(ref left,ref right);


                output.Write(left); // Left
                output.Write(right); // Right
            }
            lastIntensity = nextIntensity;
        }


        private float lastIntensity = 0;

        public float m_Intensity { get; set; }
    }
} 