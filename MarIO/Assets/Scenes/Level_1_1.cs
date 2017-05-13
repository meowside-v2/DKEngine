using DKEngine.Core.Components;
using MarIO.Assets.Models;
using MarIO.Assets.Models.Miscellaneous;
using MarIO.Assets.Scripts;

namespace MarIO.Assets.Scenes
{
    public class Level_1_1 : MapBase
    {
        public Level_1_1()
        {
            Name = MapBase.LevelsNames[nameof(Level_1_1)];
        }

        public override void Load()
        {
            new GUIUpdater();
            new BackgroundWorker();

            Trigger DeathZone = new Trigger();
            DeathZone.InitNewScript<DeathZoneScript>();
            DeathZone.Transform.Dimensions = new Vector3(3200, 10, 0);
            DeathZone.Transform.Position = new Vector3(0, 50, 0);

            Group _1 = new Group()
            {
                Name = "ground1",
                InitCollider = true,
                Type = Block.BlockType.Ground2
            };
            _1.SizeInBlocks = new Vector3(50, 3, 0);
            _1.Transform.Position = new Vector3(0, 0, 0);

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

            Mario m = new Mario()
            {
                InitCameraController = true,
                InitCharacterController = true,
                InitCollider = true
            };
            m.Transform.Position = new Vector3(10, -10, 0);

            Camera c = new Camera()
            {
                BackGround = Shared.Mechanics.OverworldBackground.ToColor()
            };

            PowerUp test = new PowerUp();
            test.Transform.Position = new Vector3(180, -50, 0);

            new SoundOutput();
            new GUIUpdater();
            new BackgroundWorker();
        }
    }
}