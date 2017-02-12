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
    public class SoundSource : Component
    {
        public enum PlayBack
        {
            PlayAfterEnd,
            PlayNew,
            StopOldStartNew
        }

        private int selection = -1;

        public WaveStream[] Files { get; private set; }
        public WaveOut OutputDevice { get; private set; }
        
        public PlaybackState State
        {
            get { return OutputDevice.PlaybackState; }
        }

        public PlayBack Type { get; set; }

        //private WaveStream _Last;

        public SoundSource(GameObject Parent)
            :base(Parent)
        {
            OutputDevice = new WaveOut();
        }

        public void Init(params WaveFileReader[] Source)
        {
            int lenght = Source.Length;
            Files = new WaveStream[lenght];
            for (int i = 0; i < lenght; i++)
                Files[i] = new WaveFileReader(Source[i]);

            if (Source.Length == 1)
                OutputDevice.Init(Files[0]);
        }

        public void Init(params Mp3FileReader[] Source)
        {
            int lenght = Source.Length;
            Files = new WaveStream[lenght];
            for (int i = 0; i < lenght; i++)
                Files[i] = new Mp3FileReader(Source[i]);

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
                OutputDevice.Init(GetNewInstanceOfSoundFile(Files[0]));
                OutputDevice.Play();
            }
            else if(Type == PlayBack.StopOldStartNew)
            {
                OutputDevice.Stop();
                OutputDevice.Init(Files[0]);
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
                        Selection = selection;
                    }

                    OutputDevice.Init(Files[selection]);
                    OutputDevice.Play();
                }
            }
            else if (Type == PlayBack.PlayNew)
            {
                if (Selection != selection)
                {
                    Selection = selection;
                }

                OutputDevice.Init(GetNewInstanceOfSoundFile(Files[selection]));
                OutputDevice.Play();
            }
            else if (Type == PlayBack.StopOldStartNew)
            {
                OutputDevice.Stop();

                if (Selection != selection)
                {
                    Selection = selection;
                }

                OutputDevice.Init(Files[selection]);
                OutputDevice.Play();
            }
        }

        public void Play(WaveStream Source)
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
                OutputDevice.Init(GetNewInstanceOfSoundFile(Source));
                OutputDevice.Play();
            }
            else if (Type == PlayBack.StopOldStartNew)
            {
                OutputDevice.Stop();
                OutputDevice.Init(Source);
                OutputDevice.Play();
            }
        }

        public void Play(UnmanagedMemoryStream Source)
        {
            if (Type == PlayBack.PlayAfterEnd)
            {
                if (OutputDevice.PlaybackState == PlaybackState.Stopped)
                {
                    OutputDevice.Init(GetNewInstanceOfSoundFile(Source));
                    OutputDevice.Play();
                }
            }
            else if (Type == PlayBack.PlayNew)
            {
                OutputDevice.Init(GetNewInstanceOfSoundFile(Source));
                OutputDevice.Play();
            }
            else if (Type == PlayBack.StopOldStartNew)
            {
                OutputDevice.Stop();
                OutputDevice.Init(GetNewInstanceOfSoundFile(Source));
                OutputDevice.Play();
            }
        }

        public void Play(byte[] Source)
        {
            if (Type == PlayBack.PlayAfterEnd)
            {
                if (OutputDevice.PlaybackState == PlaybackState.Stopped)
                {
                    OutputDevice.Init(GetNewInstanceOfSoundFile(Source));
                    OutputDevice.Play();
                }
            }
            else if (Type == PlayBack.PlayNew)
            {
                OutputDevice.Init(GetNewInstanceOfSoundFile(Source));
                OutputDevice.Play();
            }
            else if (Type == PlayBack.StopOldStartNew)
            {
                OutputDevice.Stop();
                OutputDevice.Init(GetNewInstanceOfSoundFile(Source));
                OutputDevice.Play();
            }
        }

        public void Stop()
        {
            OutputDevice.Stop();
        }

        private WaveStream GetNewInstanceOfSoundFile(WaveStream Source)
        {
            if (Source is WaveFileReader)
                return new WaveFileReader(Source);

            else if (Source is Mp3FileReader)
                return new Mp3FileReader(Source);

            else
                return null;
        }

        private WaveStream GetNewInstanceOfSoundFile(byte[] Source)
        {
            return new Mp3FileReader(new MemoryStream(Source));
        }

        private WaveStream GetNewInstanceOfSoundFile(UnmanagedMemoryStream Source)
        {
            return new WaveFileReader(Source);
        }

        protected internal override void Destroy()
        {
            throw new NotImplementedException();
        }
    }
}
