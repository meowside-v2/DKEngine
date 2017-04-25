using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio;
using NAudio.Wave;
using System.IO;
using NAudio.Wave.SampleProviders;
using DKEngine.Core.Ext;
using System.Diagnostics;

namespace DKEngine.Core.Components
{
    public class SoundPlayer
    {
        private readonly IWavePlayer outputDevice;
        private readonly MixingSampleProvider mixer;
        private bool IsAvailable = true;

        internal SoundPlayer(int sampleRate = 44100, int channelCount = 2)
        {
            outputDevice = new WaveOutEvent();
            mixer = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, channelCount))
            {
                ReadFully = true
            };
            outputDevice.Init(mixer);
            outputDevice.Play();
        }

        public void PlaySound(string fileName)
        {
            if (Engine.Sound.IsSoundEnabled)
            {
                if (IsAvailable)
                {
                    try
                    {
                        var input = new AudioFileReader(fileName);
                        AddMixerInput(new AutoDisposeFileReader(input));
                    }
                    catch
                    {
                        IsAvailable = false;
                    }
                }
            }
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

        public void PlaySound(Sound sound)
        {
            if (IsAvailable)
            {
                try
                {
                    AddMixerInput(new CachedSoundSampleProvider(sound));
                }
                catch
                {
                    IsAvailable = false;
                }
            }
        }

        private void AddMixerInput(ISampleProvider input)
        {
            mixer.AddMixerInput(ConvertToRightChannelCount(input));
        }

        public void Dispose()
        {
            outputDevice.Dispose();
        }
    }

    public class SoundSource : Component
    {
        private bool IsAvailable = true;

        public SoundSource(GameObject Parent)
            :base(Parent)
        {
            this.Name = string.Format("{0}_SoundSource", Parent.Name);

            try
            {
                Engine.LoadingScene.AllComponents.AddSafe(this);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Loading scene is NULL\n\n{0}", e);
            }
        }

        public void PlaySound(Sound sound)
        {
            if (Engine.Sound.IsSoundEnabled)
            {
                if (IsAvailable)
                {
                    try
                    {
                        Engine.Sound.Instance.PlaySound(sound);
                    }
                    catch
                    {
                        IsAvailable = false;
                    }
                }
            }
        }

        public override void Destroy()
        {
            try
            {
                Engine.LoadingScene.AllComponents.Remove(this.Name);
            }
            catch
            { }

            this.Parent = null;
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
        private readonly Sound cachedSound;
        private long position;

        public CachedSoundSampleProvider(Sound cachedSound)
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

    public class Sound
    {
        public float[] AudioData { get; private set; }
        public WaveFormat WaveFormat { get; private set; }
        public AudioFileReader FileReader { get; private set; }

        public Sound(string audioFileName)
        {
            using (FileReader = new AudioFileReader(audioFileName))
            {
                FileReader.Volume = Engine.Sound.SoundVolume;
                // TODO: could add resampling in here if required
                WaveFormat = FileReader.WaveFormat;
                var wholeFile = new List<float>((int)(FileReader.Length / 4));
                var readBuffer = new float[FileReader.WaveFormat.SampleRate * FileReader.WaveFormat.Channels];
                int samplesRead;
                while ((samplesRead = FileReader.Read(readBuffer, 0, readBuffer.Length)) > 0)
                {
                    wholeFile.AddRange(readBuffer.Take(samplesRead));
                }
                AudioData = wholeFile.ToArray();
            }
        }
    }
}
