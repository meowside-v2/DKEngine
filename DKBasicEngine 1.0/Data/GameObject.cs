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

                return (this.Transform.Position.X + this.Transform.Dimensions.X >= X && this.Transform.Position.X < X + Engine.Render.RenderWidth && this.Transform.Position.Y + this.Transform.Dimensions.Y >= Y && this.Transform.Position.Y < Y + Engine.Render.RenderHeight);
            }
        }

        protected Material _Model = null;
        public GameObject Parent  = null;
        public Animator Animator  = null;
        private Collider _Collider = null;
        public Collider Collider
        {
            get { return _Collider; }
            set
            {
                if (value == null)
                {
                    int ScriptCount = this.Scripts.Count;
                    for (int i = 0; i < ScriptCount; i++)
                    {
                        _Collider.CollisionEvent -= Scripts[i].ColliderDel;
                    }

                    _Collider = value;
                }
                else
                {
                    _Collider = value;

                    int ScriptCount = this.Scripts.Count;
                    for (int i = 0; i < ScriptCount; i++)
                    {
                        _Collider.CollisionEvent += Scripts[i].ColliderDel;
                    }
                }
            }
        }

        public readonly Transform Transform;
        public readonly List<Script> Scripts;

        public Material Model
        {
            get { return _Model; }
            set
            {
                if(value != _Model && value != null)
                {
                    _Model = value;
                    this.Transform.Dimensions = new Vector3(value.Width, value.Height, 1);
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

        public string Name { get; set; }

        public GameObject()
        {
            this.Scripts              = new List<Script>();
            this.Transform            = new Transform(this);
            this.Transform.Dimensions = new Vector3(1, 1, 1);
            this.Transform.Scale      = new Vector3(1, 1, 1);
            this.Transform.Position   = new Vector3(0, 0, 0);
            this.Animator             = new Animator(this);
            
            if (Engine.Scene != null)
            {
                Engine.Scene.Model.Add(this);
                Engine.Scene.NewlyGenerated.Add(this);
            }
        }

        public GameObject(GameObject Parent)
        {
            this.Scripts = new List<Script>();
            this.Transform            = new Transform(this);
            this.Transform.Dimensions = new Vector3(1, 1, 1);
            this.Transform.Scale      = new Vector3(1, 1, 1);
            this.Transform.Position   = new Vector3(0, 0, 0);
            this.Animator             = new Animator(this);
            
            if (Parent != null)
            {
                this.Parent = Parent;
                Parent.Child.Add(this);

                if (Engine.Scene != null)
                    Engine.Scene.NewlyGenerated.Add(this);
            }
                
            else if (Engine.Scene != null)
                Engine.Scene.Model.Add(this);
        }

        internal void Start()
        {
            int ScriptsCount = this.Scripts.Count;
            for (int i = 0; i < ScriptsCount; i++)
            {
                Scripts[i].Start();
                Engine.UpdateEvent += Scripts[i].UpdateDel;
            }
        }

        public virtual void Destroy()
        {
            Engine.ToRender.Remove(this);

            int ScriptsCount = this.Scripts.Count;
            for (int i = 0; i < ScriptsCount; i++)
                Scripts[i].Destroy();

            int ChildCount = this.Child.Count;
            for (int i = 0; i < ChildCount; i++)
                Child[i].Destroy();

            this.Animator = null;
            this.Collider = null;
            this.Parent = null;
        }

        internal virtual void Render()
        { Model?.Render(this); }
    }
}
