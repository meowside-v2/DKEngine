using DKBasicEngine_1_0.Core.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
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

                return (this.Transform.Position.X + this.Transform._ScaledDimensions.X >= X && this.Transform.Position.X < X + Engine.Render.RenderWidth && this.Transform.Position.Y + this.Transform._ScaledDimensions.Y >= Y && this.Transform.Position.Y < Y + Engine.Render.RenderHeight);
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

                    if(Animator?.Animations.Count == 0)
                    {
                        Animator.Animations.Add("default", new AnimationNode("default", _Model));
                        Animator.Play("default");
                    }
                }
            }
        }

        protected bool _IsGUI = false;
        protected string _typeName = "";

        public Material _Model         = null;
        public GameObject Parent       = null;
        public Animator Animator       = null;
        public Collider Collider       = null;
        public SoundSource SoundSource = null;
        public Color? Foreground       = null;

        public readonly Transform Transform;
        public readonly List<GameObject> Child;
        internal readonly List<Script> Scripts;

        protected bool _IsPartOfScene = true; 
        internal bool IsPartOfScene
        {
            get { return _IsPartOfScene; }
            set
            {
                _IsPartOfScene = value;
                switch (_IsPartOfScene)
                {
                    case true:
                        if(Engine.LoadingScene != null)
                            if(!Engine.LoadingScene.AllGameObjects.Contains(this))
                                Engine.LoadingScene.AllGameObjects.Add(this);
                        break;
                    case false:
                        if (Engine.LoadingScene != null)
                            if (Engine.LoadingScene.AllGameObjects.Contains(this))
                                Engine.LoadingScene.AllGameObjects.Remove(this);
                        break;
                    default:
                        break;
                }
            }
        }

        public GameObject()
        {
            this.Child                = new List<GameObject>();
            this.Scripts              = new List<Script>();
            this.Transform            = new Transform(this);
            this.Transform.Dimensions = new Vector3(1, 1, 1);
            this.Transform.Scale      = new Vector3(1, 1, 1);
            this.Transform.Position   = new Vector3(0, 0, 0);
            //this.Collider             = new Collider(this);
            //this.Animator             = new Animator(this);

            if (_IsPartOfScene && Engine.LoadingScene != null)
            {
                Engine.LoadingScene.Model.Add(this);
                Engine.LoadingScene.NewlyGeneratedGameObjects.Add(this);
                Engine.LoadingScene.AllGameObjects.Add(this);
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
            //this.Collider = new Collider(this);
            //this.Animator = new Animator(this);
            
            if (Parent != null)
            {
                this.Parent = Parent;

                Parent.Child.Add(this);
                this.Transform.Position = Parent.Transform.Position;
                this.Transform.Scale    = Parent.Transform.Scale;

                if (Engine.LoadingScene != null)
                {
                    Engine.LoadingScene.NewlyGeneratedGameObjects.Add(this);
                    Engine.LoadingScene.AllGameObjects.Add(this);
                }

                this.IsPartOfScene = Parent.IsPartOfScene;
            }
                
            else if (Engine.LoadingScene != null)
            {
                Engine.LoadingScene.Model.Add(this);
                Engine.LoadingScene.NewlyGeneratedGameObjects.Add(this);
                Engine.LoadingScene.AllGameObjects.Add(this);
            }
        }

        /*internal void Start()
        {
            int ScriptsCount = this.Scripts.Count;
            for (int i = 0; i < ScriptsCount; i++)
            {
                Scripts[i].Start();
                Engine.UpdateEvent += Scripts[i].Update;
            }
        }*/

        public void InitNewScript<T>() where T : Script
        {
            this.Scripts.Add((T)Activator.CreateInstance(typeof(T), this));
        }

        public void InitNewComponent<T>() where T : Component
        {
            //this.Scripts.Add((T)Activator.CreateInstance(typeof(T), this));
            if (typeof(T) == typeof(Animator))
            {
                this.Animator = new Animator(this);
                return;
            }

            if (typeof(T).IsSubclassOf(typeof(Script)))
            {
                this.Scripts.Add((Script)Activator.CreateInstance(typeof(T), (GameObject)this));
                return;
            }

            if (typeof(T) == typeof(Collider) || typeof(T).IsSubclassOf(typeof(Collider)))
            {
                //ConstructorInfo ctor = typeof(T).
                Type t = typeof(T);
                this.Collider = (Collider)t.Assembly.CreateInstance(t.FullName, false, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public, null, new object[] { this }, null, null);
                //(typeof(T).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, Type.EmptyTypes, null).Invoke(new object[] { this }));
                //Activator.CreateInstance(typeof(T), (GameObject)this);
                return;
            }

            if (typeof(T) == typeof(SoundSource) || typeof(T).IsSubclassOf(typeof(SoundSource)))
            {
                this.SoundSource = (SoundSource)Activator.CreateInstance(typeof(T), (GameObject)this);
                return;
            }
        }

        public virtual void Destroy()
        {
            if (Engine.LoadingScene.NewlyGeneratedGameObjects.Contains(this))
                Engine.LoadingScene.NewlyGeneratedGameObjects.Remove(this);
            Engine.LoadingScene.AllGameObjects.Remove(this);
            Engine.RenderGameObjects.Remove(this);

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
        { Model?.Render(this, Foreground); }
    }
}
