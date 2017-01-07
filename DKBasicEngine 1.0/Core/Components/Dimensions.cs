using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKBasicEngine_1_0
{
    public class Dimensions
    {
        public float Width;
        public float Height;
        public float Depth;

        public Dimensions(float Width, float Height, float Depth)
        {
            this.Width  = Width  < 0 ? 0 : Width;
            this.Height = Height < 0 ? 0 : Height;
            this.Depth  = Depth  < 0 ? 0 : Depth;
        }
    }
}
