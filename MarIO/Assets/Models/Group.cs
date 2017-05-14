using DKEngine.Core;
using DKEngine.Core.Components;

namespace MarIO.Assets.Models
{
    public class Group : GameObject
    {
        public bool InitCollider = false;

        public Group()
            : base()
        { }

        public Group(GameObject Parent)
            : base(Parent)
        { }

        public Block.BlockType Type { get; set; }
        public Vector3 SizeInBlocks { get; set; }

        protected override void Initialize()
        {
            Material tmp = Database.GetGameObjectMaterial(Block.BlockTypeNames[Type]);

            this.Transform.Dimensions = new Vector3(SizeInBlocks.X * tmp.Width, SizeInBlocks.Y * tmp.Height, 0);
            for (int i = 0; i < SizeInBlocks.Y; i++)
            {
                for (int j = 0; j < SizeInBlocks.X; j++)
                {
                    Block newBlock = new Block(this);

                    newBlock.Type = Type;
                    newBlock.Transform.Position += new Vector3(j * tmp.Width * this.Transform.Scale.X, i * tmp.Height * this.Transform.Scale.Y, this.Transform.Position.Z);
                    newBlock.Name = string.Format("{0}_{1}_{2}", Name, j, i);
                }
            }

            if (InitCollider)
                this.InitNewComponent<Collider>();
        }
    }
}