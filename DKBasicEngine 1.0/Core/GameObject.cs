using DKBasicEngine_1_0.Core.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;

namespace DKBasicEngine_1_0.Core
{
    public class GameObject : ICore, IGraphics
    {
        public string Name { get; set; }
        public bool HasShadow { get; set; }
        public bool IsInView
        {
            get
            {
                float X = this.IsGUI ? 0 : Engine.BaseCam != null ? Engine.BaseCam.X : 0;
                float Y = this.IsGUI ? 0 : Engine.BaseCam != null ? Engine.BaseCam.Y : 0;

                return (this.Transform.Position.X + this.Transform.Dimensions.X >= X && this.Transform.Position.X < X + Engine.Render.RenderWidth && this.Transform.Position.Y + this.Transform.Dimensions.Y >= Y && this.Transform.Position.Y < Y + Engine.Render.RenderHeight);
            }
        }
        public bool IsGUI
        {
            get { return Parent != null ? Parent.IsGUI : _IsGUI; }
            set { _IsGUI = value; }
        }
        public string TypeName
        {
            get { return _typeName; }
            set
            {
                _typeName = value;
                this.Model = Database.GetGameObjectMaterial(value);
            }
        }
        public Material Model
        {
            get { return _Model; }
            set
            {
                if (value != _Model && value != null)
                {
                    _Model = value;
                    this.Transform.Dimensions = new Vector3(value.Width, value.Height, 1);

                    if(Animator.Animations.Count == 0)
                    {
                        Animator.Animations.Add("default", new AnimationNode("default", _Model));
                        Animator.Play("default");
                    }
                }
            }
        }

        protected bool _IsGUI = false;
        protected string _typeName = "";

        protected Material _Model = null;
        public GameObject Parent  = null;
        public Animator Animator  = null;
        public Collider Collider  = null;
        public Color? Foreground  = null;

        public readonly Transform Transform;
        public readonly List<Script> Scripts;
        public readonly List<GameObject> Child;
        
        public GameObject()
        {
            this.Child                = new List<GameObject>();
            this.Scripts              = new List<Script>();
            this.Transform            = new Transform(this);
            this.Transform.Dimensions = new Vector3(1, 1, 1);
            this.Transform.Scale      = new Vector3(1, 1, 1);
            this.Transform.Position   = new Vector3(0, 0, 0);
            this.Animator             = new Animator(this);
            this.Collider             = new Collider(this);

            if (Engine.CurrentScene != null)
            {
                Engine.CurrentScene.Model.Add(this);
                Engine.CurrentScene.NewlyGenerated.Add(this);
                //Engine.CurrentScene.AllGameObjects.Add(this);
            }
        }

        public GameObject(GameObject Parent)
        {
            this.Child = new List<GameObject>();
            this.Scripts = new List<Script>();
            this.Transform = new Transform(this);
            this.Transform.Dimensions = new Vector3(1, 1, 1);
            this.Transform.Scale = new Vector3(1, 1, 1);
            this.Transform.Position = new Vector3(0, 0, 0);
            this.Animator = new Animator(this);
            this.Collider = new Collider(this);

            if (Parent != null)
            {
                this.Parent = Parent;
                Parent.Child.Add(this);

                this.Transform.Position = Parent.Transform.Position;
                this.Transform.Scale    = Parent.Transform.Scale;

                if (Engine.CurrentScene != null)
                {
                    Engine.CurrentScene.NewlyGenerated.Add(this);
                    //Engine.CurrentScene.AllGameObjects.Add(this);
                }
            }
                
            else if (Engine.CurrentScene != null)
            {
                Engine.CurrentScene.Model.Add(this);
                Engine.CurrentScene.NewlyGenerated.Add(this);
                //Engine.CurrentScene.AllGameObjects.Add(this);
            }
                
        }

        internal void Start()
        {
            int ScriptsCount = this.Scripts.Count;
            for (int i = 0; i < ScriptsCount; i++)
            {
                Scripts[i].Start();
                Engine.UpdateEvent += Scripts[i].Update;
            }
        }

        public virtual void Destroy()
        {
            if (Engine.CurrentScene.NewlyGenerated.Contains(this))
                Engine.CurrentScene.NewlyGenerated.Remove(this);
            //Engine.CurrentScene.AllGameObjects.Remove(this);
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

        internal void Render()
        { Model?.Render(this, Foreground); }
    }
}
