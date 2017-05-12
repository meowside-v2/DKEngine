using DKEngine.Core;
using DKEngine.Core.Components;

namespace MarIO.Assets.Models
{
    public class SoundOutput : GameObject
    {
        protected override void Initialize()
        {
            this.Name = nameof(SoundOutput);
            this.InitNewComponent<SoundSource>();
            Shared.Mechanics.FXPlayer = this;
        }
    }
}