/*
* (C) 2017 David Knieradl 
*/

using DKBasicEngine_1_0.Core.Scripts;
using System;
using System.Diagnostics;
using System.Linq;
using static DKBasicEngine_1_0.Core.UI.Text;

namespace DKBasicEngine_1_0.Core.UI
{
    public class TextBox : TextBlock, IControl
    {
        public bool IsFocused { get; set; }
        public int FocusElementID { get; private set; }

        public TextBox()
        {
            this.Scripts.Add(new TextBoxScript(this));
        }

        public TextBox(GameObject Parent)
            :base(Parent)
        {
            this.Scripts.Add(new TextBoxScript(this));
        }

        public InputType AllowedChars { get; set; }
        
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
            get { return _textStr; }
        }

        private bool TextControl(string key)
        {
            switch (AllowedChars)
            {
                case InputType.All:
                    return true;
                case InputType.AlphaNumerical:
                    return key.All(Char.IsLetterOrDigit);
                case InputType.Alpha:
                    return key.All(Char.IsLetter);
                case InputType.Numerical:
                    return key.All(Char.IsNumber);
                default:
                    return false;
            }
        }

        /*private bool TextControl(char key)
        {
            switch (AllowedChars)
            {
                case Type.All:
                    return true;
                case Type.AlphaNumerical:
                    return Char.IsLetterOrDigit(key);
                case Type.Alpha:
                    return Char.IsLetter(key);
                case Type.Numerical:
                    return Char.IsNumber(key);
                default:
                    return false;
            }
        }*/
    }
}
