using DKEngine.Core;

namespace DKEngine
{
    public sealed class Border : GameObject
    {
        public Border()
        { }

        public Border(GameObject Parent)
            : base(Parent)
        { }

        protected override void Initialize()
        {
            this.TypeName = "border";
        }
    }
}