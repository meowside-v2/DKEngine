using DKEngine.Core;
using DKEngine.Core.Components;

namespace MarIO.Assets.Models.Miscellaneous
{
    public class Heart : GameObject
    {
        public Heart()
        { }

        public Heart(GameObject Parent)
            : base(Parent)
        { }

        protected override void Initialize()
        {
            this.Name = "heart";
            //this.TypeName = "coin";
            this.InitNewComponent<Animator>();
            this.Animator.AddAnimation("default", Database.GetGameObjectMaterial("heart"));
        }
    }
}