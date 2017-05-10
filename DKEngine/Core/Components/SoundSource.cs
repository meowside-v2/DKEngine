using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DKEngine.Core.Components
{
    internal class SoundPlayer
    {
        private readonly DirectSoundOut outputDevice;
        private readonly MixingSampleProvider mixer;
        private bool IsAvailable = true;

        internal SoundPlayer(int sampleRate = 44100, int channelCount = 2)
        {
            outputDevice = new DirectSoundOut(40);
            mixer = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, channelCount))
            {
                ReadFully = true
            };
            //outputDevice.DesiredLatency = 50;
            outputDevice.Init(mixer);
            outputDevice.Play();
        }
        

        private ISampleProvider ConvertToRightChannelCount(CachedSoundSampleProvider input)
        {
            if (input.WaveFormat.Channels == mixer.WaveFormat.Channels)
            {
                input.cachedSound._MonoToStereoSampleProvider = input;
                return input.cachedSound._MonoToStereoSampleProvider;
            }
            if (input.WaveFormat.Channels == 1 && mixer.WaveFormat.Channels == 2)
            {
                input.cachedSound._MonoToStereoSampleProvider = new MonoToStereoSampleProvider(input);
                return input.cachedSound._MonoToStereoSampleProvider;
            }
            throw new NotImplementedException("Not yet implemented this channel count conversion");
        }

        public void PlaySound(Sound sound)
        {
            if (IsAvailable)
            {
                try
                {
                    AddMixerInput(ConvertToRightChannelCount(new CachedSoundSampleProvider(sound)));
                }
                catch
                {
                    IsAvailable = false;
                }
            }
        }

        public void StopSound(Sound sound)
        {
            if (IsAvailable)
            {
                try
                {
                    RemoveMixerInput(sound._MonoToStereoSampleProvider);
                    
                }
                catch
                { }
            }
        }

        private void AddMixerInput(ISampleProvider input)
        {
            mixer.AddMixerInput(input);
        }

        private void RemoveMixerInput(ISampleProvider input)
        {
            mixer.RemoveMixerInput(input);
        }

        public void Dispose()
        {
            outputDevice.Dispose();
        }
    }

    /// <summary>
    /// SoundSource component used for sound effects
    /// </summary>
    /// <seealso cref="DKEngine.Core.Components.Component" />
    public class SoundSource : Component
    {
        private bool IsAvailable = true;

        public SoundSource(GameObject Parent)
            : base(Parent)
        {
            this.Name = string.Format("{0}_{1}", Parent.Name, nameof(SoundSource));
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

        public void StopSound(Sound sound)
        {
            if (Engine.Sound.IsSoundEnabled)
            {
                if (IsAvailable)
                {
                    try
                    {
                        Engine.Sound.Instance.StopSound(sound);
                    }
                    catch
                    { }
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

    internal class CachedSoundSampleProvider : ISampleProvider
    {
        public Sound cachedSound;
        private long position;

        public CachedSoundSampleProvider(Sound cachedSound)
        {
            this.cachedSound = cachedSound;
            this.cachedSound._CachedSoundSampleProvider = this;
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

    /// <summary>
    /// Class holding specified audio file
    /// </summary>
    public class Sound
    {
        public float[] AudioData { get; private set; }
        public WaveFormat WaveFormat { get; private set; }
        public AudioFileReader FileReader { get; private set; }
        internal CachedSoundSampleProvider _CachedSoundSampleProvider { get; set; }
        internal ISampleProvider _MonoToStereoSampleProvider { get; set; }

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