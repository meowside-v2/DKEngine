﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKBasicEngine_1_0
{
    public class Button : TextBlock, IControl
    {
        public Action OnClick { get; set; }
        public bool IsFocused { get; set; }
        private bool IsHeld { get; set; }

        public Button(Scene ParentPage)
            : base(ParentPage)
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