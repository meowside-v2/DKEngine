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

        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public double width
        {
            get
            {
                return (model == null ? 0 : model.width * ScaleX);
            }
        }
        public double height
        {
            get
            {
                return (model == null ? 0 : model.height * ScaleY);
            }
        }
        public double depth
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

            this.Start();

            lock (Engine.ToUpdate)
            {
                Engine.ToUpdate.Add(this);
            }
        }

        public virtual void Start()
        { }

        public virtual void Update()
        { }

        public object DeepCopy()
        {
            return this.MemberwiseClone();
        }

        public void Render(int x, int y, byte[] bufferData, bool[] bufferKey)
        {
            if (model != null) model.Render((int)(X - x), (int)(Y - y), AnimationState, bufferData, bufferKey, ScaleX, ScaleY);
        }
    }
}
