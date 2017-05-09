using DKEngine.Core;

namespace MarIO.Assets.Scenes
{
    internal abstract class MapBase : Scene
    {
        public sealed override void Init()
        {
            Load();

            Shared.Mechanics.TimeCounter.Start();
        }

        public sealed override void Unload()
        {
            Shared.Mechanics.TimeCounter.Reset();
        }

        public abstract void Load();
    }
}