using DKEngine.Core;
using DKEngine.Core.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarIO.Assets.Models
{
    class Block : GameObject
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
            Flag,
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
            { BlockType.Bush2, "bush02" },
            { BlockType.Bush3, "bush_03" },
            { BlockType.BushSmall, "bush_small" },
            { BlockType.CastleBig, "castle_big" },
            { BlockType.CastleSmall, "castle_small" },
            { BlockType.Cloud1, "cloud_01" },
            { BlockType.Cloud2, "cloud_02" },
            { BlockType.Cloud3, "cloud_03" },
            { BlockType.Fence, "fence" },
            { BlockType.Flag, null },
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

        private BlockType _type = BlockType.Ground1;
        public BlockType Type
        {
            get { return _type; }
            set { _type = value; Init(); }
        }

        public Block()
            :base()
        { }

        public Block(GameObject Parent)
            :base(Parent)
        { }



        protected override void Init()
        {
            switch (Type)
            {
                case BlockType.Ground1:
                    this.TypeName = "block_01";
                    break;
                case BlockType.Ground2:
                    this.TypeName = "block_02";
                    break;
                case BlockType.Ground3:
                    this.TypeName = "block_03";
                    break;
                case BlockType.Ground4:
                    this.TypeName = "block_04";
                    break;
                case BlockType.Bridge:
                    this.TypeName = "bridge";
                    break;
                case BlockType.Bush1:
                    this.TypeName = "bush_01";
                    break;
                case BlockType.Bush2:
                    this.TypeName = "bush_02";
                    break;
                case BlockType.Bush3:
                    this.TypeName = "bush_03";
                    break;
                case BlockType.BushSmall:
                    this.TypeName = "bush_small";
                    break;
                case BlockType.CastleBig:
                    this.TypeName = "castle_big";
                    break;
                case BlockType.CastleSmall:
                    this.TypeName = "castle_small";
                    break;
                case BlockType.Cloud1:
                    this.TypeName = "cloud_01";
                    break;
                case BlockType.Cloud2:
                    this.TypeName = "cloud_02";
                    break;
                case BlockType.Cloud3:
                    this.TypeName = "cloud_03";
                    break;
                case BlockType.Fence:
                    this.TypeName = "fence";
                    break;
                case BlockType.Flag:
                    /*
                     
                    NEEDS TO BE DONE
                     
                     */
                    break;
                case BlockType.Mountain:
                    this.TypeName = "mountain";
                    break;
                case BlockType.Sky:
                    this.TypeName = "sky";
                    break;
                case BlockType.Water1:
                    this.TypeName = "water_01";
                    break;
                case BlockType.Water2:
                    this.TypeName = "water_02";
                    break;
                case BlockType.Pipe1:
                    this.TypeName = "pipe_01";
                    break;
                case BlockType.Pipe2:
                    this.TypeName = "pipe_02";
                    break;
                case BlockType.Pipe3:
                    this.TypeName = "pipe_03";
                    break;
                case BlockType.Pipe4:
                    this.TypeName = "pipe_04";
                    break;
                case BlockType.Pipe5:
                    this.TypeName = "pipe_05";
                    break;
                case BlockType.UnderGround1:
                    this.TypeName = "underground_block_01";
                    break;
                case BlockType.UnderGround2:
                    this.TypeName = "underground_block_02";
                    break;
                case BlockType.UnderGround3:
                    this.TypeName = "underground_block_03";
                    break;
                case BlockType.UnderGround4:
                    this.TypeName = "underground_block_04";
                    break;
                case BlockType.UnderGroundBackground1:
                    this.TypeName = "background_01";
                    break;
                case BlockType.UnderGroundBackground2:
                    this.TypeName = "background_01";
                    break;
                default:
                    throw new Exception("A TO SE TI JAK POVEDLO");
            }
        }
    }
}
