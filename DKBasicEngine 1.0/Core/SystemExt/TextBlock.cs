using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKBasicEngine_1_0
{
    public class TextBlock : ICore, I3Dimensional
    {
        protected double _x = 0;
        protected double _y = 0;

        protected double vertOffset = 0;
        protected double horiOffset = 0;

        protected HorizontalAlignment _HA;
        protected VerticalAlignment _VA;

        protected string _stringText = "";
        protected List<Letter> _text = new List<Letter>();

        protected double _scaleX = 1;
        protected double _scaleY = 1;
        protected double _scaleZ = 1;

        public enum HorizontalAlignment
        {
            Left,
            Center,
            Right
        };

        public enum VerticalAlignment
        {
            Top,
            Center,
            Bottom
        };

        public enum Layer
        {
            Background = 0,
            GUI = 128
        };

        public double X
        {
            get
            {
                return _x + horiOffset;
            }

            set
            {
                _x = value;
            }
        }
        public double Y
        {
            get
            {
                return _y + vertOffset;
            }

            set
            {
                _y = value;
            }
        }
        public double Z { get; set; }

        public double width
        {
            get
            {
                return (_text.Count > 0 ? _text[_text.Count - 1].X + _text[_text.Count - 1].width - 1 : 0);
            }
        }

        public double height
        {
            get
            {
                return 5 * ScaleY;
            }
        }
        public double depth
        {
            get
            {
                return 0;
            }
        }

        public double ScaleX
        {
            get
            {
                return _scaleX;
            }
            set
            {
                if (value < 0.1f)
                    _scaleX = 0.1f;

                else
                    _scaleX = value;
            }
        }

        public double ScaleY
        {
            get
            {
                return _scaleY;
            }
            set
            {
                if (value < 0.1f)
                    _scaleY = 0.1f;

                else
                    _scaleY = value;
            }
        }

        public double ScaleZ
        {
            get
            {
                return _scaleZ;
            }
            set
            {
                if (value < 0.1f)
                    _scaleZ = 0.1f;

                else
                    _scaleZ = value;
            }
        }

        public bool HasShadow { get; set; }

        public virtual string Text
        {
            set
            {
                Task.Factory.StartNew(() => TextRasterize(value));
            }

            get
            {
                return _stringText;
            }
        }

        public HorizontalAlignment HAlignment
        {
            set
            {
                _HA = value;

                switch (value)
                {
                    case HorizontalAlignment.Left:
                        horiOffset = 0;
                        break;

                    case HorizontalAlignment.Center:
                        horiOffset = (Engine.Render.RenderWidth - this.width) / 2;
                        break;

                    case HorizontalAlignment.Right:
                        horiOffset = Engine.Render.RenderWidth - this.width;
                        break;

                    default:
                        break;
                }
            }
        }
        public VerticalAlignment VAlignment
        {
            set
            {
                _VA = value;

                switch (value)
                {
                    case VerticalAlignment.Top:
                        vertOffset = 0;
                        break;

                    case VerticalAlignment.Center:
                        vertOffset = (Engine.Render.RenderHeight - this.height) / 2;
                        break;

                    case VerticalAlignment.Bottom:
                        vertOffset = Engine.Render.RenderHeight - this.height;
                        break;

                    default:
                        break;
                }
            }
        }

        public TextBlock() { }
        public TextBlock(int X, int Y, int Z)
            : this()
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }
        public TextBlock(int X, int Y, int Z, HorizontalAlignment HAlignment, VerticalAlignment VAlignment, string Text)
            : this(X, Y, Z)
        {
            this.Text = Text;

            this.HAlignment = HAlignment;
            this.VAlignment = VAlignment;
        }
        public TextBlock(int X, int Y, string Layer)
            : this()
        {
            this.X = X;
            this.Y = Y;

            switch (Layer)
            {
                case "GUI":
                    this.Z = 100;
                    break;

                case "Background":
                    this.Z = 0;
                    break;
            }
        }
        public TextBlock(int X, int Y, string Layer, HorizontalAlignment HAlignment, VerticalAlignment VAlignment, string Text)
            : this(X, Y, Layer)
        {
            this.X = X;
            this.Y = Y;

            switch (Layer)
            {
                case "GUI":
                    this.Z = 100;
                    break;

                case "Background":
                    this.Z = 0;
                    break;
            }

            this.Text = Text;

            this.HAlignment = HAlignment;
            this.VAlignment = VAlignment;
        }

        protected virtual void TextRasterize(string txt)
        {
            _stringText = txt;

            List<Letter> retValue = new List<Letter>();

            int Xoffset = 0;

            foreach (char letter in txt)
            {
                if (letter == ' ')
                {
                    Xoffset += 3;
                }

                else
                {
                    retValue.Add(new Letter(this,
                                        Xoffset,
                                        0,
                                        0,
                                        Database.letterMaterial[(int)Database.font[Char.ToUpper(letter)]]));

                    Xoffset += Database.letterMaterial[(int)Database.font[Char.ToUpper(letter)]].width + 1;
                }
            }

            _text = retValue;

            VAlignment = _VA;
            HAlignment = _HA;
        }

        public object DeepCopy()
        {
            TextBlock retVal = (TextBlock)this.MemberwiseClone();

            retVal.Text = "";

            foreach (Letter letter in _text.ToList())
            {
                retVal._text.Add((Letter)letter.DeepCopy());
            }

            return retVal;
        }

        public void Render(int x, int y, byte[] imageBuffer, bool[] imageBufferKey)
        {


            foreach (Letter item in _text.FindAll(obj => Finder(obj, x, y)))
            {
                item.Render((int)(this.X - x), (int)(this.Y - y), imageBuffer, imageBufferKey);
                if (HasShadow) item.Render((int)(this.X - x + 1), (int)(this.Y - y + 1), imageBuffer, imageBufferKey, Color.Black);
            }
        }

        protected bool Finder(I3Dimensional obj, int x, int y)
        {
            return obj.X + obj.width >= x && obj.X < x + Engine.Render.RenderWidth && obj.Y + obj.height >= y && obj.Y < y + Engine.Render.RenderHeight;
        }
    }
}
