using System;
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

        protected override void Init()
        {
            this.TypeName = "border";
        }
    }
}
