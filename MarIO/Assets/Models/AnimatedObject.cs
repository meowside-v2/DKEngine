using DKEngine.Core;

namespace MarIO.Assets.Models
{
    internal abstract class AnimatedObject : GameObject
    {
        enum Direction
        {
            Left,

            Right,
            Down
        }

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