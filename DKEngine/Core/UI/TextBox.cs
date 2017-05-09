/*
* (C) 2017 David Knieradl
*/

using DKEngine.Core.Scripts;
using System;
using System.Linq;
using static DKEngine.Core.UI.Text;

namespace DKEngine.Core.UI
{
    public class TextBox : TextBlock, IControl
    {
        public bool IsFocused { get; set; }
        public int FocusElementID { get; private set; }

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

        public InputType AllowedChars { get; set; }

        public TextBox()
        {
            this.InitNewScript<TextBoxScript>();
        }

        public TextBox(GameObject Parent)
            : base(Parent)
        {
            this.InitNewScript<TextBoxScript>();
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
    }
}