using System;

namespace DKBasicEngine_1_0
{
    public class GameObject : ICore, I3Dimensional, IGraphics
    {
        I3Dimensional Parent;

        public bool HasShadow { get; set; }

        public Animator Animator { get; set; }
        public Collider collider;

        protected double _scaleX = 1;
        protected double _scaleY = 1;
        protected double _scaleZ = 1;

        protected double _x = 0;
        protected double _y = 0;

        protected string _typeName = "";
        private bool _changed = false;
        private Material _model;

        public string TypeName
        {
            get
            {
                return _typeName;
            }
            set
            {
                _typeName = value;
                this.Model = Database.GetGameObjectMaterial(value);
            }
        }

        public double X
        {
            get { return Parent != null ? _x + Parent.X : _x; }
            set { _x = value; }
        }
        public double Y
        {
            get { return Parent != null ? _y + Parent.Y : _y; }
            set { _y = value; }
        }
        public double Z { get; set; }

        public double width
        {
            get { return (Model == null ? 0 : Model.width * ScaleX); }
            set { }
        }
        public double height
        {
            get { return (Model == null ? 0 : Model.height * ScaleY); }
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

                    _changed = true;
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

                    _changed = true;
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

                    _changed = true;
                }
            }
        }

        public bool LockScaleRatio { get; set; } = true;
        public Material Model
        {
            get { return _model; }
            set
            {
                _model = value;
                _changed = true;
            }
        }
        private Material modelRastered = null;
        public bool IsGUI { get; set; } = false;

        public GameObject(I3Dimensional Parent = null)
        {
            this.X = 0;
            this.Y = 0;
            this.Z = 0;

            this.Parent = Parent;

            Animator = new Animator(this);

            this.Start();

            Engine.ToUpdate.Add(this);
            Engine.ToRender.Add(this);

            _changed = true;
        }

        public virtual void Start()
        { }

        public virtual void Update()
        {
            Animator?.Update();

            if (_changed)
            {
                if(Model != null) modelRastered = new Material(Model, this);
                _changed = false;
            }
        }

        public void Destroy()
        {
            this.Animator = null;
            this.collider = null;
            this.modelRastered = null;
            this.Parent = null;

            Engine.ToUpdate.Remove(this);
            Engine.ToRender.Remove(this);
        }

        public void Render()
        {
            modelRastered?.Render(this);
        }
    }
}
