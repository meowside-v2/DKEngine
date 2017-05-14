using DKEngine.Core;
using DKEngine.Core.Components;
using MarIO.Assets.Models.Miscellaneous;
using MarIO.Assets.Scripts;
using System;
using System.Collections.Generic;
using static DKEngine.Core.Components.Transform;

namespace MarIO.Assets.Models
{
    public class Block : AnimatedObject
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
            NoCoin,
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
            { BlockType.Finish, "" },
            { BlockType.Ground1, "block_1_with_coin" },
            { BlockType.Ground2, "block_02" },
            { BlockType.Ground3, "block_03" },
            { BlockType.Ground4, "block_04" },
            { BlockType.Mountain, "mountain" },
            { BlockType.NoCoin, "block_nocoins" },
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
        public bool InitCollider { get; set; }
        public CollisionState State { get; set; }

        public bool SpecialActionActivate
        {
            get { return _specialAction; }
            set
            {
                if (value)
                {
                    Shared.AnimatedWorldReferences.SpecialActions.Push(this);
                }

                _specialAction = value;
            }
        }

        public Action SpecialAction { get; set; }
        public Direction PipeEnterDirection { get; set; }
        public bool CoinGot { get; set; }
        public bool PowerUp
        {
            get { return _powerUp; }
            set
            {
                _powerUp = value;
                if (value)
                    _hadBonus = true;
            }
        }
        public byte CoinCount
        {
            get { return _coinCount; }
            set
            {
                _coinCount = value;
                if (value > 0)
                    _hadBonus = true;
            }
        }
        public bool HadBonus
        {
            get { return _hadBonus; }
        }

        private bool _powerUp = false;
        private byte _coinCount = 0;
        private bool _hadBonus = false;
        private bool _specialAction = false;
        private SoundOutput FX_Player;

        public Block()
            : base()
        { }

        public Block(GameObject Parent)
            : base(Parent)
        { }

        protected override void Initialize()
        {
            this.TypeName = BlockTypeNames[Type];
            if (InitCollider)
                this.InitNewComponent<Collider>();

            switch (Type)
            {
                case BlockType.Finish:
                    {
                        this.Transform.Dimensions = new Vector3(32, 200, 0);

                        Block part1 = new Block(this)
                        {
                            Name = string.Format("{0}_Flag", this.Name),
                            Type = BlockType.Flag
                        };
                        Block part2 = new Block(this)
                        {
                            Name = string.Format("{0}_Pole", this.Name),
                            Type = BlockType.FlagPole
                        };
                        part2.Transform.Position -= new Vector3(16, 0, 0);
                    }
                    break;

                case BlockType.Pipe1:
                    {
                        PipeEnterDirection = Direction.Right;
                        this.InitNewComponent<Collider>();
                        this.Collider.IsTrigger = true;
                        this.Collider.Area = new System.Drawing.RectangleF(-1, 15, 1, 1);

                        this.InitNewScript<PipePort>();

                        Blocker block = new Blocker(this)
                        {
                            Name = string.Format("{0}_Blocker", this.Name)
                        };
                        block.InitNewComponent<Collider>();
                        block.Collider.Area = new System.Drawing.RectangleF(0, 0, this.Transform.Dimensions.X, this.Transform.Dimensions.Y);
                    }
                    break;

                case BlockType.Pipe3:
                    {
                        PipeEnterDirection = Direction.Down;
                        this.InitNewComponent<Collider>();
                        this.Collider.IsTrigger = true;
                        this.Collider.Area = new System.Drawing.RectangleF(15, -1, 1, 1);

                        this.InitNewScript<PipePort>();

                        Blocker block = new Blocker(this)
                        {
                            Name = string.Format("{0}_Blocker", this.Name)
                        };
                        block.InitNewComponent<Collider>();
                        block.Collider.Area = new System.Drawing.RectangleF(0, 0, this.Transform.Dimensions.X, this.Transform.Dimensions.Y);
                    }
                    break;
            }

            if (CoinCount > 0 || PowerUp)
            {
                this.InitNewComponent<Animator>();
                this.Animator.AddAnimation("default", this.TypeName);
                this.Animator.AddAnimation("nocoin", BlockTypeNames[BlockType.NoCoin]);
            }

            FX_Player = GameObject.Find<SoundOutput>(nameof(SoundOutput));
        }

        public void GetContent()
        {
            if (PowerUp)
            {
                GameObject.Instantiate<PowerUp>(new Vector3(this.Transform.Position.X + 4, this.Transform.Position.Y, this.Transform.Position.Z - 1), new Vector3(), new Vector3(1, 1, 1));
                PowerUp = false;
                this.Animator.Play("nocoin");
            }

            else if (CoinCount > 0 && !CoinGot)
            {
                GameObject.Instantiate<Coin>(new Vector3(this.Transform.Position.X + 4, this.Transform.Position.Y, this.Transform.Position.Z - 1), new Vector3(), new Vector3(1, 1, 1)).AddAsFloatingCoin();
                CoinCount--;
                Shared.Mechanics.GameScore += Shared.Mechanics.COIN_SCORE;
                Shared.Mechanics.FXSoundSource.PlaySound(Coin.COIN_FX);
                CoinGot = true;

                if (CoinCount == 0)
                {
                    this.Animator.Play("nocoin");
                }
            }

            
        }

        public void DestroyAnim()
        { }
    }
}