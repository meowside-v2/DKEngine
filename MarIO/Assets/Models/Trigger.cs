using DKEngine.Core;
using DKEngine.Core.Components;

namespace MarIO.Assets.Models
{
    public class Trigger : GameObject
    {
        public Trigger()
            : base()
        { }

        public Trigger(GameObject Parent)
            : base(Parent)
        { }

        protected override void Initialize()
        {
            this.InitNewComponent<Collider>();
            this.Collider.IsTrigger = true;
        }
    }
}