using DKEngine;
using DKEngine.Core;
using DKEngine.Core.Components;
using System;
using System.Diagnostics;

namespace MarIO.Assets.Scripts
{
    internal class MusicScript : Script
    {
        private Sound Music;
        private SoundSource Output;
        private TimeSpan MusicLenght;

        private Stopwatch Timer;

        public MusicScript(GameObject Parent) : base(Parent)
        { }

        protected override void OnColliderEnter(Collider e)
        { }

        protected override void Start()
        {
            Music = new Sound(Shared.Assets.Sounds.OVERWORLD_THEME);
            Output = Component.Find<SoundSource>("MusicPlayer_SoundSource");
            
            MusicLenght = Music.FileReader.TotalTime;
            Output.PlaySound(Music);
            
            Timer = Stopwatch.StartNew();
        }

        protected override void Update()
        {
            if (Timer.Elapsed > MusicLenght)
            {
                Output.PlaySound(Music);

                Timer.Restart();
            }
        }
    }
}