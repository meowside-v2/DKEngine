using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace DKBasicEngine_1_0
{
    public class GameObject : ICore, I3Dimensional, IGraphics
    {
        public Collider collider;
        protected double _scaleX = 1;
        protected double _scaleY = 1;
        protected double _scaleZ = 1;

        protected string _typeName = "";

        public string TypeName
        {
            get
            {
                return _typeName;
            }
            set
            {
                _typeName = value;

                try
                {
                    this.model = Database.GetGameObjectMaterial(value);
                }
                catch (Exception e)
                {
                    MessageBox.Show("Object nenalezen\n " + e);
                }
            }
        }

        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public int width
        {
            get
            {
                return (model == null ? 0 : (int)(model.width * ScaleX));
            }
        }
        public int height
        {
            get
            {
                return (model == null ? 0 : (int)(model.height * ScaleY));
            }
        }
        public int depth
        {
            get
            {
                return 0;
            }
        }

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

        public int AnimationState { get; set; } = 0;
        
        public Material model { get; set; }

        public GameObject()
        {
            this.X = 0;
            this.Y = 0;
            this.Z = 0;
        }

        public GameObject(int X, int Y, int Z)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }

        public GameObject(string TypeName, int X, int Y, int Z)
        {
            this.TypeName = TypeName;

            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }

        public object DeepCopy()
        {
            return this.MemberwiseClone();
        }

        public void Render(int x, int y, byte[] bufferData, bool[] bufferKey)
        {
            if (model != null) model.Render(X - x, Y - y, AnimationState, bufferData, bufferKey, ScaleX, ScaleY);
        }
    }
}
