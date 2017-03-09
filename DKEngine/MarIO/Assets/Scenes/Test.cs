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
            Group _1 = new Group();
            _1.Type = Block.BlockType.Ground2;
            _1.SizeInBlocks = new Vector3(50, 3, 0);
            _1.Transform.Position = new Vector3(0, 100, 0);
            _1.Name = "ground1";
            _1.InitCollider = true;

            Group _2 = new Group();
            _2.Type = Block.BlockType.Ground2;
            _2.SizeInBlocks = new Vector3(10, 3, 0);
            _2.Transform.Position = new Vector3(60 * 16, 100, 0);
            _2.Name = "ground2";
            _2.InitCollider = true;

            Group _3 = new Group();
            _3.Type = Block.BlockType.Ground2;
            _3.SizeInBlocks = new Vector3(50, 3, 0);
            _3.Transform.Position = new Vector3(60 * 16 + 20 * 16, 100, 0);
            _3.Name = "ground3";
            _3.InitCollider = true;

            Mario m = new Mario();
            Camera c = new Camera();
            c.Parent = m;
            c.Position += new Vector3(-100, -120, 0);
            c.BackGround = System.Drawing.Color.FromArgb(0xFF, 0x20, 0xEE, 0xEE);
            
        }
    }
}
