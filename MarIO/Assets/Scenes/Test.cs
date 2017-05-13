using DKEngine.Core.Components;
using MarIO.Assets.Models;
using MarIO.Assets.Scripts;
using System.Drawing;

namespace MarIO.Assets.Scenes
{
    public class Test : MapBase
    {
        public static string StaticName = "test";

        public Test()
        {
            Name = StaticName;
        }

        public override void Load()
        {
            Group _1 = new Group()
            {
                Name = "ground1",
                InitCollider = true,
                Type = Block.BlockType.Ground2
            };
            _1.SizeInBlocks = new Vector3(50, 3, 0);
            _1.Transform.Position = new Vector3(0, 0, 0);

            Group _2 = new Group()
            {
                Name = "ground2",
                InitCollider = true,
                Type = Block.BlockType.Ground2
            };
            _2.SizeInBlocks = new Vector3(10, 3, 0);
            _2.Transform.Position = new Vector3(60 * 16, 0, 0);

            Group _3 = new Group()
            {
                Name = "ground3",
                Type = Block.BlockType.Ground2,
                InitCollider = true
            };
            _3.SizeInBlocks = new Vector3(50, 3, 0);
            _3.Transform.Position = new Vector3(80 * 16, 0, 0);

            for (int i = 0; i < 10; i++)
            {
                Block tmp = new Block()
                {
                    Type = Block.BlockType.Ground2,
                    Name = string.Format("PlatformTest_{0:00}", i)
                };
                tmp.Transform.Position = new Vector3(80 + 16 * i, -80, 0);
                tmp.InitCollider = true;
            }

            Block pipe = new Block()
            {
                Name = "pipe1",
                Type = Block.BlockType.Pipe1
            };
            pipe.Transform.Position = new Vector3(240, -32, 0);

            Block blck = new Block()
            {
                Name = "random1",
                Type = Block.BlockType.Ground2
            };
            blck.InitNewComponent<Collider>();
            blck.Collider.Area = new System.Drawing.RectangleF(0, 0, 16, 16);
            blck.Transform.Position = new Vector3(400, -16, 0);

            Block blck2 = new Block()
            {
                Type = Block.BlockType.Ground2,
                Name = "random2"
            };

            blck2.Transform.Position = new Vector3(600, -16, 0);
            blck2.InitNewComponent<Collider>();
            blck2.Collider.Area = new System.Drawing.RectangleF(0, 0, 16, 16);

            Goomba goomba = new Goomba();
            goomba.Transform.Position = new Vector3(500, -20, 0);

            Mario m = new Mario()
            {
                InitCameraController = true,
                InitCharacterController = true,
                InitCollider = true
            };
            m.Transform.Position = new Vector3(10, -10, 0);

            new MusicPlayer();

            Camera c = new Camera()
            {
                BackGround = Shared.Mechanics.OverworldBackground.ToColor()
            };

            new GUIUpdater();
            new SoundOutput();
            new BackgroundWorker();

            Trigger DeathZone = new Trigger();
            DeathZone.InitNewScript<DeathZoneScript>();
            DeathZone.Transform.Dimensions = new Vector3(3200, 10, 0);
            DeathZone.Transform.Position = new Vector3(0, 50, 0);
            DeathZone.Model = new Material(Color.Black, DeathZone);
        }
    }
}