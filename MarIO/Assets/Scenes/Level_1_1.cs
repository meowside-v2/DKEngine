using DKEngine.Core.Components;
using MarIO.Assets.Models;
using MarIO.Assets.Scripts;

namespace MarIO.Assets.Scenes
{
    internal class Level_1_1 : MapBase
    {
        public Level_1_1()
        {
            Name = "1-1";
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
        }
    }
}