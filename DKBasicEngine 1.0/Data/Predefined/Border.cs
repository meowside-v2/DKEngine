using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKBasicEngine_1_0
{
    public class Border : GameObject
    {
        public Border(I3Dimensional Parent)
            :base(Parent)
        {
            this.TypeName = "border";
        }
    }
}
