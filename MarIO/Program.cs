using DKEngine;
using DKEngine.Core;
using MarIO.Assets.Scenes;
using MarIO.Assets.Sprites;
using System.Globalization;

namespace MarIO
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Engine.Init();

            Engine.Sound.SoundVolume = 0.5f;

            Database.LoadResources(Sprites.ResourceManager.GetResourceSet(CultureInfo.CurrentCulture, true, true));
            Database.LoadResources(Enemies.ResourceManager.GetResourceSet(CultureInfo.CurrentCulture, true, true));

            Engine.LoadSceneToMemory<MainMenu>();
            Engine.LoadSceneToMemory<Level_1_1>();
            Engine.LoadSceneToMemory<GameOver>();
            Engine.LoadSceneToMemory<WorldScreen>();

            Engine.LoadScene<About>();

            //Engine.ChangeScene(nameof(MainMenu));
        }
    }
}