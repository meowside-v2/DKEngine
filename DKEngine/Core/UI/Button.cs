/*
* (C) 2017 David Knieradl
*/

using DKEngine.Core.Scripts;
using System;

namespace DKEngine.Core.UI
{
    public class Button : TextBlock, IControl
    {
        public Action OnClick = null;
        public bool IsFocused { get; set; } = false;
        public int FocusElementID { get; private set; }

        public Button()
        {
            this.InitNewScript<ButtonScript>();
        }

        public Button(GameObject Parent)
            : base(Parent)
        {
            this.InitNewScript<ButtonScript>();
        }
    }
}