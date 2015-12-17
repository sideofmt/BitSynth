using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitSynth
{
    class Oscillator
    {
        private double m_Sig;
        private int m_WaveType;
        private int m_PulseWidth;
        private float m_Vol;
        private WaveInfo waveinfo;

        private int lastWaveType;

        SineWave sin;
        SawtoothWave saw;
        TriangleWave triangle;
        PulseWave pulse;
        Noise noise;

        public Oscillator()
        {
            sin = new SineWave();
            saw = new SawtoothWave();
            triangle = new TriangleWave();
            pulse = new PulseWave();
            noise = new Noise();

            m_Sig = 0.0;
            m_WaveType = 0;
            m_PulseWidth = 50;
            waveinfo = new WaveInfo();

            lastWaveType = 0;
        }


        public void setWaveType(int waveType)
        {
            this.m_WaveType = waveType;
        }

        public void setPulseWidth(int pulseWidth)
        {
            this.m_PulseWidth = pulseWidth;
        }

        public void setVol(float vol)
        {
            this.m_Vol = vol;
        }
        public float getVol()
        {
            return m_Vol;
        }
        public void setWaveinfo(WaveInfo waveinfo)
        {
            this.waveinfo = waveinfo;
        }

        public double oscillatorProcess(double frequency)
        {
            if (frequency <= 0) return 0;

            switch (m_WaveType)
            {
                case 0:
                    if (lastWaveType != m_WaveType)sin.initializer() ;
                    m_Sig = sin.process(frequency, waveinfo.sampleRate);
                    break;
                case 1:
                    if (lastWaveType != m_WaveType) saw.initializer();
                    m_Sig = saw.process(frequency, waveinfo.sampleRate);
                    break;
                case 2:
                    if (lastWaveType != m_WaveType) pulse.initializer();
                    pulse.setPulseWidth(m_PulseWidth);
                    m_Sig = pulse.process(frequency, waveinfo.sampleRate);
                    break;
                case 3:
                    if (lastWaveType != m_WaveType) triangle.initializer();
                    m_Sig = triangle.process(frequency, waveinfo.sampleRate);
                    break;
                case 4:
                    m_Sig = noise.process(frequency, waveinfo.sampleRate);
                    break;
                default:
                    if (lastWaveType != m_WaveType) sin.initializer();
                    m_Sig = sin.process(frequency, waveinfo.sampleRate);
                    break;
            }
            lastWaveType = m_WaveType;
            return m_Sig;
        }

    }


    class SineWave
    {
        private double m_RadianIncreaseValue;
        private double m_SineWaveRadian;
        private static readonly double m_Coef = Math.PI * 2;

        public SineWave()
        {
            m_RadianIncreaseValue = 0.0;
            m_SineWaveRadian = 0.0;
        }

        public void initializer()
        {
            m_RadianIncreaseValue = 0.0;
            m_SineWaveRadian = 0.0;
        }

        public double process(double frequency, double sampleRate)
        {
            m_RadianIncreaseValue = m_Coef / (sampleRate / frequency);
            m_SineWaveRadian += m_RadianIncreaseValue;
            return Math.Sin(m_SineWaveRadian);
        }
    }

    class SawtoothWave
    {
        private int m_SawWaveSig;
        private int m_IncreaseValue;
        private static readonly uint m_IntSize = (uint)(Math.Pow(2,sizeof(int)*8)-1);
        private static readonly uint m_HalfIntSize = (uint)(Math.Pow(2, sizeof(int) * 8) - 1)/2;

        public SawtoothWave()
        {
            m_SawWaveSig = 0;
            m_IncreaseValue = 0;
        }

        public void initializer()
        {
            m_SawWaveSig = 0;
            m_IncreaseValue = 0;
        }

        public double process(double frequency, double sampleRate)
        {
            m_IncreaseValue = (int)(m_IntSize / (sampleRate / frequency));
            m_SawWaveSig += m_IncreaseValue;
            return (double)m_SawWaveSig / (double)m_HalfIntSize;
        }
    }

    class PulseWave
    {
        private float m_PulseWidth;
        private int m_counter;
        private bool m_IsPluse;

        public PulseWave()
        {
            m_PulseWidth = 0.5f;
            m_counter = 0;
            m_IsPluse = true;
        }

        public void initializer()
        {
            m_PulseWidth = 0.5f;
            m_counter = 0;
            m_IsPluse = true;
        }

        public void setPulseWidth(int pulseWidth)
        {
            if (pulseWidth > 100) pulseWidth = 100;
            if (pulseWidth < 0) pulseWidth = 0;

            this.m_PulseWidth = (float)pulseWidth/100;
        }

        public double process(double frequency, double sampleRate)
        {
            int onePeriod = (int)(sampleRate / frequency);
            int changePeriod = (int)(onePeriod * m_PulseWidth);

            if (m_counter > onePeriod) m_counter = 0;

            if (m_counter > changePeriod) m_IsPluse = false;
            else m_IsPluse = true;

            m_counter++;

            if (m_IsPluse)
            {
                return 1.0;
            }
            else
            {
                return -1.0;
            }
        }
    }

    class TriangleWave
    {
        private long m_TriangleWaveSig;
        private int m_IncreaseValue;
        private bool m_IsIncrease;

        private static readonly uint m_IntSize = (uint)(Math.Pow(2, sizeof(int) * 8) - 1);
        private static readonly uint m_HalfIntSize = (uint)(Math.Pow(2, sizeof(int) * 8) - 1) / 2;

        public TriangleWave()
        {
            m_TriangleWaveSig = 0;
            m_IncreaseValue = 0;
            m_IsIncrease = true;
        }

        public void initializer()
        {
            m_TriangleWaveSig = 0;
            m_IncreaseValue = 0;
            m_IsIncrease = true;
        }

        public double process(double frequency, double sampleRate)
        {
            m_IncreaseValue = (int)(m_IntSize / (sampleRate/2 / frequency));
            if (!m_IsIncrease) m_IncreaseValue = -m_IncreaseValue;

            m_TriangleWaveSig += m_IncreaseValue;

            if (m_TriangleWaveSig >= m_HalfIntSize)
            {
                m_IsIncrease = false;
                m_TriangleWaveSig = m_HalfIntSize;
            }
            if (m_TriangleWaveSig <= -m_HalfIntSize)
            {
                m_IsIncrease = true;
                m_TriangleWaveSig = -m_HalfIntSize;
            }
            return (double)m_TriangleWaveSig / (double)m_HalfIntSize;
        }
    }

    class Noise
    {
        private static readonly Random rand = new Random();
        private static readonly int m_IntSize = (int)(Math.Pow(2, sizeof(int) * 4) - 1);

        public Noise()
        {
        }

        public double process(double frequency, double sampleRate)
        {
            return (double)rand.Next(0, m_IntSize) / (double)m_IntSize;
        }
    }
}
