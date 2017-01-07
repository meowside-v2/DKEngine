using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKBasicEngine_1_0
{
    public class I3Dimensional
    {
        public Dimensions Dimensions;
        internal virtual float width { get { return Dimensions.Width * Scale.X; } }
        internal virtual float height { get { return Dimensions.Height * Scale.Y; } }
        internal virtual float depth { get { return Dimensions.Depth * Scale.Z; } }

        public Transform Transform;
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
