using System.Collections.Generic;

namespace DKBasicEngine_1_0
{
    public class xRectangle : ICore, I3Dimensional
    {
        private float _scaleX = 1;
        private float _scaleY = 1;
        private float _scaleZ = 1;

        List<I3Dimensional> border = new List<I3Dimensional>();

        public I3Dimensional Parent { get; private set; } = null;

        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public float width { get; set; }
        public float height { get; set; }
        public float depth { get; set; }

        public float ScaleX
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

        public float ScaleY
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

        public float ScaleZ
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
                border.Add(new Border(null, this) { X = i, Z = this.Z });
                border.Add(new Border(null, this) { X = i, Y = height + 8, Z = this.Z });
            }

            for (int i = 8; i <= height; i += 8)
            {
                border.Add(new Border(null, this) { Y = i, Z = this.Z });
                border.Add(new Border(null, this) { X = width + 8, Y = i, Z = this.Z });
            }

            lock (Engine.ToStart)
            {
                lock (Engine.ToUpdate)
                {
                    Engine.ToStart.Add(this);
                    Engine.ToUpdate.Add(this);
                }
            }
        }

        public void Start() { }

        public void Update() { }

        public void Destroy() { }

        public void Render() { }
        
    }
}
