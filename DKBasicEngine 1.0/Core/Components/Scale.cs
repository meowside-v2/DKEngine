using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKBasicEngine_1_0
{
    public struct Scale
    {
        public float X;
        public float Y;
        public float Z;

        public Scale(float X, float Y, float Z)
        {
            this.X = X <= 0 ? 0.001f : X;
            this.Y = Y <= 0 ? 0.001f : Y;
            this.Z = Z <= 0 ? 0.001f : Z;
        }
    }
}
