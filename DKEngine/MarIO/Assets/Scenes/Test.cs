using DKEngine.Core;
using DKEngine.Core.Components;
using MarIO.Assets.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarIO.Assets.Scenes
{
    class Test : Scene
    {
        protected override void Init()
        {
            new TestObject();
            new ChangedObject();

            new Camera();

            /*foreach (var item in Enum.GetValues(typeof(Block.BlockType)))
            {
                Debug.WriteLine(item);
            }*/
        }
    }
}
