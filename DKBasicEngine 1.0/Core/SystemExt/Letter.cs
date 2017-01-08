using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace DKBasicEngine_1_0
{
    internal class Letter : GameObject
    {
        public new TextBlock Parent;

        internal float VertOffset = 0;
        internal float HorOffset = 0;

        public override bool HasShadow
        {
            get { return Parent.HasShadow; }
        }

        public bool _IsVisble { get; set; }

        internal override float X
        {
            get { return HorOffset + Transform.X * Parent.FontSize * Parent.ScaleX + Parent.X; }
        }
        internal override float Y
        {
            get { return VertOffset + Transform.Y * Parent.FontSize * Parent.ScaleY + Parent.Y; }
        }
        internal override float Z { get { return Parent.Z + Transform.Z; } }

        internal override float Width
        {
            get { return Model.Width * Parent.FontSize * Parent.ScaleX; }
        }

        internal override float Height
        {
            get { return Model.Height * Parent.FontSize * Parent.ScaleX; }
        }

        internal override float Depth
        {
            get { return 0; }
        }

        internal override float ScaleX
        {
            get { return Parent.FontSize * Parent.ScaleX; }
        }

        internal override float ScaleY
        {
            get { return Parent.FontSize * Parent.ScaleY; }
        }

        internal override float ScaleZ
        {
            get { return Parent.FontSize * Parent.ScaleZ; }
        }
        
        public Color? Foreground { get { return Parent.Foreground; } }
        

        public Letter(TextBlock Parent, Transform Transform, Material sourceModel)
        {
            Model = sourceModel;

            this.Transform = Transform;
            this.Parent = Parent;

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
            //Engine.UpdateEvent -= this._UpdateDel;
            Engine.ToRender.Remove(this);
            Engine.ToUpdate.Remove(this);
            Parent._text.Remove(this);

            Parent = null;
            Animator = null;
            Model = null;
        }

        internal override void Render()
        {
            Model?.Render(this, Foreground);
        }
    }
}
