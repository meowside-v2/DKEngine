using DKEngine.Core.Components;
using DKEngine.Core.Ext;
using DKEngine.Core.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DKEngine.Core
{
    public abstract class GameObject : Component
    {
        public bool HasShadow { get; set; } = false;
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
                        Animator.AddAnimation("default", _Model);
                        Animator.Play("default");
                    }
                }
            }
        }

        internal Collider _collider;
        public Collider Collider
        {
            get { return _collider; }
            set
            {
                if(_collider != value)
                {
                    _collider = value;
                    if (value != null)
                    {
                        foreach (Script scr in this.Scripts)
                        {
                            scr.CollisionHandler = new Collider.CollisionEnterHandler(scr.OnColliderEnter);
                            _collider.CollisionEvent += scr.CollisionHandler;
                        }
                    }
                    else
                    {
                        foreach (Script scr in this.Scripts)
                        {
                            _collider.CollisionEvent -= scr.CollisionHandler;
                            scr.CollisionHandler = null;
                            
                        }
                    }
                }
            }
        }


        internal bool _IsGUI = false;
        internal string _typeName = "";

        internal Material _Model       = null;
        public Animator Animator       = null;
        
        public SoundSource SoundSource = null;
        public Color? Foreground { get; set; }

        public Transform Transform { get; }
        public List<GameObject> Child { get; }
        internal List<Script> Scripts { get; }

        public GameObject()
            :base(null)
        {
            this.Child                = new List<GameObject>();
            this.Scripts              = new List<Script>();
            this.Transform = new Transform(this)
            {
                Dimensions = new Vector3(1, 1, 1),
                Scale = new Vector3(1, 1, 1),
                Position = new Vector3(0, 0, 0)
            };
            //this.Collider             = new Collider(this);
            //this.Animator             = new Animator(this);
            
        }

        public GameObject(GameObject Parent)
            :base(Parent)
        {
            this.Child = new List<GameObject>();
            this.Scripts = new List<Script>();
            this.Transform = new Transform(this)
            {
                Dimensions = new Vector3(1, 1, 1),
                Scale = new Vector3(1, 1, 1),
                Position = new Vector3(0, 0, 0)
            };
            //this.Collider = new Collider(this);
            //this.Animator = new Animator(this);

            if (Parent != null)
            {
                this.Parent = Parent;

                Parent.Child.Add(this);
                this.Transform.Position = Parent.Transform.Position;
                this.Transform.Scale    = Parent.Transform.Scale;
            }
        }

        internal override void Init()
        {
            Initialize();

            try
            {
                if (Parent == null)
                    Engine.LoadingScene.Model.Add(this);
                
                if(typeof(Letter) != this.GetType())
                {
                    Engine.LoadingScene.AllComponents.AddSafe(this);
                    //Engine.LoadingScene.AllGameObjects.AddSafe(this);
                }
                
                Engine.LoadingScene.GameObjectsToAddToRender.Push(this);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Loading scene is NULL\n\n{0}", e);
            }
        }

        protected abstract void Initialize();

        public void InitNewScript<T>() where T : Script
        {
            this.Scripts.Add((T)Activator.CreateInstance(typeof(T), this));
        }

        public void InitNewComponent<T>() where T : Component
        {
            if (typeof(T) == typeof(Animator) || typeof(T).IsSubclassOf(typeof(Animator)))
            {
                if(this.Animator == null)
                {
                    this.Animator = new Animator(this);
                }

                return;
            }

            /*if (typeof(T).IsSubclassOf(typeof(Script)))
            {
                this.Scripts.Add((Script)Activator.CreateInstance(typeof(T), (GameObject)this));
                return;
            }*/

            if (typeof(T) == typeof(Collider) || typeof(T).IsSubclassOf(typeof(Collider)))
            {
                if(this.Collider == null)
                {
                    Type t = typeof(T);
                    this.Collider = (Collider)t.Assembly.CreateInstance(t.FullName, false, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public, null, new object[] { this }, null, null);
                }

                return;
            }

            if (typeof(T) == typeof(SoundSource) || typeof(T).IsSubclassOf(typeof(SoundSource)))
            {
                if(this.SoundSource == null)
                {
                    Type t = typeof(T);
                    this.SoundSource = (SoundSource)t.Assembly.CreateInstance(t.FullName, false, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public, null, new object[] { this }, null, null);   
                }

                return;
            }
        }

        public override void Destroy()
        {
            try
            {
                Engine.CurrentScene.NewlyGeneratedComponents.Pop();
            }
            catch { }

            try
            {
                Engine.CurrentScene.AllComponents.Remove(this.Name);
            }
            catch { }

            /*try
            {
                Engine.CurrentScene.AllGameObjects.Remove(this.Name);
            }
            catch { }*/

            try
            {
                Engine.RenderObjects.Remove(this);
            }
            catch { }

            try
            {
                Engine.CurrentScene.Model.Remove(this);    
            }
            catch { }

            int ScriptsCount = this.Scripts.Count;
            for (int i = 0; i < ScriptsCount; i++)
                Scripts[i].Destroy();

            int ChildCount = this.Child.Count;
            for (int i = 0; i < ChildCount; i++)
                Child[i].Destroy();

            this.Animator?.Destroy();
            this.Animator = null;

            this.Collider?.Destroy();
            this.Collider = null;
            
            this.Parent = null;
        }

        internal virtual void Render()
        { Model?.Render(this, Foreground); }

        public static new T Find<T>(string Name) where T : GameObject
        {
            T retValue = null;

            try
            {
                retValue = (T)Engine.LoadingScene.AllComponents[Name];
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Object not found\n" + ex);
            }

            return retValue;
        }

        public static GameObject Find(string Name)
        {
            GameObject retValue = null;

            try
            {
                retValue = (GameObject)Engine.LoadingScene.AllComponents[Name];
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Object not found\n" + ex);
            }

            return retValue;
        }
    }
}
