using DKEngine.Core;
using DKEngine.Core.Components;
using MarIO.Assets.Scripts;

namespace MarIO.Assets.Models
{
    internal class MusicPlayer : GameObject
    {
        protected override void Initialize()
        {
            this.Name = "MusicPlayer";
            //this.InitNewComponent<SoundSource>();
            this.InitNewScript<MusicScript>();
        }
    }
}