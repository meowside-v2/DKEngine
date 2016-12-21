using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace DKBasicEngine_1_0
{
    public class GameObject : ICore, I3Dimensional, IGraphics
    {
        I3Dimensional Parent;
        
        public Animator Animator { get; private set; }
        public Material Model { get; private set; } = null;
        public bool IsGUI { get; set; } = false;
        public bool HasShadow { get; set; }

        public Collider collider;

        protected float _scaleX = 1;
        protected float _scaleY = 1;
        protected float _scaleZ = 1;

        protected float _x = 0;
        protected float _y = 0;

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
                this.Model = Database.GetGameObjectMaterial(value);
            }
        }

        public float X
        {
            get { return Parent != null ? _x + Parent.X : _x; }
            set { _x = value; }
        }
        public float Y
        {
            get { return Parent != null ? _y + Parent.Y : _y; }
            set { _y = value; }
        }
        public float Z { get; set; }

        public float width
        {
            get { return (Model == null ? 0 : Model.width * ScaleX); }
            set { }
        }
        public float height
        {
            get { return (Model == null ? 0 : Model.height * ScaleY); }
            set { }
        }
        public float depth
        {
            get { return 0; }
            set { }
        }

        public float ScaleX
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

        public float ScaleY
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

        public float ScaleZ
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
        

        public GameObject(I3Dimensional Parent = null)
        {
            this.X = 0;
            this.Y = 0;
            this.Z = 0;

            this.Parent = Parent;

            Animator = new Animator(this);

            Engine.ToStart.Add(this);
            Engine.ToUpdate.Add(this);
            Engine.ToRender.Add(this);
            
        }

        public virtual void Start()
        { }

        public virtual void Update()
        {
            Animator?.Update();
        }

        public void Destroy()
        {
            Engine.ToUpdate.Remove(this);
            Engine.ToRender.Remove(this);

            this.Animator = null;
            this.collider = null;
            this.Parent = null;
        }

        public void Render()
        {
            Model?.Render(this);
        }
    }
}
