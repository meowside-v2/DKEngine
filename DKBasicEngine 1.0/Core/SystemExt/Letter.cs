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
        public override sealed bool HasShadow { get { return ((TextBlock)Parent).TextShadow; } }
        public Color? Foreground { get { return ((TextBlock)Parent).Foreground; } }
        
        private Letter() { }

        public Letter(TextBlock Parent, Material sourceModel)
            :base(Parent)
        {
            //this.Parent = Parent;
            Model = sourceModel;
            
            if (this.Parent == null)
                throw new Exception("wtf jak dohaje zase");
        }

        public override sealed void Destroy()
        {
            Engine.ToRender.Remove(this);
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
