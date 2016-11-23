using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKBasicEngine_1_0
{
    public class TextBlock : ICore, I3Dimensional
    {
        //I3Dimensional Parent;

        protected bool _changed = false;

        protected double _x = 0;
        protected double _y = 0;

        protected double vertOffset = 0;
        protected double horiOffset = 0;

        protected HorizontalAlignment _HA;
        protected VerticalAlignment _VA;
        
        protected List<Letter> _text = new List<Letter>();

        protected double _scaleX = 1;
        protected double _scaleY = 1;
        protected double _scaleZ = 1;
        
        public Color? Foreground { get; set; }

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
            get { return _x + horiOffset; }
            set { _x = value; }
        }
        public double Y
        {
            get { return _y + vertOffset; }
            set { _y = value; }
        }
        public double Z { get; set; }

        private double _width = 0;
        private double _height = 0;
        public double width
        {
            get { return _width * ScaleX; }
            set
            {
                if(value != _width)
                {
                    if (value < 0)
                        _width = value;

                    else
                        _width = value;

                    _changed = true;
                }
            }
        }
        public double height
        {
            get { return _height * ScaleY; }
            set
            {
                if (value != _height)
                {
                    if (value < 0)
                        _height = value;

                    else
                        _height = value;

                    _changed = true;
                }
            }
        }

        public double depth
        {
            get { return 0; }
            set { }
        }

        public double ScaleX
        {
            get
            {
                return _scaleX;
            }
            set
            {
                if (value != _scaleX)
                {
                    if (value < 0.1f)
                        _scaleX = 0.1f;

                    else
                        _scaleX = value;

                    if (LockScaleRatio)
                    {
                        _scaleZ = _scaleX;
                        _scaleY = _scaleX;
                    }
                }
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
                if (value != _scaleY)
                {
                    if (value < 0.1f)
                        _scaleY = 0.1f;

                    else
                        _scaleY = value;

                    if (LockScaleRatio)
                    {
                        _scaleX = _scaleY;
                        _scaleZ = _scaleY;
                    }
                }
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
                if (value != _scaleZ)
                {
                    if (value < 0.1f)
                        _scaleZ = 0.1f;

                    else
                        _scaleZ = value;

                    if (LockScaleRatio)
                    {
                        _scaleX = _scaleZ;
                        _scaleY = _scaleZ;
                    }
                }
            }
        }

        public bool LockScaleRatio { get; set; } = true;

        public bool HasShadow { get; set; }

        protected string _textStr = "";
        
        public virtual string Text
        {
            get { return _textStr; }
            set
            {
                if(value.All(ch => !ch.IsUnsupportedEscapeSequence()))
                {
                    _textStr = value;
                    _changed = true;
                }
            }
        }

        public HorizontalAlignment HAlignment
        {
            set
            {
                if(value != _HA)
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

                    _changed = true;
                }
            }
        }
        public VerticalAlignment VAlignment
        {
            set
            {
                if(value != _VA)
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

                    _changed = true;
                }
            }
        }

        public TextBlock(IPage ParentPage)
        {
            this.Start();
            if(ParentPage != null)
                ParentPage.Model.Add(this);
        }
        
        public virtual void Start()
        {
            lock (Engine.ToUpdate)
            {
                Engine.ToUpdate.Add(this);
            }
        }
        public virtual void Update()
        {
            if (_changed)
            {
                List<Letter> retValue = new List<Letter>();

                int Xoffset = 0;
                int Yoffset = 0;

                foreach (char letter in Text)
                {
                    if (letter == ' ')
                    {
                        Xoffset += 3;
                    }

                    else
                    {
                        if(letter == '\r' || letter == '\n')
                        {
                            Xoffset = 0;
                            Yoffset += 6;

                            continue;
                        }

                        Material newLetterMaterial = Database.GetLetter(letter);

                        if(Xoffset + newLetterMaterial.width > this.width)
                        {
                            Xoffset = 0;
                            Yoffset += 6;
                        }

                        retValue.Add(new Letter(this,
                                            Xoffset,
                                            Yoffset,
                                            0,
                                            newLetterMaterial));

                        Xoffset += newLetterMaterial.width + 1;
                    }
                }

                lock (_text)
                {
                    _text = retValue;
                }

                VAlignment = _VA;
                HAlignment = _HA;

                _changed = false;
            }
        }

        public void Render(int x, int y, byte[] imageBuffer, bool[] imageBufferKey)
        {
            List<Letter> reference;

            lock (_text)
            {
                reference = _text.ToList();
            }

            foreach (Letter item in reference.FindAll(obj => Finder(obj, x, y)))
            {
                item.Render((int)(this.X - x), (int)(this.Y - y), imageBuffer, imageBufferKey, Foreground ?? Color.White);
                if (HasShadow) item.Render((int)(this.X - x + 1), (int)(this.Y - y + 1), imageBuffer, imageBufferKey, Color.Black);
            }
        }

        protected bool Finder(I3Dimensional obj, int x, int y)
        {
            return obj.X + obj.width >= x && obj.X < x + Engine.Render.RenderWidth && obj.Y + obj.height >= y && obj.Y < y + Engine.Render.RenderHeight;
        }
    }
}
