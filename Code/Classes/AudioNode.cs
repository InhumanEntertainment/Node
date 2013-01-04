using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using Microsoft.Xna.Framework.Audio;

namespace Inhuman
{
    public class AudioNode : Node
    {
        private SoundEffect _sound;

        [System.Xml.Serialization.XmlIgnore]
        public SoundEffect Sound
        {
            get
            {
                return _sound;
            }
            set
            {
                if (value != _sound)
                {
                    _sound = value;
                    NotifyPropertyChanged("Sound");
                }
            }
        }

        string _Filename;
        public string Filename
        {
            get
            {
                return _Filename;
            }
            set
            {
                if (value != _Filename)
                {
                    _Filename = value;
                    NotifyPropertyChanged("Filename");
                }
            }
        }

        float _volume;
        public float Volume
        {
            get
            {
                return _volume;
            }
            set
            {
                if (value != _volume)
                {
                    _volume = value;
                    NotifyPropertyChanged("Volume");
                }
            }
        }

        //===================================================================================================================================================//
        public AudioNode()
        {            
        }

        //===================================================================================================================================================//
        public void Play()
        {            
            if (Sound != null)
	        {
                Sound.Play();
	        }
        }

        //===================================================================================================================================================//
        public void LoadSound()
        {
            if (Sound == null && Filename != "")
            {
                IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();

                if (storage.FileExists(Filename))
                {
                    // Open file //
                    IsolatedStorageFileStream stream = storage.OpenFile(Filename, FileMode.Open);

                    //Stream to Audio //
                    MemoryStream memStream = new MemoryStream();
                    byte[] buffer = new byte[256];

                    long fileSize = stream.Length;
                    long readSize = 44;
                    stream.Position = 44;
                    while (readSize < fileSize)
                    {
                        int readLength = stream.Read(buffer, 0, buffer.Length);
                        readSize += readLength;

                        memStream.Write(buffer, 0, buffer.Length);
                    }

                    // Create Sound //
                    Sound = new SoundEffect(memStream.ToArray(), Microphone.Default.SampleRate, AudioChannels.Mono);
                }
            }
        }

        //===================================================================================================================================================//
        byte[] AudioBuffer;
        MemoryStream AudioStream = new MemoryStream();

        //===================================================================================================================================================//
        public void Record()
        {
            Microphone mic = Microphone.Default;

            mic.BufferDuration = TimeSpan.FromMilliseconds(100);
            AudioBuffer = new byte[mic.GetSampleSizeInBytes(mic.BufferDuration)];
            AudioStream.SetLength(0);

            WriteWavHeader(AudioStream, mic.SampleRate);

            mic.BufferReady += new EventHandler<EventArgs>(mic_BufferReady);
            mic.Start();
        }

        //===================================================================================================================================================//
        public void StopRecording()
        {
            Microphone mic = Microphone.Default;

            mic.Stop();
            mic.BufferReady -= new EventHandler<EventArgs>(mic_BufferReady);
            UpdateWavHeader(AudioStream);

            byte[] SoundData = AudioStream.ToArray();
            Sound = new SoundEffect(SoundData, mic.SampleRate, AudioChannels.Mono);

            TimeSpan SoundTime = mic.GetSampleDuration(SoundData.Length);
            Info = "Duration: " + SoundTime.Minutes.ToString("0") + ":" + SoundTime.Seconds.ToString("00");

            // Save Audio // 
            Filename = Id + ".wav";
            IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
            IsolatedStorageFileStream stream = storage.CreateFile(Filename);

            byte[] buffer = new byte[256];

            long fileSize = SoundData.Length;
            long readSize = 0;
            AudioStream.Position = 0;
            while (readSize < fileSize)
            {                
                int readLength = AudioStream.Read(buffer, 0, buffer.Length);
                readSize += readLength;

                stream.Write(buffer, 0, buffer.Length);
            }

            stream.Close();
            stream.Dispose();

            Volume = 0;
        }

        //===================================================================================================================================================//
        void WriteWavHeader(Stream stream, int sampleRate)
        {
            const int bitsPerSample = 16;
            const int bytesPerSample = bitsPerSample / 8;
            var encoding = System.Text.Encoding.UTF8;

            // ChunkID Contains the letters "RIFF" in ASCII form (0x52494646 big-endian form).
            stream.Write(encoding.GetBytes("RIFF"), 0, 4);

            // NOTE this will be filled in later
            stream.Write(BitConverter.GetBytes(0), 0, 4);

            // Format Contains the letters "WAVE"(0x57415645 big-endian form).
            stream.Write(encoding.GetBytes("WAVE"), 0, 4);

            // Subchunk1ID Contains the letters "fmt " (0x666d7420 big-endian form).
            stream.Write(encoding.GetBytes("fmt "), 0, 4);

            // Subchunk1Size 16 for PCM.  This is the size of therest of the Subchunk which follows this number.
            stream.Write(BitConverter.GetBytes(16), 0, 4);

            // AudioFormat PCM = 1 (i.e. Linear quantization) Values other than 1 indicate some form of compression.
            stream.Write(BitConverter.GetBytes((short)1), 0, 2);

            // NumChannels Mono = 1, Stereo = 2, etc.
            stream.Write(BitConverter.GetBytes((short)1), 0, 2);

            // SampleRate 8000, 44100, etc.
            stream.Write(BitConverter.GetBytes(sampleRate), 0, 4);

            // ByteRate =  SampleRate * NumChannels * BitsPerSample/8
            stream.Write(BitConverter.GetBytes(sampleRate * bytesPerSample), 0, 4);

            // BlockAlign NumChannels * BitsPerSample/8 The number of bytes for one sample including all channels.
            stream.Write(BitConverter.GetBytes((short)(bytesPerSample)), 0, 2);

            // BitsPerSample    8 bits = 8, 16 bits = 16, etc.
            stream.Write(BitConverter.GetBytes((short)(bitsPerSample)), 0, 2);

            // Subchunk2ID Contains the letters "data" (0x64617461 big-endian form).
            stream.Write(encoding.GetBytes("data"), 0, 4);

            // NOTE to be filled in later
            stream.Write(BitConverter.GetBytes(0), 0, 4);
        }

        //===================================================================================================================================================//
        void UpdateWavHeader(Stream stream)
        {
            if (!stream.CanSeek) throw new Exception("Can't seek stream to update wav header");

            var oldPos = stream.Position;

            // ChunkSize  36 + SubChunk2Size
            stream.Seek(4, SeekOrigin.Begin);
            stream.Write(BitConverter.GetBytes((int)stream.Length - 8), 0, 4);

            // Subchunk2Size == NumSamples * NumChannels * BitsPerSample/8 This is the number of bytes in the data.
            stream.Seek(40, SeekOrigin.Begin);
            stream.Write(BitConverter.GetBytes((int)stream.Length - 44), 0, 4);

            stream.Seek(oldPos, SeekOrigin.Begin);
        }

        //===================================================================================================================================================//
        void mic_BufferReady(object sender, EventArgs e)
        {
            Microphone mic = Microphone.Default;

            mic.GetData(AudioBuffer);
            AudioStream.Write(AudioBuffer, 0, AudioBuffer.Length);

            TimeSpan SoundTime = mic.GetSampleDuration((int)AudioStream.Length);
            Info = "Recording: " + SoundTime.Minutes.ToString("0") + ":" + SoundTime.Seconds.ToString("00");


            // Calculate Volume //
            long totalSquare = 0;
            for (int i = 0; i < AudioBuffer.Length; i += 2)
            {
                short sample = (short)(AudioBuffer[i] | (AudioBuffer[i + 1] << 8));
                totalSquare += sample * sample;
            }

            long meanSquare = 2 * totalSquare / AudioBuffer.Length;
            double rms = Math.Sqrt(meanSquare); 
            double volume = rms / 32768.0;

            Volume = (float)Math.Pow(volume * 2, 0.5f);
        }
    }
}
