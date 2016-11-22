using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKBasicEngine_1_0
{
    public class TextBox : TextBlock, IControl
    {
        public bool IsFocused { get; set; }

        private int counter = 0;
        private const int TimeOut = 10;

        public TextBox(IPage ParentPage)
            : base(ParentPage)
        {
            ParentPage.PageControls.Add(this);
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
                }
            }

            get
            {
                return _textStr;
            }
        }

        public override void Update()
        {
            if (IsFocused)
            {

                if (Console.KeyAvailable)
                {

                    if (counter == 0)
                    {
                        char key = Console.ReadKey(true).KeyChar;

                        while (Console.KeyAvailable) Console.ReadKey();

                        if(key == '\b')
                        {
                            if (Text.Length > 0)
                            {
                                Text = Text.Remove(Text.Length - 1, 1);
                                _changed = true;
                            }
                        }

                        else if (TextControl(key))
                        {
                            Text += key;
                            _changed = true;
                        }
                    }

                    counter++;

                    if (counter > TimeOut) counter = 0;
                }

                else if (counter != 0)
                    counter = 0;
            }

            base.Update();
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
            if (key.IsEscapeSequence())
                return false;

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
