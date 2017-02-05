/*
* (C) 2017 David Knieradl 
*/

using DKBasicEngine_1_0.Core.Scripts;
using System;

namespace DKBasicEngine_1_0.Core.UI
{
    public class Button : TextBlock, IControl
    {
        public Action OnClick = null;
        public bool IsFocused { get; set; } = false;
        public int FocusElementID { get; private set; }

        public Button()
        {
            this.Scripts.Add(new ButtonScript(this));
            //Engine.Scene.PageControls.Add(this);
        }

        public Button(GameObject Parent)
            : base(Parent)
        {
            this.Scripts.Add(new ButtonScript(this));
            /*FocusElementID = Engine.Scene.PageControls.Count;
            Engine.Scene.PageControls.Add(this);*/
        }
    }
}
