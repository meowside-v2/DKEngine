using DKEngine.Core;
using DKEngine.Core.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarIO.Assets.Scripts
{
    class SoundOutput : GameObject
    {
        protected override void Initialize()
        {
            this.Name = nameof(SoundOutput);
            this.InitNewComponent<SoundSource>();
            Shared.Mechanics.FXPlayer = this;
        }
    }
}
