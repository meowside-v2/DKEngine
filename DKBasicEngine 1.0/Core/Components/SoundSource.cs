using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio;
using NAudio.Wave;
using System.IO;
using NAudio.Wave.SampleProviders;

namespace DKEngine.Core.Components
{
    public class SoundSource : Component
    {
        /*public enum PlayBack
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
                    OutputDevice.Dispose();
                    OutputDevice = new WaveOut();
                    
                    OutputDevice.Play();
                }
            }
            else if (Type == PlayBack.PlayNew)
            {
                OutputDevice.Dispose();
                OutputDevice = new WaveOut();

                OutputDevice.Init(GetNewInstanceOfSoundFile(Files[0]));
                OutputDevice.Play();
            }
            else if(Type == PlayBack.StopOldStartNew)
            {
                //OutputDevice.Stop();

                OutputDevice.Dispose();
                OutputDevice = new WaveOut();

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

                    OutputDevice.Dispose();
                    OutputDevice = new WaveOut();

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

                OutputDevice.Dispose();
                OutputDevice = new WaveOut();

                OutputDevice.Init(GetNewInstanceOfSoundFile(Files[selection]));
                OutputDevice.Play();
            }
            else if (Type == PlayBack.StopOldStartNew)
            {
                //OutputDevice.Stop();

                if (Selection != selection)
                {
                    Selection = selection;
                }

                OutputDevice.Dispose();
                OutputDevice = new WaveOut();

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
                    OutputDevice.Dispose();
                    OutputDevice = new WaveOut();

                    OutputDevice.Init(Source);
                    OutputDevice.Play();
                }
            }
            else if (Type == PlayBack.PlayNew)
            {
                OutputDevice.Dispose();
                OutputDevice = new WaveOut();
                OutputDevice.Init(GetNewInstanceOfSoundFile(Source));
                OutputDevice.Play();
            }
            else if (Type == PlayBack.StopOldStartNew)
            {
                //OutputDevice.Stop();

                OutputDevice.Dispose();
                OutputDevice = new WaveOut();

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
        }*/

        private readonly IWavePlayer outputDevice;
        private readonly MixingSampleProvider mixer;

        public SoundSource(GameObject Parent, int sampleRate = 44100, int channelCount = 2)
            :base(Parent)
        {
            outputDevice = new WaveOutEvent();
            mixer = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, channelCount));
            mixer.ReadFully = true;
            outputDevice.Init(mixer);
            outputDevice.Play();
        }

        public void PlaySound(string fileName)
        {
            var input = new AudioFileReader(fileName);
            AddMixerInput(new AutoDisposeFileReader(input));
        }

        private ISampleProvider ConvertToRightChannelCount(ISampleProvider input)
        {
            if (input.WaveFormat.Channels == mixer.WaveFormat.Channels)
            {
                return input;
            }
            if (input.WaveFormat.Channels == 1 && mixer.WaveFormat.Channels == 2)
            {
                return new MonoToStereoSampleProvider(input);
            }
            throw new NotImplementedException("Not yet implemented this channel count conversion");
        }

        public void PlaySound(CachedSound sound)
        {
            AddMixerInput(new CachedSoundSampleProvider(sound));
        }

        private void AddMixerInput(ISampleProvider input)
        {
            mixer.AddMixerInput(ConvertToRightChannelCount(input));
        }

        public void Dispose()
        {
            outputDevice.Dispose();
        }

        protected internal override void Destroy()
        {
            throw new NotImplementedException();
        }
    }

    class AutoDisposeFileReader : ISampleProvider
    {
        private readonly AudioFileReader reader;
        private bool isDisposed;
        public AutoDisposeFileReader(AudioFileReader reader)
        {
            this.reader = reader;
            this.WaveFormat = reader.WaveFormat;
        }

        public int Read(float[] buffer, int offset, int count)
        {
            if (isDisposed)
                return 0;
            int read = reader.Read(buffer, offset, count);
            if (read == 0)
            {
                reader.Dispose();
                isDisposed = true;
            }
            return read;
        }

        public WaveFormat WaveFormat { get; private set; }
    }

    class CachedSoundSampleProvider : ISampleProvider
    {
        private readonly CachedSound cachedSound;
        private long position;

        public CachedSoundSampleProvider(CachedSound cachedSound)
        {
            this.cachedSound = cachedSound;
        }

        public int Read(float[] buffer, int offset, int count)
        {
            var availableSamples = cachedSound.AudioData.Length - position;
            var samplesToCopy = Math.Min(availableSamples, count);
            Array.Copy(cachedSound.AudioData, position, buffer, offset, samplesToCopy);
            position += samplesToCopy;
            return (int)samplesToCopy;
        }

        public WaveFormat WaveFormat { get { return cachedSound.WaveFormat; } }
    }

    public class CachedSound
    {
        public float[] AudioData { get; private set; }
        public WaveFormat WaveFormat { get; private set; }
        public CachedSound(string audioFileName)
        {
            using (var audioFileReader = new AudioFileReader(audioFileName))
            {
                // TODO: could add resampling in here if required
                WaveFormat = audioFileReader.WaveFormat;
                var wholeFile = new List<float>((int)(audioFileReader.Length / 4));
                var readBuffer = new float[audioFileReader.WaveFormat.SampleRate * audioFileReader.WaveFormat.Channels];
                int samplesRead;
                while ((samplesRead = audioFileReader.Read(readBuffer, 0, readBuffer.Length)) > 0)
                {
                    wholeFile.AddRange(readBuffer.Take(samplesRead));
                }
                AudioData = wholeFile.ToArray();
            }
        }
    }
}
