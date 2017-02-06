using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio;
using NAudio.Wave;
using System.IO;

namespace DKBasicEngine_1_0.Core.Components
{
    public sealed class SoundSource
    {
        public enum PlayBack
        {
            PlayAfterEnd,
            PlayNew
        }

        private int selection = -1;

        public WaveFileReader[] Files { get; private set; }
        public WaveOut OutputDevice { get; private set; }
        
        public PlaybackState State
        {
            get { return OutputDevice.PlaybackState; }
        }

        public PlayBack Type { get; set; }

        public SoundSource()
        {
            OutputDevice = new WaveOut();
        }

        public void Init(params WaveFileReader[] Source)
        {
            Files = new WaveFileReader[Source.Length];

            for (int i = 0; i < Source.Length; i++)
            {
                Files[i] = Source[i];
            }

            if (Source.Length == 1)
                OutputDevice.Init(Files[0]);
        }

        public void Init(params Mp3FileReader[] Source)
        {
            Files = new WaveFileReader[Source.Length];

            for(int i = 0; i < Source.Length; i++)
            {
                Files[i] = new WaveFileReader(Source[i]);
            }

            if (Source.Length == 1)
                OutputDevice.Init(Files[0]);
        }

        public void Play()
        {
            if(Type == PlayBack.PlayAfterEnd)
            {
                if(OutputDevice.PlaybackState == PlaybackState.Stopped)
                {
                    OutputDevice.Play();
                }
            }
            else if (Type == PlayBack.PlayNew)
            {
                OutputDevice.Stop();
                OutputDevice.Init(new WaveFileReader(Files[0]));
                OutputDevice.Play();
            }
        }

        public void Play(int Selection)
        {
            if (Type == PlayBack.PlayAfterEnd)
            {
                if (OutputDevice.PlaybackState == PlaybackState.Stopped)
                {
                    if (Selection != selection)
                    {
                        OutputDevice.Init(Files[selection]);
                    }

                    OutputDevice.Play();
                }
            }
            else if (Type == PlayBack.PlayNew)
            {
                OutputDevice.Stop();

                if (Selection != selection)
                {
                    Selection = selection;
                }

                OutputDevice.Init(new WaveFileReader(Files[selection]));
                OutputDevice.Play();
            }
        }

        public void Play(WaveFileReader Source)
        {
            if (Type == PlayBack.PlayAfterEnd)
            {
                if (OutputDevice.PlaybackState == PlaybackState.Stopped)
                {
                    OutputDevice.Init(Source);
                    OutputDevice.Play();
                }
            }
            else if (Type == PlayBack.PlayNew)
            {
                OutputDevice.Stop();
                OutputDevice.Init(Source);
                OutputDevice.Play();
            }
        }

        public void Stop()
        {
            OutputDevice.Stop();
        }
    }
}
