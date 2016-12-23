using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace DKBasicEngine_1_0
{
    public class Letter : ICore, I3Dimensional, IGraphics
    {
        public I3Dimensional Parent { get { return _Parent; } }
        public readonly TextBlock _Parent;
        internal float _x { get; private set; } = 0;
        internal float _y { get; private set; } = 0;

        internal float VertOffset = 0;
        internal float HorOffset = 0;

        public bool HasShadow
        {
            get { return _Parent.HasShadow; }
            set { }
        }

        public bool _IsVisble { get; set; }
        
        public float X
        {
            get { return HorOffset + _x * _Parent.FontSize * _Parent.ScaleX + _Parent.X; }
            set { _x = value; }
        }
        public float Y
        {
            get { return VertOffset + _y * _Parent.FontSize * _Parent.ScaleY + _Parent.Y; }
            set { _y = value; }
        }
        public float Z { get; set; }

        public float width
        {
            get { return Model.Width * _Parent.FontSize * _Parent.ScaleX; }
            set { }
        }

        public float height
        {
            get { return Model.Height * _Parent.FontSize * _Parent.ScaleX; }
            set { }
        }

        public float depth
        {
            get { return 0; }
            set { }
        }

        public float ScaleX
        {
            get { return _Parent.FontSize * _Parent.ScaleX; }
            set { }
        }

        public float ScaleY
        {
            get { return _Parent.FontSize * _Parent.ScaleY; }
            set { }
        }

        public float ScaleZ
        {
            get { return _Parent.FontSize * _Parent.ScaleZ; }
            set { }
        }

        public bool LockScaleRatio { get; set; } = true;

        public Animator Animator { get; private set; }
        public Material Model { get; private set; }
        

        public Color? Foreground { get { return _Parent.Foreground; } }
        

        public bool IsGUI
        {
            get { return _Parent.IsGUI; }
            set { }
        }

        public Letter(TextBlock Parent, float x, float y, float z, Material sourceModel)
        {
            Model = sourceModel;

            this.X = x;
            this.Y = y;
            this.Z = z;

            this._Parent = Parent;

            Animator = new Animator(this);

            lock (Engine.ToStart)
            {
                lock (Engine.ToRender)
                {
                    Engine.ToStart.Add(this);
                    Engine.ToRender.Add(this);
                }
            }
        }

        public void Start() { }

        public void Update() { }

        public void Destroy()
        {
            Engine.ToRender.Remove(this);
            Animator = null;
            Model = null;
        }

        public void Render()
        {
            Model?.Render(this, Foreground);
        }
    }
}
