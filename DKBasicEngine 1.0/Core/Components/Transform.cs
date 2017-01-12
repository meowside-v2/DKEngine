using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKBasicEngine_1_0
{
    public class Transform : I3Dimensional
    {
        public Dimensions Dimensions;
        internal virtual float Width { get { return Dimensions.Width * Scale.X; } }
        internal virtual float Height { get { return Dimensions.Height * Scale.Y; } }
        internal virtual float Depth { get { return Dimensions.Depth * Scale.Z; } }

        public Position Position;
        internal virtual float X { get; }
        internal virtual float Y { get; }
        internal virtual float Z { get; }

        public Scale Scale;
        internal virtual float ScaleX { get; }
        internal virtual float ScaleY { get; }
        internal virtual float ScaleZ { get; }

        public bool LockScaleRatio;
    }
}
