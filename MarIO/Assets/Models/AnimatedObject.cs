using DKEngine.Core;

namespace MarIO.Assets.Models
{
    public abstract class AnimatedObject : GameObject
    {
        public virtual bool IsDestroyed { get; set; }
        public bool ChangeState = false;

        public AnimatedObject()
            : base()
        { }

        public AnimatedObject(GameObject Parent)
            : base(Parent)
        { }
    }
}