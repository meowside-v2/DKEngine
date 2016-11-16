using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKBasicEngine_1_0
{
    public class TextBox : TextBlock
    {
        public TextBox()
            : base()
        { }
        public TextBox(int X, int Y, int Z)
            : base(X, Y, Z)
        { }
        public TextBox(int X, int Y, int Z, HorizontalAlignment HAlignment, VerticalAlignment VAlignment, string Text)
            : base(X, Y, Z, HAlignment, VAlignment, Text)
        { }
        public TextBox(int X, int Y, string Layer)
            : base(X, Y, Layer)
        { }
        public TextBox(int X, int Y, string Layer, HorizontalAlignment HAlignment, VerticalAlignment VAlignment, string Text)
            : base(X, Y, Layer, HAlignment, VAlignment, Text)
        { }

        private double _textXOffset = 0;

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
                    TextRasterize(value);
                }
            }

            get
            {
                return _stringText;
            }
        }

        public void RemoveLastLetter()
        {
            if (this.Text.Length > 0)
            {
                this.Text = Text.Remove(Text.Length - 1, 1);
            }
        }

        protected override void TextRasterize(string txt)
        {
            if (txt.Length > _stringText.Length)
            {
                string temp = txt.Remove(0, _stringText.Length);

                foreach (char letter in temp)
                {
                    if (letter == ' ')
                    {
                        _textXOffset += 3;
                    }

                    else
                    {
                        _text.Add(new Letter(this,
                                            _textXOffset,
                                            0,
                                            0,
                                            Database.letterMaterial[(int)Database.font[Char.ToUpper(letter)]]));

                        _textXOffset += Database.letterMaterial[(int)Database.font[Char.ToUpper(letter)]].width + 1;
                    }
                }
            }

            else if (txt.Length < _stringText.Length)
            {
                int numberOfLettersToRemove = _stringText.Length - txt.Length;

                for (int i = 0; i < numberOfLettersToRemove; i++)
                {
                    if (_stringText[_stringText.Length - 1] == ' ')
                    {
                        _textXOffset -= 3;
                    }

                    else
                    {
                        _textXOffset -= _text[_text.Count - 1].width + 1;
                        _text.Remove(_text[_text.Count - 1]);
                    }
                }
            }

            VAlignment = _VA;
            HAlignment = _HA;

            _stringText = txt;
        }

        public bool TextControl(string key)
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
    }
}
