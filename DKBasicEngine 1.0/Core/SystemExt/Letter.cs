using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace DKBasicEngine_1_0
{
    internal class Letter : GameObject
    {
        public override EmptyGameObject Parent { get { return _Parent; } }

        public TextBlock _Parent;

        internal float VertOffset = 0;
        internal float HorOffset = 0;

        public override bool HasShadow
        {
            get { return _Parent.HasShadow; }
        }

        public bool _IsVisble { get; set; }

        internal override float X
        {
            get { return HorOffset + Transform.X * _Parent.FontSize * _Parent.ScaleX + _Parent.X; }
        }
        internal override float Y
        {
            get { return VertOffset + Transform.Y * _Parent.FontSize * _Parent.ScaleY + _Parent.Y; }
        }
        internal override float Z { get { return _Parent.Z + Transform.Z; } }

        internal override float width
        {
            get { return Model.Width * _Parent.FontSize * _Parent.ScaleX; }
        }

        internal override float height
        {
            get { return Model.Height * _Parent.FontSize * _Parent.ScaleX; }
        }

        internal override float depth
        {
            get { return 0; }
        }

        internal override float ScaleX
        {
            get { return _Parent.FontSize * _Parent.ScaleX; }
        }

        internal override float ScaleY
        {
            get { return _Parent.FontSize * _Parent.ScaleY; }
        }

        internal override float ScaleZ
        {
            get { return _Parent.FontSize * _Parent.ScaleZ; }
        }
        
        public Color? Foreground { get { return _Parent.Foreground; } }
        

        public Letter(TextBlock Parent, Transform Transform, Material sourceModel)
        {
            Model = sourceModel;

            this.Transform = Transform;
            this._Parent = Parent;

            Animator = new Animator(this);

            /*lock (Engine.ToStart)
            {
                lock (Engine.ToRender)
                {
                    Engine.ToStart.Add(this);
                    Engine.ToRender.Add(this);
                }
            }*/
        }

        public override void Destroy()
        {
            Engine.ToRender.Remove(this);
            Engine.ToUpdate.Remove(this);
            _Parent._text.Remove(this);

            _Parent = null;
            Animator = null;
            Model = null;
        }

        internal override void Render()
        {
            Model?.Render(this, Foreground);
        }
    }
}
