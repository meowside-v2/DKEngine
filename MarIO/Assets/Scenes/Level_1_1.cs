using DKEngine.Core;
using DKEngine.Core.Components;
using MarIO.Assets.Models;
using MarIO.Assets.Models.Miscellaneous;
using MarIO.Assets.Scripts;

namespace MarIO.Assets.Scenes
{
    public class Level_1_1 : MapBase
    {
        private const int offset = 520;

        public Level_1_1()
        {
            Name = MapBase.LevelsNames[nameof(Level_1_1)];
            Shared.Mechanics.LastWorldType = typeof(Level_1_1);
        }

        public override void Load()
        {

            /*-------------- BG PRESET ----------------*/
            for(int i = 0; i < 8; i++)
            {
                new Block()
                {
                    Name = $"cloud_1_{i}",
                    Type = Block.BlockType.Cloud1,
                }.Transform.Position = new Vector3(50 + i * offset, -82, -2);

                new Block()
                {
                    Name = $"cloud_2_{i}",
                    Type = Block.BlockType.Cloud1,
                }.Transform.Position = new Vector3(160 + i * offset, -122, -2);

                new Block()
                {
                    Name = $"cloud_3_{i}",
                    Type = Block.BlockType.Cloud3,
                }.Transform.Position = new Vector3(260 + i * offset, -70, -2);

                new Block()
                {
                    Name = $"cloud_4_{i}",
                    Type = Block.BlockType.Cloud2,
                }.Transform.Position = new Vector3(370 + i * offset, -103, -2);

                new Block()
                {
                    Name = $"mountain_1_{i}",
                    Type = Block.BlockType.Mountain
                }.Transform.Position = new Vector3(120 + i * offset, 16, -2);

                new Block()
                {
                    Name = $"mountain_2_{i}",
                    Type = Block.BlockType.Mountain
                }.Transform.Position = new Vector3(250 + i * offset, 16, -2);

                new Block()
                {
                    Name = $"bush_1_{i}",
                    Type = Block.BlockType.Bush1
                }.Transform.Position = new Vector3(5 + i * offset, 24, -1);

                new Block()
                {
                    Name = $"bush_2_{i}",
                    Type = Block.BlockType.Bush3
                }.Transform.Position = new Vector3(210 + i * offset, 24, -1);

                new Block()
                {
                    Name = $"bush_3_{i}",
                    Type = Block.BlockType.Bush2
                }.Transform.Position = new Vector3(375 + i * offset, 24, -1);
            }

            #region GROUND
            Group _1 = new Group()
            {
                Name = "ground1",
                InitCollider = true,
                Type = Block.BlockType.Ground2
            };
            _1.SizeInBlocks = new Vector3(64, 3, 0);
            _1.Transform.Position = new Vector3(0, 48, 0);

            Group _2 = new Group()
            {
                Name = "ground2",
                InitCollider = true,
                Type = Block.BlockType.Ground2
            };
            _2.SizeInBlocks = new Vector3(20, 3, 0);
            _2.Transform.Position = new Vector3(1056, 48, 0);

            Group _3 = new Group()
            {
                Name = "ground3",
                InitCollider = true,
                Type = Block.BlockType.Ground2
            };
            _3.SizeInBlocks = new Vector3(68, 3, 0);
            _3.Transform.Position = new Vector3(1424, 48, 0);

            Group _4 = new Group()
            {
                Name = "ground4",
                InitCollider = true,
                Type = Block.BlockType.Ground2
            };
            _4.SizeInBlocks = new Vector3(100, 3, 0);
            _4.Transform.Position = new Vector3(2544, 48, 0);
            #endregion

            #region Platform1
            new Block()
            {
                Name = "bonus_1",
                Type = Block.BlockType.Ground1,
                CoinCount = 1,
                InitCollider = true
            }.Transform.Position = new Vector3(320, -12, 0);

            new Block()
            {
                Name = "platform_1",
                Type = Block.BlockType.Ground4,
                InitCollider = true
            }.Transform.Position = new Vector3(400, -12, 0);

            new Block()
            {
                Name = "platform_1",
                Type = Block.BlockType.Ground1,
                InitCollider = true,
                CoinCount = 0,
                PowerUp = true
            }.Transform.Position = new Vector3(416, -12, 0);

            new Block()
            {
                Name = "platform_1",
                Type = Block.BlockType.Ground4,
                InitCollider = true
            }.Transform.Position = new Vector3(432, -12, 0);

            new Block()
            {
                Name = "platform_1",
                Type = Block.BlockType.Ground1,
                InitCollider = true,
                CoinCount = 1
            }.Transform.Position = new Vector3(432, -76, 0);

            new Block()
            {
                Name = "platform_1",
                CoinCount = 1,
                Type = Block.BlockType.Ground1,
                InitCollider = true
            }.Transform.Position = new Vector3(448, -12, 0);

            new Block()
            {
                Name = "platform_1",
                Type = Block.BlockType.Ground4,
                InitCollider = true
            }.Transform.Position = new Vector3(464, -12, 0);
            #endregion

            new Block()
            {
                Name = "pipe",
                Type = Block.BlockType.Pipe3
            }.Transform.Position = new Vector3(544, 16, 1);

            new Goomba().Transform.Position = new Vector3(600, 18, 0);

            {
                GameObject holder = new GameObject();
                holder.Transform.Dimensions = new Vector3(32, 64, 0);
                holder.Transform.Position = new Vector3(700, 32, 0);
                holder.InitNewComponent<Collider>();

                new Block(holder)
                {
                    Name = "pipe",
                    Type = Block.BlockType.Pipe4
                }.Transform.Position += new Vector3(0, 0, -1);

                new Block(holder)
                {
                    Name = "pipe",
                    Type = Block.BlockType.Pipe3
                }.Transform.Position += new Vector3(0, -32, 1);
            }

            new Goomba().Transform.Position = new Vector3(760, 18, 0);
            new Goomba().Transform.Position = new Vector3(800, 18, 0);

            {
                GameObject holder = new GameObject();
                holder.Transform.Dimensions = new Vector3(32, 64, 0);
                holder.Transform.Position = new Vector3(860, 16, 0);
                holder.InitNewComponent<Collider>();

                new Block(holder)
                {
                    Name = "pipe",
                    Type = Block.BlockType.Pipe4
                }.Transform.Position += new Vector3(0, 0, -1);

                new Block(holder)
                {
                    Name = "pipe",
                    Type = Block.BlockType.Pipe3
                }.Transform.Position += new Vector3(0, -32, 1);
            }

            new Block()
            {
                Type = Block.BlockType.Ground4,
                InitCollider = true
            }.Transform.Position = new Vector3(1184, -12, 0);

            new Block()
            {
                Type = Block.BlockType.Ground1,
                CoinCount = 3,
                InitCollider = true
            }.Transform.Position = new Vector3(1200, -12, 0);

            new Block()
            {
                Type = Block.BlockType.Ground4,
                InitCollider = true
            }.Transform.Position = new Vector3(1216, -12, 0);

            for (int i = 0; i < 10; i++)
            {
                new Block()
                {
                    Type = Block.BlockType.Ground4,
                    InitCollider = true
                }.Transform.Position = new Vector3(1232 + i * 16, -76, 0);
            }

            for (int i = 0; i < 3; i++)
            {
                new Block()
                {
                    Type = Block.BlockType.Ground4,
                    InitCollider = true
                }.Transform.Position = new Vector3(1440 + i * 16, -76, 0);
            }

            new Block()
            {
                Type = Block.BlockType.Ground1,
                InitCollider = true,
                CoinCount = 1
            }.Transform.Position = new Vector3(1488, -76, 0);

            new Block()
            {
                Type = Block.BlockType.Ground4,
                InitCollider = true,
                CoinCount = 5
            }.Transform.Position = new Vector3(1488, -12, 0);

            new Block()
            {
                Type = Block.BlockType.Ground4,
                InitCollider = true,
                CoinCount = 5
            }.Transform.Position = new Vector3(1616, -12, 0);

            new Block()
            {
                Type = Block.BlockType.Ground4,
                InitCollider = true,
                CoinCount = 1
            }.Transform.Position = new Vector3(1632, -12, 0);

            #region Bonus Field

            new Block()
            {
                Type = Block.BlockType.Ground1,
                InitCollider = true,
                CoinCount = 1
            }.Transform.Position = new Vector3(1680, -12, 0);

            new Block()
            {
                Type = Block.BlockType.Ground1,
                InitCollider = true,
                CoinCount = 1
            }.Transform.Position = new Vector3(1744, -12, 0);

            new Block()
            {
                Type = Block.BlockType.Ground1,
                InitCollider = true,
                PowerUp = true
            }.Transform.Position = new Vector3(1744, -76, 0);

            new Block()
            {
                Type = Block.BlockType.Ground1,
                InitCollider = true,
                CoinCount = 1
            }.Transform.Position = new Vector3(1808, -12, 0);

            #endregion

            new Block()
            {
                Type = Block.BlockType.Ground4,
                InitCollider = true
            }.Transform.Position = new Vector3(1968, -12, 0);

            for(int i = 0; i < 3; i++)
            {
                new Block()
                {
                    Type = Block.BlockType.Ground4,
                    InitCollider = true
                }.Transform.Position = new Vector3(2000 + i * 16, -76, 0);
            }

            new Block()
            {
                Type = Block.BlockType.Ground4,
                InitCollider = true
            }.Transform.Position = new Vector3(2080, -76, 0);

            new Block()
            {
                Type = Block.BlockType.Ground1,
                InitCollider = true,
                CoinCount = 1
            }.Transform.Position = new Vector3(2096, -76, 0);

            new Block()
            {
                Type = Block.BlockType.Ground1,
                InitCollider = true,
                CoinCount = 1
            }.Transform.Position = new Vector3(2112, -76, 0);

            new Block()
            {
                Type = Block.BlockType.Ground4,
                InitCollider = true
            }.Transform.Position = new Vector3(2096, -12, 0);

            new Block()
            {
                Type = Block.BlockType.Ground4,
                InitCollider = true
            }.Transform.Position = new Vector3(2112, -12, 0);

            new Block()
            {
                Type = Block.BlockType.Ground4,
                InitCollider = true
            }.Transform.Position = new Vector3(2128, -12, 0);

            #region Stairs1

            new Group()
            {
                InitCollider = true,
                SizeInBlocks = new Vector3(4, 1, 0),
                Type = Block.BlockType.Ground3
            }.Transform.Position = new Vector3(2192, 32, 0);

            new Group()
            {
                InitCollider = true,
                SizeInBlocks = new Vector3(3, 1, 0),
                Type = Block.BlockType.Ground3
            }.Transform.Position = new Vector3(2208, 16, 0);

            new Group()
            {
                InitCollider = true,
                SizeInBlocks = new Vector3(2, 1, 0),
                Type = Block.BlockType.Ground3
            }.Transform.Position = new Vector3(2224, 0, 0);

            new Group()
            {
                InitCollider = true,
                SizeInBlocks = new Vector3(1, 1, 0),
                Type = Block.BlockType.Ground3
            }.Transform.Position = new Vector3(2240, -16, 0);

            #endregion

            #region Stairs2

            new Group()
            {
                InitCollider = true,
                SizeInBlocks = new Vector3(4, 1, 0),
                Type = Block.BlockType.Ground3
            }.Transform.Position = new Vector3(2288, 32, 0);

            new Group()
            {
                InitCollider = true,
                SizeInBlocks = new Vector3(3, 1, 0),
                Type = Block.BlockType.Ground3
            }.Transform.Position = new Vector3(2288, 16, 0);

            new Group()
            {
                InitCollider = true,
                SizeInBlocks = new Vector3(2, 1, 0),
                Type = Block.BlockType.Ground3
            }.Transform.Position = new Vector3(2288, 0, 0);

            new Group()
            {
                InitCollider = true,
                SizeInBlocks = new Vector3(1, 1, 0),
                Type = Block.BlockType.Ground3
            }.Transform.Position = new Vector3(2288, -16, 0);

            #endregion

            #region Stairs3

            new Group()
            {
                InitCollider = true,
                SizeInBlocks = new Vector3(5, 1, 0),
                Type = Block.BlockType.Ground3
            }.Transform.Position = new Vector3(2432, 32, 0);

            new Group()
            {
                InitCollider = true,
                SizeInBlocks = new Vector3(4, 1, 0),
                Type = Block.BlockType.Ground3
            }.Transform.Position = new Vector3(2448, 16, 0);

            new Group()
            {
                InitCollider = true,
                SizeInBlocks = new Vector3(3, 1, 0),
                Type = Block.BlockType.Ground3
            }.Transform.Position = new Vector3(2464, 0, 0);

            new Group()
            {
                InitCollider = true,
                SizeInBlocks = new Vector3(2, 1, 0),
                Type = Block.BlockType.Ground3
            }.Transform.Position = new Vector3(2480, -16, 0);

            #endregion

            #region Stairs4

            new Group()
            {
                InitCollider = true,
                SizeInBlocks = new Vector3(4, 1, 0),
                Type = Block.BlockType.Ground3
            }.Transform.Position = new Vector3(2544, 32, 0);

            new Group()
            {
                InitCollider = true,
                SizeInBlocks = new Vector3(3, 1, 0),
                Type = Block.BlockType.Ground3
            }.Transform.Position = new Vector3(2544, 16, 0);

            new Group()
            {
                InitCollider = true,
                SizeInBlocks = new Vector3(2, 1, 0),
                Type = Block.BlockType.Ground3
            }.Transform.Position = new Vector3(2544, 0, 0);

            new Group()
            {
                InitCollider = true,
                SizeInBlocks = new Vector3(1, 1, 0),
                Type = Block.BlockType.Ground3
            }.Transform.Position = new Vector3(2544, -16, 0);

            #endregion

            new Block()
            {
                Type = Block.BlockType.Pipe3
            }.Transform.Position = new Vector3(2704, 16, 1);

            new Block()
            {
                Type = Block.BlockType.Ground4,
                InitCollider = true
            }.Transform.Position = new Vector3(2768, -12, 0);

            new Block()
            {
                Type = Block.BlockType.Ground4,
                InitCollider = true
            }.Transform.Position = new Vector3(2784, -12, 0);

            new Block()
            {
                Type = Block.BlockType.Ground1,
                InitCollider = true,
                CoinCount = 1
            }.Transform.Position = new Vector3(2800, -12, 0);

            new Block()
            {
                Type = Block.BlockType.Ground4,
                InitCollider = true
            }.Transform.Position = new Vector3(2816, -12, 0);

            new Block()
            {
                Type = Block.BlockType.Pipe3
            }.Transform.Position = new Vector3(2928, 16, 1);

            #region Stairs5

            new Group()
            {
                InitCollider = true,
                SizeInBlocks = new Vector3(7, 1, 0),
                Type = Block.BlockType.Ground3
            }.Transform.Position = new Vector3(2960, 32, 0);

            new Group()
            {
                InitCollider = true,
                SizeInBlocks = new Vector3(6, 1, 0),
                Type = Block.BlockType.Ground3
            }.Transform.Position = new Vector3(2976, 16, 0);

            new Group()
            {
                InitCollider = true,
                SizeInBlocks = new Vector3(5, 1, 0),
                Type = Block.BlockType.Ground3
            }.Transform.Position = new Vector3(2992, 0, 0);

            new Group()
            {
                InitCollider = true,
                SizeInBlocks = new Vector3(4, 1, 0),
                Type = Block.BlockType.Ground3
            }.Transform.Position = new Vector3(3008, -16, 0);

            new Group()
            {
                InitCollider = true,
                SizeInBlocks = new Vector3(3, 1, 0),
                Type = Block.BlockType.Ground3
            }.Transform.Position = new Vector3(3024, -32, 0);

            new Group()
            {
                InitCollider = true,
                SizeInBlocks = new Vector3(2, 1, 0),
                Type = Block.BlockType.Ground3
            }.Transform.Position = new Vector3(3040, -48, 0);

            new Group()
            {
                InitCollider = true,
                SizeInBlocks = new Vector3(1, 1, 0),
                Type = Block.BlockType.Ground3
            }.Transform.Position = new Vector3(3056, -64, 0);

            #endregion

            new Block()
            {
                Type = Block.BlockType.CastleBig
            }.Transform.Position = new Vector3(3216, -152, -1);

            Trigger EndOfWorld = new Trigger();
            EndOfWorld.Transform.Position = new Vector3(3216, -40, 0);
            EndOfWorld.Transform.Dimensions = new Vector3(200, 80, 0);
            EndOfWorld.InitNewScript<WorldEnd>();

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

            new SoundOutput();
            new GUIUpdater();
            new BackgroundWorker();
            new MusicPlayer();

            Trigger DeathZone = new Trigger();
            DeathZone.InitNewScript<DeathZoneScript>();
            DeathZone.Transform.Dimensions = new Vector3(5000, 10, 0);
            DeathZone.Transform.Position = new Vector3(0, 100, 0);
        }
    }
}