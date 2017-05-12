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
            for (int i = 0; i < Transform.Dimensions.Y; i += tmp.Height)
            {
                for (int j = 0; j < Transform.Dimensions.X; j += tmp.Width)
                {
                    Block newBlock = new Block(this);

                    newBlock.Type = Type;
                    newBlock.Transform.Position += new Vector3(j * this.Transform.Scale.X, i * this.Transform.Scale.Y, 0);
                    newBlock.Name = string.Format("{0}_{1}_{2}", Name, j, i);
                }
            }

            if (InitCollider)
                this.InitNewComponent<Collider>();
        }
    }
}