using DKEngine.Core;
using DKEngine.Core.Components;

namespace MarIO.Assets.Models.Miscellaneous
{
    internal class Coin : GameObject
    {
        public static Sound COIN_FX = new Sound(Shared.Assets.Sounds.COIN_GET_FX);

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