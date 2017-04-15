using DKEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DKEngine.Core.Components;
using System.Diagnostics;

namespace MarIO.Assets.Scripts
{
    class MusicScript : Script
    {
        Sound Music;
        SoundSource Output;
        TimeSpan MusicLenght;

        Stopwatch Timer;

        public MusicScript(GameObject Parent) : base(Parent)
        { }

        protected override void OnColliderEnter(Collider e)
        { }

        protected override void Start()
        {
            Music = new Sound("./Sounds/Overworld_Theme.mp3");
            Output = Component.Find<SoundSource>("MusicPlayer_SoundSource");

            MusicLenght = Music.FileReader.TotalTime;

            Output.PlaySound(Music);

            Timer = Stopwatch.StartNew();
        }

        protected override void Update()
        {
            if(Timer.Elapsed > MusicLenght)
            {
                Output.PlaySound(Music);

                Timer.Restart();
            }
        }
    }
}
