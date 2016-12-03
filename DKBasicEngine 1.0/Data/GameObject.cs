using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace DKBasicEngine_1_0
{
    public class GameObject : ICore, I3Dimensional, IGraphics
    {
        I3Dimensional Parent;

        public bool HasShadow { get; set; }

        public Collider collider;

        protected double _scaleX = 1;
        protected double _scaleY = 1;
        protected double _scaleZ = 1;

        protected double _x = 0;
        protected double _y = 0;

        protected string _typeName = "";
        private bool _changed;

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
                    this.modelBase = Database.GetGameObjectMaterial(value);
                }
                catch (Exception e)
                {
                    MessageBox.Show("Object nenalezen\n " + e);
                }
            }
        }

        public double X
        {
            get { return (_x + Parent.X) * (Parent.ScaleX * this.ScaleX); }
            set { _x = value; }
        }
        public double Y
        {
            get { return (_y + Parent.Y) * (Parent.ScaleY * this.ScaleY); }
            set { _y = value; }
        }
        public double Z { get; set; }

        public double width
        {
            get { return (modelBase == null ? 0 : modelBase.width * ScaleX * Parent.ScaleX); }
            set { }
        }
        public double height
        {
            get { return (modelBase == null ? 0 : modelBase.height * ScaleY * Parent.ScaleY); }
            set { }
        }
        public double depth
        {
            get { return 0; }
            set { }
        }

        public double ScaleX
        {
            get
            {
                return _scaleX;
            }
            set
            {
                if(value != _scaleX)
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
                if(value != _scaleY)
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
                if(value != _scaleZ)
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

        public int AnimationState { get; set; } = 0;

        public Material modelBase { get; set; }
        public Material modelRastered { get; set; }

        public GameObject(I3Dimensional Parent)
        {
            this.X = 0;
            this.Y = 0;
            this.Z = 0;

            this.Parent = Parent;

            this.Start();

            lock (Engine.ToUpdate)
            {
                Engine.ToUpdate.Add(this);
            }
        }

        public virtual void Start()
        { }

        public virtual void Update()
        {
            if (_changed)
            {
                modelRastered = new Material(modelBase, this);
            }
        }

        public void Render()
        {
            modelRastered?.Render(this);
        }
    }
}
