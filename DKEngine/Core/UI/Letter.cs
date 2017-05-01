/*
* (C) 2017 David Knieradl
*/

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
                Engine.LoadingScene.AllComponents.Remove(this.Name);
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

            /*if(Engine.RenderGameObjects.Contains(this))
                Engine.RenderGameObjects.Remove(this);*/

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

        protected override void Initialize()
        { }
    }
}