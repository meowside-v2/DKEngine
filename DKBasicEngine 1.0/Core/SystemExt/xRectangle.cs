using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKBasicEngine_1_0
{
    public class xRectangle : ICore, I3Dimensional
    {
        private double _scaleX = 1;
        private double _scaleY = 1;
        private double _scaleZ = 1;

        List<I3Dimensional> border = new List<I3Dimensional>();

        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public int width { get; set; }
        public int height { get; set; }
        public int depth { get; set; }

        public double ScaleX
        {
            get
            {
                return _scaleX;
            }
            set
            {
                if (value < 0.1f)
                    _scaleX = 0.1f;

                else
                    _scaleX = value;
            }
        }

        public double ScaleY
        {
            get
            {
                return _scaleY;
            }
            set
            {
                if (value < 0.1f)
                    _scaleY = 0.1f;

                else
                    _scaleY = value;
            }
        }

        public double ScaleZ
        {
            get
            {
                return _scaleZ;
            }
            set
            {
                if (value < 0.1f)
                    _scaleZ = 0.1f;

                else
                    _scaleZ = value;
            }
        }

        public xRectangle(int x, int y, int z, int width, int height)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;

            this.width = width;
            this.height = height;

            this.border.Clear();

            for (int i = 0; i <= width + 8; i += 8)
            {
                border.Add(new Border(X + i, Y, this.Z));
                border.Add(new Border(X + i, Y + height + 8, this.Z));
            }

            for (int i = 8; i <= height; i += 8)
            {
                border.Add(new Border(X, Y + i, this.Z));
                border.Add(new Border(X + width + 8, Y + i, this.Z));
            }
        }

        public object DeepCopy()
        {
            return this.MemberwiseClone();
        }

        public void Render(int x, int y, byte[] bufferData, bool[] bufferKey)
        {
            List<I3Dimensional> temp;

            lock (border)
            {
                temp = border.Where(item => Finder(item, x, y)).ToList();
            }

            while (temp.Count > 0)
            {
                if (BufferIsFull(bufferKey))
                    return;

                int tempHeight = temp.Max(item => (item).Z);
                List<I3Dimensional> toRender = temp.Where(item => (item).Z == tempHeight).ToList();

                foreach (ICore item in toRender)
                {
                    item.Render(x, y, bufferData, bufferKey);
                }

                temp.RemoveAll(item => toRender.FirstOrDefault(item2 => ReferenceEquals(item, item2)) != null);
            }
            //model.Render(x, y, bufferData, bufferKey);
        }

        private bool Finder(I3Dimensional obj, int x, int y)
        {
            return (obj.X + obj.width >= x && obj.X < x + Shared.RenderWidth && obj.Y + obj.height >= y && obj.Y < y + Shared.RenderHeight);
        }

        private bool FindBiggerZ(I3Dimensional item1, I3Dimensional item2)
        {
            return (item1.Z > item2.Z);
        }

        private bool BufferIsFull(bool[] key)
        {
            return !(key.Contains(false));
        }
    }
}
