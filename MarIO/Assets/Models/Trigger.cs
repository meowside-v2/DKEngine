using DKEngine.Core;
using DKEngine.Core.Components;

namespace MarIO.Assets.Models
{
    public class Trigger : GameObject
    {
        public Trigger()
            : base()
        {
            this.InitNewComponent<Collider>();
            this.Collider.IsTrigger = true;
        }

        public Trigger(GameObject Parent)
            : base(Parent)
        {
            this.InitNewComponent<Collider>();
            this.Collider.IsTrigger = true;
        }

        protected override void Initialize()
        {
            
        }
    }
}