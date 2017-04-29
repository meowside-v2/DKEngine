using DKEngine.Core;
using DKEngine.Core.Components;
using MarIO.Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarIO.Assets.Models
{
    class MusicPlayer : GameObject
    {
        protected override void Initialize()
        {
            this.Name = "MusicPlayer";
            this.InitNewComponent<SoundSource>();
            this.InitNewScript<MusicScript>();
        }
    }
}
