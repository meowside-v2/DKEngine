using DKEngine.Core;

namespace MarIO.Assets.Models
{
    internal abstract class AnimatedObject : GameObject
    {
        public bool IsDestroyed = false;
        public bool ChangeState = false;

        public AnimatedObject()
            : base()
        { }

        public AnimatedObject(GameObject Parent)
            : base(Parent)
        { }
    }
}