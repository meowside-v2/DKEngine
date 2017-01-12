﻿using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace DKBasicEngine_1_0
{
    public class GameObject : Transform, ICore, IGraphics
    {
        public GameObject Parent = null;

        internal override float X { get { return Parent != null ? Position.X + Parent.X : Position.X; } }
        internal override float Y { get { return Parent != null ? Position.Y + Parent.Y : Position.Y; } }
        internal override float Z { get { return Parent != null ? Position.Z + Parent.Z : Position.Z; } }

        internal override float ScaleX { get { return Parent != null ? Scale.X * Parent.Scale.X : Scale.X; } }
        internal override float ScaleY { get { return Parent != null ? Scale.Y * Parent.Scale.Y : Scale.Y; } }
        internal override float ScaleZ { get { return Parent != null ? Scale.Z * Parent.Scale.Z : Scale.Z; } }

        public Animator Animator;
        public Material Model;
        public Collider Collider;

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

        public GameObject()
        {
            this.Dimensions = new Dimensions(1, 1, 1);
            this.Scale = new Scale(1, 1, 1);
            this.Position = new Position(0, 0, 0);
            this.Animator = new Animator(this);

            lock (Engine.ToStart)
                    lock (Engine.ToRender)
                    {
                        Engine.ToRender.Add(this);
                        Engine.ToStart.Add(this);
                    }

            if (Engine.Scene != null)
                Engine.Scene.Model.Add(this);
        }

        public GameObject(GameObject Parent)
        {
            this.Dimensions = new Dimensions(0, 0, 0);
            this.Scale = new Scale(1, 1, 1);
            this.Position = new Position(0, 0, 0);
            this.Animator = new Animator(this);

            lock (Engine.ToStart)
                    lock (Engine.ToRender)
                    {
                        Engine.ToRender.Add(this);
                        Engine.ToStart.Add(this);
                    }

            if (Parent != null)
                this.Parent = Parent;
        }

        public virtual void Start()
        { }

        public virtual void Update()
        {
            Animator?.Update();
        }

        public virtual void Destroy()
        {
            Engine.ToUpdate.Remove(this);
            Engine.ToRender.Remove(this);

            this.Animator = null;
            this.Collider = null;
            this.Parent = null;
        }

        internal virtual void Render()
        {
            Model?.Render(this);
        }

        public virtual void OnColliderEnter(Collider e)
        { }
    }
}
