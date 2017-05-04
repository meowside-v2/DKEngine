using DKEngine.Core;
using DKEngine.Core.Components;
using MarIO.Assets.Scripts;
using System;
using System.Collections.Generic;

namespace MarIO.Assets.Models
{
    internal class Block : AnimatedObject
    {
        public enum BlockType
        {
            Ground1,
            Ground2,
            Ground3,
            Ground4,
            Bridge,
            Bush1,
            Bush2,
            Bush3,
            BushSmall,
            CastleBig,
            CastleSmall,
            Cloud1,
            Cloud2,
            Cloud3,
            Fence,
            Finish,
            Flag,
            FlagPole,
            Mountain,
            Sky,
            Water1,
            Water2,
            Pipe1,
            Pipe2,
            Pipe3,
            Pipe4,
            Pipe5,
            UnderGround1,
            UnderGround2,
            UnderGround3,
            UnderGround4,
            UnderGroundBackground1,
            UnderGroundBackground2,
            NumberOfObjects
        }

        public static Dictionary<BlockType, string> BlockTypeNames = new Dictionary<BlockType, string>()
        {
            { BlockType.Bridge, "bridge" },
            { BlockType.Bush1, "bush_01" },
            { BlockType.Bush2, "bush_02" },
            { BlockType.Bush3, "bush_03" },
            { BlockType.BushSmall, "bush_small" },
            { BlockType.CastleBig, "castle_big" },
            { BlockType.CastleSmall, "castle_small" },
            { BlockType.Cloud1, "cloud_01" },
            { BlockType.Cloud2, "cloud_02" },
            { BlockType.Cloud3, "cloud_03" },
            { BlockType.Fence, "fence" },
            { BlockType.Flag, "finish_flag" },
            { BlockType.FlagPole, "flag_pole" },
            { BlockType.Ground1, "block_01" },
            { BlockType.Ground2, "block_02" },
            { BlockType.Ground3, "block_03" },
            { BlockType.Ground4, "block_04" },
            { BlockType.Mountain, "mountain" },
            { BlockType.Pipe1, "pipe_01" },
            { BlockType.Pipe2, "pipe_02" },
            { BlockType.Pipe3, "pipe_03" },
            { BlockType.Pipe4, "pipe_04" },
            { BlockType.Pipe5, "pipe_05" },
            { BlockType.Sky, "sky" },
            { BlockType.UnderGround1, "underground_block_01" },
            { BlockType.UnderGround2, "underground_block_02" },
            { BlockType.UnderGround3, "underground_block_03" },
            { BlockType.UnderGround4, "underground_block_04" },
            { BlockType.UnderGroundBackground1, "background_01" },
            { BlockType.UnderGroundBackground2, "background_02" },
            { BlockType.Water1, "water_01" },
            { BlockType.Water2, "water_02" },
        };

        public enum CollisionState
        {
            Stay,
            Up,
            Down
        }

        public BlockType Type { get; set; }
        public delegate void PipeEnter();
        public bool InitCollider { get; set; }
        public CollisionState State { get; set; }

        private event PipeEnter PipeEnterEvent;

        public Block()
            : base()
        { }

        public Block(GameObject Parent)
            : base(Parent)
        { }

        protected override void Initialize()
        {
            this.TypeName = BlockTypeNames[Type];
            if(InitCollider)
                this.InitNewComponent<Collider>();

            switch (Type)
            {
                case BlockType.Ground1:
                    break;

                case BlockType.Ground2:
                    break;

                case BlockType.Ground3:
                    break;

                case BlockType.Ground4:
                    break;

                case BlockType.Bridge:
                    break;

                case BlockType.Bush1:
                    break;

                case BlockType.Bush2:
                    break;

                case BlockType.Bush3:
                    break;

                case BlockType.BushSmall:
                    break;

                case BlockType.CastleBig:
                    break;

                case BlockType.CastleSmall:
                    break;

                case BlockType.Cloud1:
                    break;

                case BlockType.Cloud2:
                    break;

                case BlockType.Cloud3:
                    break;

                case BlockType.Fence:
                    break;

                case BlockType.Finish:
                    {
                        this.Transform.Dimensions = new Vector3(32, 200, 0);

                        Block part1 = new Block(this)
                        {
                            Name = string.Format("{0}_Flag", this.Name),
                            TypeName = Block.BlockTypeNames[BlockType.Flag]
                        };
                        Block part2 = new Block(this)
                        {
                            Name = string.Format("{0}_Pole", this.Name),
                            TypeName = Block.BlockTypeNames[BlockType.FlagPole]
                        };
                        part2.Transform.Position += new Vector3(16, 0, 0);
                    }
                    break;

                case BlockType.Mountain:
                    break;

                case BlockType.Sky:
                    break;

                case BlockType.Water1:
                    break;

                case BlockType.Water2:
                    break;

                case BlockType.Pipe1:
                    {
                        this.InitNewComponent<Collider>();
                        this.Collider.IsTrigger = true;
                        this.Collider.Area = new System.Drawing.RectangleF(-2, 0, 1, this.Transform.Dimensions.Y);

                        this.InitNewScript<PipePort>();

                        Blocker block = new Blocker(this)
                        {
                            Name = string.Format("{0}_Blocker", this.Name)
                        };
                        block.InitNewComponent<Collider>();
                        block.Collider.Area = new System.Drawing.RectangleF(0, 0, this.Transform.Dimensions.X, this.Transform.Dimensions.Y);
                    }
                    break;

                case BlockType.Pipe2:
                    break;

                case BlockType.Pipe3:
                    {
                        this.InitNewComponent<Collider>();
                        this.Collider.IsTrigger = true;
                        this.Collider.Area = new System.Drawing.RectangleF(0, -1, this.Transform.Dimensions.X, 1);

                        this.InitNewScript<PipePort>();

                        Blocker block = new Blocker(this)
                        {
                            Name = string.Format("{0}_Blocker", this.Name)
                        };
                        block.InitNewComponent<Collider>();
                        block.Collider.Area = new System.Drawing.RectangleF(0, 0, this.Transform.Dimensions.X, this.Transform.Dimensions.Y);
                    }
                    break;

                case BlockType.Pipe4:
                    break;

                case BlockType.Pipe5:
                    break;

                case BlockType.UnderGround1:
                    this.InitNewComponent<Collider>();
                    this.InitNewScript<BottomMarioChecker>();
                    this.Collider.IsTrigger = true;
                    this.Collider.Area = new System.Drawing.RectangleF(0, this.Transform.Dimensions.Y * this.Transform.Scale.Y, this.Transform.Dimensions.X * this.Transform.Scale.X, 1);
                    break;

                case BlockType.UnderGround2:
                    break;

                case BlockType.UnderGround3:
                    break;

                case BlockType.UnderGround4:
                    break;

                case BlockType.UnderGroundBackground1:
                    break;

                case BlockType.UnderGroundBackground2:
                    break;

                default:
                    throw new Exception("A TO SE TI JAK POVEDLO");
            }
        }
    }
}