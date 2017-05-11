using DKEngine.Core.Components;
using DKEngine.Core.Ext;
using DKEngine.Core.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;

namespace DKEngine.Core
{
    /// <summary>
    /// Primitive type for all renderable objects
    /// </summary>
    /// <seealso cref="DKEngine.Core.Components.Component" />
    public class GameObject : Component
    {

        /// <summary>
        /// The GameObject has shadow
        /// </summary>
        public bool HasShadow = false;

        /// <summary>
        /// Gets a value indicating whether this instance is in view.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is in view; otherwise, <c>false</c>.
        /// </value>
        public bool IsInView
        {
            get
            {
                float X = this.IsGUI ? 0 : Engine.BaseCam != null ? Engine.BaseCam.X : 0;
                float Y = this.IsGUI ? 0 : Engine.BaseCam != null ? Engine.BaseCam.Y : 0;

                return (this.Transform.Position.X + this.Transform._ScaledDimensions.X >= X && this.Transform.Position.X < X + Engine.Render.RenderWidth && this.Transform.Position.Y + this.Transform._ScaledDimensions.Y >= Y && this.Transform.Position.Y < Y + Engine.Render.RenderHeight);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is GUI.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is GUI; otherwise, <c>false</c>.
        /// </value>
        public bool IsGUI
        {
            get { return Parent != null ? Parent.IsGUI : _IsGUI; }
            set { _IsGUI = value; }
        }

        /// <summary>
        /// Gets or sets the name of the type.
        /// </summary>
        /// <value>
        /// The name of the type.
        /// </value>
        public string TypeName
        {
            get { return _typeName; }
            set
            {
                _typeName = value;
                this.Model = Database.GetGameObjectMaterial(value);
            }
        }

        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        /// <value>
        /// The model.
        /// </value>
        public Material Model
        {
            get { return _Model; }
            set
            {
                if (value != _Model && value != null)
                {
                    _Model = value;
                    this.Transform.Dimensions = new Vector3(value.Width, value.Height, 1);

                    if (Animator?.Animations.Count == 0)
                    {
                        Animator.AddAnimation("default", _Model);
                        Animator.Play("default");
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the collider.
        /// </summary>
        /// <value>
        /// The collider.
        /// </value>
        public Collider Collider
        {
            get { return _collider; }
            set
            {
                if (_collider != value)
                {
                    if (_collider != null)
                    {
                        foreach (Script scr in this.Scripts)
                        {
                            _collider.CollisionEvent -= scr.CollisionHandler;
                            scr.CollisionHandler = null;
                        }
                    }

                    if (value != null)
                    {
                        foreach (Script scr in this.Scripts)
                        {
                            scr.CollisionHandler = new Collider.CollisionEnterHandler(scr.OnColliderEnter);
                            value.CollisionEvent += scr.CollisionHandler;
                        }
                    }

                    _collider = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the animator.
        /// </summary>
        /// <value>
        /// The animator.
        /// </value>
        public Animator Animator { get; set; }

        /// <summary>
        /// Gets or sets the sound source.
        /// </summary>
        /// <value>
        /// The sound source.
        /// </value>
        public SoundSource SoundSource { get; set; }

        /// <summary>
        /// Gets or sets the foreground.
        /// </summary>
        /// <value>
        /// The foreground.
        /// </value>
        public Color? Foreground { get; set; }

        /// <summary>
        /// Gets the transform.
        /// </summary>
        /// <value>
        /// The transform.
        /// </value>
        public Transform Transform { get; }

        /// <summary>
        /// Gets the list of childs.
        /// </summary>
        /// <value>
        /// The child.
        /// </value>
        public List<GameObject> Child { get; }
        
        internal List<Script> Scripts { get; }
        internal bool _IsGUI = false;
        internal string _typeName = "";
        internal Material _Model = null;
        internal Collider _collider = null;

        public GameObject()
            : base(null)
        {
            this.Child = new List<GameObject>();
            this.Scripts = new List<Script>();
            this.Transform = new Transform(this)
            {
                Dimensions = new Vector3(1, 1, 1),
                Scale = new Vector3(1, 1, 1),
                Position = new Vector3(0, 0, 0)
            };
        }

        public GameObject(GameObject Parent)
            : base(Parent)
        {
            this.Child = new List<GameObject>();
            this.Scripts = new List<Script>();
            this.Transform = new Transform(this)
            {
                Dimensions = new Vector3(1, 1, 1),
                Scale = new Vector3(1, 1, 1),
                Position = new Vector3(0, 0, 0)
            };

            if (Parent != null)
            {
                this.Parent = Parent;

                Parent.Child.Add(this);
                this.Transform.Position = Parent.Transform.Position;
                this.Transform.Scale = Parent.Transform.Scale;
            }
        }

        internal override void Init()
        {
            Initialize();

            try
            {
                if (Parent == null)
                    Engine.LoadingScene.Model.Add(this);

                Engine.LoadingScene.GameObjectsToAddToRender.Push(this);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Loading scene is NULL\n\n{0}", e);
            }
        }

        protected virtual void Initialize()
        { }

        /// <summary>
        /// Initializes the new script.
        /// </summary>
        /// <typeparam name="T">Scirpt</typeparam>
        public void InitNewScript<T>() where T : Script
        {
            this.Scripts.Add((T)Activator.CreateInstance(typeof(T), this));
        }

        /// <summary>
        /// Initializes the new component.
        /// </summary>
        /// <typeparam name="T">Component</typeparam>
        public void InitNewComponent<T>() where T : Component
        {
            if (typeof(T) == typeof(Animator) || typeof(T).IsSubclassOf(typeof(Animator)))
            {
                if (this.Animator == null)
                {
                    this.Animator = new Animator(this);
                }

                return;
            }
            
            if (typeof(T) == typeof(Collider) || typeof(T).IsSubclassOf(typeof(Collider)))
            {
                if (this.Collider == null)
                {
                    Type t = typeof(T);
                    this.Collider = (Collider)t.Assembly.CreateInstance(t.FullName, false, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public, null, new object[] { this }, null, null);
                }

                return;
            }

            if (typeof(T) == typeof(SoundSource) || typeof(T).IsSubclassOf(typeof(SoundSource)))
            {
                if (this.SoundSource == null)
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
                if (Engine.LoadingScene.NewlyGeneratedComponents.Contains(this))
                {
                    Engine.LoadingScene.DestroyObjectAwaitList.Add(this);
                    return;
                }   
            }
            catch { }

            try
            {
                Engine.CurrentScene.AllComponents.Remove(this.Name);
            }
            catch { }

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

        /// <summary>
        /// Finds the specified GameObject of desired name.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="Name">Desired name</param>
        /// <returns></returns>
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

        /// <summary>
        /// Finds the specified GameObject of desired name.
        /// </summary>
        /// <param name="Name">Desired name</param>
        /// <returns></returns>
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

        /// <summary>
        /// Instantiates GameObject.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="Position">The position</param>
        /// <param name="Dimensions">The dimensions</param>
        /// <param name="Scale">The scale</param>
        /// <returns></returns>
        public static T Instantiate<T>(Vector3 Position, Vector3 Dimensions, Vector3 Scale)
            where T : GameObject, new()
        {
            T retValue = new T();

            retValue.Transform.Position = Position;
            retValue.Transform.Dimensions = Dimensions;
            retValue.Transform.Scale = Scale;

            return retValue;
        }

        /// <summary>
        /// Instantiates GameObject.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="Transform">The transform</param>
        /// <returns></returns>
        public static T Instantiate<T>(Transform @Transform)
            where T : GameObject, new()
        {
            return Instantiate<T>(@Transform.Position, @Transform.Dimensions, @Transform.Scale);
        }
    }
}