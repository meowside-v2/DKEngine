using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKBasicEngine_1_0
{
    public class Border : GameObject
    {
        public Border(int x, int y, int z)
            :base(x, y, z)
        {
            this.TypeName = "border";
        }
    }
}
