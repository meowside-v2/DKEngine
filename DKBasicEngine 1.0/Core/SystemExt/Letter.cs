using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKBasicEngine_1_0
{
    public class Letter : ICore, I3Dimensional, IGraphics
    {
        TextBlock Parent;
        private double _x;
        private double _y;

        public double X
        {
            get
            {
                return _x * Parent.ScaleX;
            }
            set
            {
                _x = value;
            }
        }
        public double Y
        {
            get
            {
                return _y * Parent.ScaleY;
            }
            set
            {
                _y = value;
            }
        }
        public double Z { get; set; }

        public double width
        {
            get
            {
                return model.width * Parent.ScaleX;
            }
        }

        public double height
        {
            get
            {
                return model.height * Parent.ScaleY;
            }
        }

        public double depth { get; }

        public double ScaleX
        {
            get
            {
                return Parent.ScaleX;
            }

            set { }
        }

        public double ScaleY
        {
            get
            {
                return Parent.ScaleY;
            }

            set { }
        }

        public double ScaleZ
        {
            get
            {
                return Parent.ScaleZ;
            }

            set { }
        }

        public Material model { get; set; }

        public int AnimationState { get; set; }

        public Letter(TextBlock Parent, double x, double y, double z, Material sourceModel)
        {
            model = sourceModel;

            this.X = x;
            this.Y = y;
            this.Z = z;

            this.Parent = Parent;
        }

        public object DeepCopy()
        {
            return this.MemberwiseClone();
        }

        public void Render(int x, int y, byte[] imageBuffer, bool[] imageBufferKey)
        {
            model.Render((int)(X + x), (int)(Y + y), 0, imageBuffer, imageBufferKey, Parent.ScaleX, Parent.ScaleY);
        }

        public void Render(int x, int y, byte[] imageBuffer, bool[] imageBufferKey, Color? clr)
        {
            model.Render((int)(X + x), (int)(Y + y), 0, imageBuffer, imageBufferKey, ScaleX, ScaleY, clr);
        }
    }
}
