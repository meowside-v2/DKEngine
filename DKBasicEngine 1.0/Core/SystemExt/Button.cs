using System;

namespace DKBasicEngine_1_0
{
    public class Button : TextBlock, IControl
    {
        public Action OnClick = null;
        private bool IsHeld = false;
        public bool IsFocused { get; set; } = false;
        
        public Button(Scene ParentPage, I3Dimensional Parent)
            : base(ParentPage, Parent)
        {
            ParentPage.PageControls.Add(this);
        }

        public override void Update()
        {
            base.Update();

            if (Engine.Input.IsKeyPressed(ConsoleKey.Enter))
            {
                if (IsFocused && !IsHeld)
                {
                    OnClick?.Invoke();
                    IsHeld = true;
                }
            }

            else if (IsHeld)
            {
                IsHeld = false;
            }
        }
    }
}
