using DKEngine.Core;

namespace MarIO.Assets.Scenes
{
    abstract class MapBase : Scene
    {
        public sealed override void Init()
        {
            Load();

            Shared.TimeCounter.Start();
        }

        public sealed override void Unload()
        {
            Shared.TimeCounter.Reset();
        }

        public abstract void Load();
    }
}
