/*
* (C) 2017 David Knieradl 
*/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace DKEngine.Core.UI
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
            if (Engine.LoadingScene != null)
            {
                if(Engine.LoadingScene.NewlyGeneratedGameObjects.Contains(this))
                    Engine.LoadingScene.NewlyGeneratedGameObjects.Remove(this);

                Engine.LoadingScene.AllComponents.Remove(this.Name);
            }
            
            if(Engine.RenderGameObjects.Contains(this))
                Engine.RenderGameObjects.Remove(this);

            /*Engine.LoadingScene.AllGameObjects.Remove(this);
            //if (Engine.ToRender.Contains(this))
                Engine.RenderGameObjects.Remove(this);*/

            Parent.Child.Remove(this);
            //((TextBlock)Parent)._text.Remove(this);

            Animator?.Destroy();

            Parent = null;
            Animator = null;
            Model = null;
        }

        protected override void Init()
        {
            IsPartOfScene = false;
        }
    }
}
