/*
* (C) 2017 David Knieradl 
*/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace DKBasicEngine_1_0.Core.UI
{
    internal sealed class Letter : GameObject
    {
        private Letter()
        { }

        public Letter(TextBlock Parent)
            :base(Parent)
        { }

        public override void Destroy()
        {
            //Engine.CurrentScene.AllGameObjects.Remove(this);
            //if (Engine.ToRender.Contains(this))
                Engine.ToRender.Remove(this);

            Parent.Child.Remove(this);
            ((TextBlock)Parent)._text.Remove(this);

            Animator.Destroy();

            Parent = null;
            Animator = null;
            Model = null;
        }
    }
}
