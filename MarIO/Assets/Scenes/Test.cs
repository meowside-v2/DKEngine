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
    class Test : MapBase
    {
        public Test()
        {
            Name = "test";
        }

        public override void Load()
        {
            new GUIUpdater();

            Group _1 = new Group()
            {
                Name = "ground1",
                InitCollider = true,
                Type = Block.BlockType.Ground2
            };
            _1.SizeInBlocks = new Vector3(50, 3, 0);
            _1.Transform.Position = new Vector3(0, -40, 0);


            Group _2 = new Group()
            {
                Name = "ground2",
                InitCollider = true,
                Type = Block.BlockType.Ground2
            };
            _2.SizeInBlocks = new Vector3(10, 3, 0);
            _2.Transform.Position = new Vector3(60 * 16, -40, 0);


            Group _3 = new Group()
            {
                Name = "ground3",
                Type = Block.BlockType.Ground2,
                InitCollider = true
            };
            _3.SizeInBlocks = new Vector3(50, 3, 0);
            _3.Transform.Position = new Vector3(80 * 16, -40, 0);

            Block pipe = new Block()
            {
                Name = "pipe1",
                Type = Block.BlockType.Pipe1
            };
            pipe.Transform.Position = new Vector3(240, -72, 0);

            Block blck = new Block()
            {
                Name = "random1",
                Type = Block.BlockType.Ground2
            };
            blck.InitNewComponent<Collider>();
            blck.Collider.Area = new System.Drawing.RectangleF(0, 0, 16, 16);
            blck.Transform.Position = new Vector3(400, -56, 0);

            Block blck2 = new Block()
            {
                Type = Block.BlockType.Ground2,
                Name = "random2"
            };

            blck2.Transform.Position = new Vector3(600, -56, 0);
            blck2.InitNewComponent<Collider>();
            blck2.Collider.Area = new System.Drawing.RectangleF(0, 0, 16, 16);

            Enemy goomba = new Enemy()
            {
                Type = Enemy.EnemyType.Goomba
            };
            goomba.Transform.Position = new Vector3(500, -60, 0);

            Mario m = new Mario();
            m.Transform.Position = new Vector3(10, -100, 0);
            //m.InitNewScript<CameraController>();

            new MusicPlayer();

            Camera c = new Camera()
            {
                BackGround = System.Drawing.Color.FromArgb(0xFF, 0x20, 0xEE, 0xEE)
            };
        }

        public override void Set(params string[] Args)
        { }
    }
}
