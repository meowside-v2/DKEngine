using DKEngine.Core;
using DKEngine.Core.Components;
using MarIO.Assets.Models;
using MarIO.Assets.Scripts;
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
            _1.Transform.Scale = new Vector3(2, 2, 2);
            _1.SizeInBlocks = new Vector3(50, 3, 0);
            _1.Transform.Position = new Vector3(0, 100, 0);
            _1.Name = "ground1";
            _1.InitCollider = true;

            Group _2 = new Group();
            _2.Type = Block.BlockType.Ground2;
            _2.Transform.Scale = new Vector3(2, 2, 2);
            _2.SizeInBlocks = new Vector3(10, 3, 0);
            _2.Transform.Position = new Vector3(60 * 16, 100, 0);
            _2.Name = "ground2";
            _2.InitCollider = true;

            Group _3 = new Group();
            _3.Type = Block.BlockType.Ground2;
            _3.Transform.Scale = new Vector3(2, 2, 2);
            _3.SizeInBlocks = new Vector3(50, 3, 0);
            _3.Transform.Position = new Vector3(60 * 16 + 20 * 16, 100, 0);
            _3.Name = "ground3";
            _3.InitCollider = true;

            Block pipe = new Block();
            pipe.Type = Block.BlockType.Pipe1;
            pipe.Transform.Scale = new Vector3(2, 2, 2);
            pipe.Transform.Position = new Vector3(240, 36, 0);

            Mario m = new Mario();
            m.Transform.Scale = new Vector3(2, 2, 2);
            m.InitNewScript<CameraController>();

            Camera c = new Camera();
            c.BackGround = System.Drawing.Color.FromArgb(0xFF, 0x20, 0xEE, 0xEE);
            
        }
    }
}
