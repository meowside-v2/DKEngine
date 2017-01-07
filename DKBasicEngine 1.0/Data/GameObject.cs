using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace DKBasicEngine_1_0
{
    public class GameObject : EmptyGameObject, IGraphics
    {
        public Animator Animator { get; internal set; }
        public Material Model { get; internal set; }

        private bool _IsGUI = false;
        public bool IsGUI
        {
            get { return Parent != null ? ((IGraphics)Parent).IsGUI : _IsGUI; }
            set { _IsGUI = value; }
        }
        public virtual bool HasShadow { get; set; }

        public Collider collider;

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
        
        internal GameObject()
            :base()
        {
            Animator = new Animator(this);
            
            lock (Engine.ToRender)
                Engine.ToRender.Add(this);
        }

        public GameObject(Scene ToAddToModel)
            : base(ToAddToModel)
        {
            Animator = new Animator(this);

            lock (Engine.ToRender)
                Engine.ToRender.Add(this);
        }

        public GameObject(EmptyGameObject Parent)
            : base(Parent)
        {
            Animator = new Animator(this);

            lock (Engine.ToRender)
                Engine.ToRender.Add(this);
        }
        
        public override void Update()
        {
            Animator?.Update();
        }

        public override void Destroy()
        {
            Engine.ToUpdate.Remove(this);
            Engine.ToRender.Remove(this);

            this.Animator = null;
            this.collider = null;
            this.Parent = null;
        }

        internal virtual void Render()
        {
            Model?.Render(this);
        }
    }
}
