/*
* (C) 2017 David Knieradl 
*/

using System;
using System.Diagnostics;
using System.Linq;

namespace DKBasicEngine_1_0
{
    public class TextBox : TextBlock, IControl
    {
        public bool IsFocused { get; set; }
        public int FocusElementID { get; private set; }

        public TextBox()
        {
            this.Scripts.Add(new TextBoxScript(this));
            /*FocusElementID = Engine.Scene.PageControls.Count;
            Engine.Scene.PageControls.Add(this);*/
        }

        public TextBox(GameObject Parent)
            :base(Parent)
        {
            this.Scripts.Add(new TextBoxScript(this));
            /*FocusElementID = Engine.Scene.PageControls.Count;
            Engine.Scene.PageControls.Add(this);*/
        }

        public Type AllowedChars { get; set; }

        public enum Type
        {
            All,
            AlphaNumerical,
            Alpha,
            Numerical
        };

        public override string Text
        {
            set
            {
                if (TextControl(value))
                {
                    _textStr = value;
                    _changed = true;
                }
                
            }

            get
            {
                return _textStr;
            }
        }

        private bool TextControl(string key)
        {
            if (AllowedChars == Type.All)
                return true;

            else if (AllowedChars == Type.Alpha)
                return key.All(Char.IsLetter);

            else if (AllowedChars == Type.Numerical)
                return key.All(Char.IsNumber);

            else if (AllowedChars == Type.AlphaNumerical)
                return key.All(Char.IsLetterOrDigit);

            return true;
        }

        private bool TextControl(char key)
        {
            if (AllowedChars == Type.All)
                return true;

            else if (AllowedChars == Type.Alpha)
                return Char.IsLetter(key);

            else if (AllowedChars == Type.Numerical)
                return Char.IsNumber(key);

            else if (AllowedChars == Type.AlphaNumerical)
                return Char.IsLetterOrDigit(key);

            return true;
        }
    }
}
