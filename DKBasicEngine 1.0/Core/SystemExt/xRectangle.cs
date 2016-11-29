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

        //I3Dimensional Parent;

        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public double width { get; set; }
        public double height { get; set; }
        public double depth { get; set; }

        public double ScaleX
        {
            get
            {
                return _scaleX;
            }
            set
            {
                if (value != _scaleX)
                {
                    if (value < 0.1f)
                        _scaleX = 0.1f;

                    else
                        _scaleX = value;

                    if (LockScaleRatio)
                    {
                        _scaleZ = _scaleX;
                        _scaleY = _scaleX;
                    }
                }
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
                if (value != _scaleY)
                {
                    if (value < 0.1f)
                        _scaleY = 0.1f;

                    else
                        _scaleY = value;

                    if (LockScaleRatio)
                    {
                        _scaleX = _scaleY;
                        _scaleZ = _scaleY;
                    }
                }
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
                if (value != _scaleZ)
                {
                    if (value < 0.1f)
                        _scaleZ = 0.1f;

                    else
                        _scaleZ = value;

                    if (LockScaleRatio)
                    {
                        _scaleX = _scaleZ;
                        _scaleY = _scaleZ;
                    }
                }
            }
        }

        public bool LockScaleRatio { get; set; } = true;

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
                border.Add(new Border(this) { X = i, Z = this.Z });
                border.Add(new Border(this) { X = i, Y = height + 8, Z = this.Z });
            }

            for (int i = 8; i <= height; i += 8)
            {
                border.Add(new Border(this) { Y = i, Z = this.Z });
                border.Add(new Border(this) { X = width + 8, Y = i, Z = this.Z });
            }
        }

        public void Start()
        {
            lock (Engine.ToUpdate)
            {
                Engine.ToUpdate.Add(this);
            }
        }
        public void Update() { }

        public void Render()
        {
            List<I3Dimensional> temp;

            lock (border)
            {
                temp = border.Where(item => item.IsInView()).ToList();
            }

            while (temp.Count > 0)
            {
                if (Engine.Render.imageBuffer.BufferIsFull(255, 4))
                    return;

                double tempHeight = temp.Max(item => item.Z);
                List<I3Dimensional> toRender = temp.Where(item => (item).Z == tempHeight).ToList();

                foreach (ICore item in toRender)
                {
                    item.Render();
                }

                temp.RemoveAll(item => toRender.FirstOrDefault(item2 => ReferenceEquals(item, item2)) != null);
            }
        }

        private bool BufferIsFull(bool[] key)
        {
            return !(key.Contains(false));
        }
    }
}
