using DKEngine.Core;
using System.Collections.Generic;

namespace MarIO.Assets.Scenes
{
    public abstract class MapBase : Scene
    {
        public static Dictionary<string, string> LevelsNames = new Dictionary<string, string>()
        {
            { nameof(Test), "test" },
            { nameof(Level_1_1), "1-1" }
        };

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