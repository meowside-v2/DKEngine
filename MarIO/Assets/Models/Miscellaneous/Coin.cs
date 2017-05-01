using DKEngine.Core;
using DKEngine.Core.Components;

namespace MarIO.Assets.Models.Miscellaneous
{
    internal class Coin : GameObject
    {
        public Coin()
        { }

        public Coin(GameObject Parent)
            : base(Parent)
        { }

        protected override void Initialize()
        {
            this.Name = "coin";
            //this.TypeName = "coin";
            this.InitNewComponent<Animator>();
            this.Animator.AddAnimation("default", Database.GetGameObjectMaterial("coin"));
        }
    }
}