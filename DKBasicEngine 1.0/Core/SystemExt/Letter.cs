/*
* (C) 2017 David Knieradl 
*/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace DKBasicEngine_1_0
{
    internal sealed class Letter : GameObject
    {
        /*internal float VertOffset = 0;
        internal float HorOffset = 0;*/

        public override sealed bool HasShadow { get { return ((TextBlock)Parent).TextShadow; } }
        
        /*internal override float X { get { return HorOffset + Position.X * _Parent.FontSize * Parent.ScaleX + Parent.X; } }
        internal override float Y { get { return VertOffset + Position.Y * _Parent.FontSize * Parent.ScaleY + Parent.Y; } }
        internal override float Z { get { return Parent.Z + Position.Z; } }

        internal override float Width { get { return Model.Width * _Parent.FontSize * Parent.ScaleX; } }
        internal override float Height { get { return Model.Height * _Parent.FontSize * Parent.ScaleX; } }
        internal override float Depth { get { return 0; } }

        internal override float ScaleX { get { return _Parent.FontSize * Parent.ScaleX; } }
        internal override float ScaleY { get { return _Parent.FontSize * Parent.ScaleY; } }
        internal override float ScaleZ { get { return _Parent.FontSize * Parent.ScaleZ; } }*/
        
        public Color? Foreground { get { return ((TextBlock)Parent).Foreground; } }
        
        public Letter(TextBlock Parent, Material sourceModel)
            :base(Parent)
        {
            Model = sourceModel;
            this.Parent = Parent;
            
            Animator = new Animator(this);
        }

        public override sealed void Destroy()
        {
            Engine.ToRender.Remove(this);
            Engine.UpdateEvent -= this.UpdateHandler;
            Parent.Child.Remove(this);
            ((TextBlock)Parent)._text.Remove(this);

            Animator.Destroy();

            Parent = null;
            Animator = null;
            Model = null;
        }

        internal override sealed void Render()
        { Model?.Render(this, Foreground); }
    }
}
