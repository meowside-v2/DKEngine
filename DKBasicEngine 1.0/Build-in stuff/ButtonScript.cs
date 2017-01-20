using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKBasicEngine_1_0
{
    internal sealed class ButtonScript : Script
    {
        Button _Parent;
        private bool IsHeld = false;

        public ButtonScript(Button Parent)
            : base(Parent)
        {
            _Parent = Parent;
        }

        public override void Update()
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
    }
}
