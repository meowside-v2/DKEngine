using System.Collections.Generic;

namespace DKBasicEngine_1_0
{
    public class xRectangle : GameObject, ICore
    {

        public xRectangle(EmptyGameObject Parent)
            : base(Parent)
        { }

        public xRectangle(Scene Scene)
            : base(Scene)
        { }
    }
}
