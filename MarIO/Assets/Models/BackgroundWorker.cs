using DKEngine.Core;
using MarIO.Assets.Scripts;

namespace MarIO.Assets.Models
{
    internal class BackgroundWorker : GameObject
    {
        protected override void Initialize()
        {
            this.InitNewScript<BlockAnimatorScript>();
            this.InitNewScript<FloatingCoinAnimatorScript>();
            this.InitNewScript<FloatingTextAnimatorScript>();
            this.InitNewScript<SpecialBlocksUpdateScript>();
            this.InitNewScript<WorldChangeManagerScript>();
        }
    }
}