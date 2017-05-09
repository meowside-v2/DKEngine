/*
* (C) 2017 David Knieradl
*/

using System;

namespace DKEngine.Core.UI
{
    internal sealed class Letter : GameObject
    {
        private Letter()
        { }

        public Letter(TextBlock Parent)
            : base(Parent)
        { }

        public override void Destroy()
        {
            try
            {
                
                if (Engine.LoadingScene.NewlyGeneratedComponents.Contains(this))
                {
                    Engine.LoadingScene.NewlyGeneratedComponents.Pop();
                }
            }
            catch
            { }

            try
            {
                Engine.RenderObjects.Remove(this);
            }
            catch { }
            
            Parent?.Child.Remove(this);

            Animator?.Destroy();

            Parent = null;
            Animator = null;
            Model = null;
        }

        protected override void Initialize()
        { }
    }
}