using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Midi;

namespace BitSynth
{
    class MidiMessageInput
    {
        InputDevice inputDevice;
        private bool[] pitchesPressed;

        public MidiMessageInput()
        {
            pitchesPressed = new bool[128];
            for(int i = 0; i < 128; i++)
            {
                pitchesPressed[i] = false;
            }
        }
        ~MidiMessageInput()
        {
            Close();
        }

        public void addItem(System.Windows.Forms.ListBox listBox)
        {
            // DeviceをlistBoxに追加
            int deviceNum = InputDevice.InstalledDevices.Count;
            listBox.Items.Clear();
            for (int i = 0; i < deviceNum; i++)
            {
                listBox.Items.Add(InputDevice.InstalledDevices[i].Name);
            }
        }

        public bool setInputDeviceAndOpen(int deviceId)
        {
            if (inputDevice != null) Close();

            try
            {

                if (deviceId >= 0 && deviceId < InputDevice.InstalledDevices.Count)
                {
                    // DeviceのOpen処理
                    inputDevice = InputDevice.InstalledDevices[deviceId];
                    inputDevice.Open();
                    inputDevice.StartReceiving(null);

                    // Summarizerに任せる
                    Summarizer summarizer = new Summarizer(inputDevice, pitchesPressed);
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (DeviceException e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public void Close()
        {
            try {
                // DeviceのClose処理
                inputDevice.StopReceiving();
                inputDevice.Close();
                inputDevice.RemoveAllEventHandlers();
            }
            catch(NullReferenceException e)
            {
                Console.WriteLine(e);
            }catch(DeviceException e)
            {
                Console.WriteLine(e);
            }
        }

        public bool IsNoDevice()
        {
            if (inputDevice == null) return true;
            else return false;
        }


        public class Summarizer
        {
            private InputDevice inputDevice;
            bool[] pitchesPressed;
            KeyPressed key;

            public Summarizer(InputDevice inputDevice, bool[] pitchesPressed)
            {
                this.inputDevice = inputDevice;
                this.pitchesPressed = pitchesPressed;
                key = new KeyPressed();

                // EventHandlerにNoteOnとNoteOffを追加
                inputDevice.NoteOn += new InputDevice.NoteOnHandler(this.NoteOn);
                inputDevice.NoteOff += new InputDevice.NoteOffHandler(this.NoteOff);
            }


            public void NoteOn(NoteOnMessage msg)
            {
                lock (this)
                {
                    pitchesPressed[(int)msg.Pitch] = true;
                    key.setKeyPressed((int)msg.Pitch,true);
                    //key.setKeyPressed(pitchesPressed);
                }
            }

            public void NoteOff(NoteOffMessage msg)
            {
                lock (this)
                {
                    pitchesPressed[(int)msg.Pitch] = false;
                    key.setKeyPressed((int)msg.Pitch, false);
                    ///pitchesPressed.Remove(msg.Pitch);
                    //key.setKeyPressed(pitchesPressed);
                    //pitchesPressed[msg.Pitch] = false;
                }
            }
        }
    }
}
