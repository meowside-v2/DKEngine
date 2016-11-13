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

        public int X
        {
            get
            {
                return (int)(_x * Parent.ScaleX);
            }
            set
            {
                _x = value;
            }
        }
        public int Y
        {
            get
            {
                return (int)(_y * Parent.ScaleY);
            }
            set
            {
                _y = value;
            }
        }
        public int Z { get; set; }

        public int width
        {
            get
            {
                return (int)(model.width * Parent.ScaleX);
            }
        }

        public int height
        {
            get
            {
                return (int)(model.height * Parent.ScaleY);
            }
        }

        public int depth { get; }

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

        public Letter(TextBlock Parent, int x, int y, int z, Material sourceModel)
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
            model.Render(X + x, Y + y, imageBuffer, imageBufferKey, Parent.ScaleX, Parent.ScaleY);
        }

        public void Render(int x, int y, byte[] imageBuffer, bool[] imageBufferKey, Color? clr)
        {
            model.Render(X + x, Y + y, imageBuffer, imageBufferKey, ScaleX, ScaleY, clr);
        }
    }
}
