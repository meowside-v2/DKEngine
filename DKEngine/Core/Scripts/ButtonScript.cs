using DKEngine.Core.Components;
using DKEngine.Core.UI;
using System;

namespace DKEngine.Core.Scripts
{
    internal sealed class ButtonScript : Script
    {
        private Button _Parent;
        private bool IsHeld = false;

        public ButtonScript(Button Parent)
            : base(Parent)
        {
            _Parent = Parent;
        }

        protected internal override void Update()
        {
            if (Engine.Input.IsKeyPressed(ConsoleKey.Enter))
            {
                if (_Parent.IsFocused && !IsHeld)
                {
                    _Parent.OnClick?.Invoke();
                    IsHeld = true;
                }
            }
            else if (IsHeld)
            {
                IsHeld = false;
            }
        }

        protected internal override void Start()
        { }

        protected internal override void OnColliderEnter(Collider e)
        { }
    }
}