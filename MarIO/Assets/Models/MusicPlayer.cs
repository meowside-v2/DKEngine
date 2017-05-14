using DKEngine.Core;
using MarIO.Assets.Scripts;

namespace MarIO.Assets.Models
{
    public class MusicPlayer : GameObject
    {
        protected override void Initialize()
        {
            this.Name = "MusicPlayer";
            this.InitNewScript<MusicScript>();
        }
    }
}