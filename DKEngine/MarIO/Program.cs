using DKEngine;
using DKEngine.Core;
using MarIO.Assets.Scenes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarIO
{
    class Program
    {
        static void Main(string[] args)
        {
            Engine.Init();
            Database.LoadResources(Sprites.Sprites.ResourceManager.GetResourceSet(CultureInfo.CurrentCulture, true, true));
            Database.LoadResources(Sprites.Enemies.ResourceManager.GetResourceSet(CultureInfo.CurrentCulture, true, true));

            Engine.ChangeScene<Test>();
        }
    }
}
