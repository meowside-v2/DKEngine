using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKBasicEngine_1_0
{
    public class Transform : I3Dimensional
    {
        GameObject Parent;

        private Dimensions _Dimensions = new Dimensions();
        public Dimensions Dimensions
        {
            get { return _Dimensions; }
            set
            {
                Dimensions tmp = value - _Dimensions;
                _Dimensions = value;

                int childCount = Parent.Child.Count;
                for (int i = 0; i < childCount; i++)
                    Parent.Child[i].Transform.Dimensions += tmp;
            }
        }

        private Position _Position = new Position();
        public Position Position
        {
            get { return _Position; }
            set
            {
                Position tmp = value - _Position;
                _Position = value;

                int childCount = Parent.Child.Count;
                for (int i = 0; i < childCount; i++)
                    Parent.Child[i].Transform.Position += tmp;
            }
        }

        private Scale _Scale = new Scale();
        public Scale Scale
        {
            get { return _Scale; }
            set
            {
                Scale tmp = value / _Scale;
                _Scale = value;

                int childCount = Parent.Child.Count;
                for (int i = 0; i < childCount; i++)
                    Parent.Child[i].Transform.Scale *= tmp;
            }
        }


        public Transform(GameObject Parent)
        {
            this.Parent = Parent;
        }

        /*public Dimensions Dimensions;
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

        public bool LockScaleRatio;*/
    }
}
