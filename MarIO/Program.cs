using DKEngine;
using DKEngine.Core;
using MarIO.Assets.Scenes;
using MarIO.Assets.Sprites;
using System.Globalization;

namespace MarIO
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Engine.Init();

            Engine.Sound.SoundVolume = 0.5f;

            Database.LoadResources(Sprites.ResourceManager.GetResourceSet(CultureInfo.CurrentCulture, true, true));
            Database.LoadResources(Enemies.ResourceManager.GetResourceSet(CultureInfo.CurrentCulture, true, true));

            Engine.LoadSceneToMemory<MainMenu>();
            Engine.LoadSceneToMemory<Test>();
            Engine.ChangeScene("MainMenu");

            /*Engine.LoadSceneToMemory<Test>();
            Engine.ChangeScene("test");*/
        }
    }
}