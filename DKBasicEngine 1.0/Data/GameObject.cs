using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace DKBasicEngine_1_0
{
    public class GameObject : ICore, IGraphics
    {
        public bool IsInView
        {
            get
            {
                float X = this.IsGUI ? 0 : Engine.BaseCam != null ? Engine.BaseCam.X : 0;
                float Y = this.IsGUI ? 0 : Engine.BaseCam != null ? Engine.BaseCam.Y : 0;

                return (this.Transform.Position.X + this.Transform.Dimensions.Width >= X && this.Transform.Position.X < X + Engine.Render.RenderWidth && this.Transform.Position.Y + this.Transform.Dimensions.Height >= Y && this.Transform.Position.Y < Y + Engine.Render.RenderHeight);
            }
        }

        internal Engine.UpdateHandler UpdateHandler;

        public GameObject Parent  = null;
        protected Material _Model = null;
        public Animator Animator  = null;
        public Collider Collider  = null;

        public readonly Transform Transform;
        
        public Material Model
        {
            get { return _Model; }
            set
            {
                if(value != _Model && value != null)
                {
                    _Model = value;
                    this.Transform.Dimensions = new Dimensions(value.Width, value.Height, 1);
                }
            }
        }

        public readonly List<GameObject> Child = new List<GameObject>();

        protected bool _IsGUI = false;
        public bool IsGUI
        {
            get { return Parent != null ? Parent.IsGUI : _IsGUI; }
            set { _IsGUI = value; }
        }

        public virtual bool HasShadow { get; set; }
        
        protected string _typeName = "";
        public string TypeName
        {
            get { return _typeName; }
            set
            {
                _typeName = value;
                this.Model = Database.GetGameObjectMaterial(value);
            }
        }

        public GameObject()
        {
            this.Transform            = new Transform(this);
            this.Transform.Dimensions = new Dimensions(1, 1, 1);
            this.Transform.Scale      = new Scale(1, 1, 1);
            this.Transform.Position   = new Position(0, 0, 0);
            this.Animator             = new Animator(this);

            lock (Engine.ToStart)
                Engine.ToStart.Add(this);

            lock (Engine.ToRender)
                Engine.ToRender.Add(this);

            if (Engine.Scene != null)
                Engine.Scene.Model.Add(this);

            UpdateHandler = new Engine.UpdateHandler(this.Update);
        }

        public GameObject(GameObject Parent)
        {
            this.Transform            = new Transform(this);
            this.Transform.Dimensions = new Dimensions(1, 1, 1);
            this.Transform.Scale      = new Scale(1, 1, 1);
            this.Transform.Position   = new Position(0, 0, 0);
            this.Animator             = new Animator(this);

            lock (Engine.ToStart)
                Engine.ToStart.Add(this);

            lock (Engine.ToRender)
                Engine.ToRender.Add(this);

            if (Parent != null)
            {
                this.Parent = Parent;
                Parent.Child.Add(this);
            }
                
            else if (Engine.Scene != null)
                Engine.Scene.Model.Add(this);

            UpdateHandler = new Engine.UpdateHandler(this.Update);
        }

        public virtual void Start()
        { }

        public virtual void Update()
        { }

        public virtual void Destroy()
        {
            Engine.UpdateEvent -= this.UpdateHandler;
            Engine.ToRender.Remove(this);

            this.Animator = null;
            this.Collider = null;
            this.Parent = null;
        }

        internal virtual void Render()
        { Model?.Render(this); }

        public virtual void OnColliderEnter(Collider e)
        { }
    }
}
