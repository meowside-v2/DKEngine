using DKEngine;
using DKEngine.Core;
using MarIO.Assets.Scenes;
using System.Globalization;

namespace MarIO
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Engine.Init();
            Database.LoadResources(Sprites.Sprites.ResourceManager.GetResourceSet(CultureInfo.CurrentCulture, true, true));
            Database.LoadResources(Sprites.Enemies.ResourceManager.GetResourceSet(CultureInfo.CurrentCulture, true, true));

            Engine.LoadSceneToMemory<MainMenu>();
            Engine.ChangeScene("MainMenu");

            /*Engine.LoadSceneToMemory<Test>();
            Engine.ChangeScene("test");*/
        }
    }
}