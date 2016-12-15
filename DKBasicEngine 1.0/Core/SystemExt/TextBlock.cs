using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace DKBasicEngine_1_0
{
    public class TextBlock : ICore, I3Dimensional, IText, IGraphics
    {
        //I3Dimensional Parent;

        protected bool _changed = false;

        protected double _x = 0;
        protected double _y = 0;

        protected double _width = 0;
        protected double _height = 0;

        protected double vertOffset = 0;
        protected double horiOffset = 0;

        protected HorizontalAlignment _HA;
        protected VerticalAlignment _VA;

        protected HorizontalAlignment _THA;
        protected VerticalAlignment _TVA;

        protected string _textStr = "";

        protected List<Letter> _text = new List<Letter>();

        protected double _scaleX = 1;
        protected double _scaleY = 1;
        protected double _scaleZ = 1;

        public Color? Foreground { get; set; }

        private Color? _bg;
        public Color? Background
        {
            get { return _bg; }
            set
            {
                _bg = value;

                if (value != null)
                    modelRastered = new Material((Color)value, this);
            }
        }
        

        public double FontSize { get; set; } = 1;

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

        public HorizontalAlignment TextHAlignment
        {
            set
            {
                if (value != _THA)
                {
                    _THA = value;

                    _changed = true;
                }
            }
        }
        public VerticalAlignment TextVAlignment
        {
            set
            {
                if (value != _TVA)
                {
                    _TVA = value;

                    _changed = true;
                }
            }
        }

        public Material modelBase { get; set; }
        public Material modelRastered { get; private set; }

        public int AnimationState { get; set; } = 0;
        public bool IsGUI { get; set; } = false;

        public TextBlock(Scene ParentPage)
        {
            this.Start();
            if(ParentPage != null)
                ParentPage.Model.Add(this);
        }
        
        public virtual void Start()
        {
            Engine.ToUpdate.Add(this);
            Engine.ToRender.Add(this);
        }
        public virtual void Update()
        {
            if (_changed)
            {
                List<Letter> retValue = new List<Letter>();

                List<List<Letter>> textAligned = new List<List<Letter>>() { new List<Letter>() };

                int Xoffset = 0;
                int Yoffset = 0;

                if(width > 0)
                {
                    foreach (char letter in Text)
                    {
                        if (letter == ' ')
                        {
                            Xoffset += 3;
                        }

                        else
                        {
                            if (letter == '\r' || letter == '\n')
                            {
                                Xoffset = 0;
                                Yoffset += 6;

                                textAligned.Add(new List<Letter>());

                                continue;
                            }

                            Material newLetterMaterial = Database.GetLetter(letter);

                            if (Xoffset * ScaleX * FontSize + newLetterMaterial.width * ScaleX * FontSize > this.width)
                            {
                                Xoffset = 0;
                                Yoffset += 6;

                                textAligned.Add(new List<Letter>());
                            }

                            textAligned[Yoffset / 6].Add(new Letter(this,
                                                Xoffset,
                                                Yoffset,
                                                this.Z + 1,
                                                newLetterMaterial));

                            Xoffset += newLetterMaterial.width + 1;
                        }
                    }
                }

                double maxHeight = textAligned.Count * 6 * FontSize;

                double startY = 0;

                switch (_TVA)
                {
                    case VerticalAlignment.Top:
                        startY = 0;
                        break;
                    case VerticalAlignment.Center:
                        startY = (_height - maxHeight) / 2;
                        break;
                    case VerticalAlignment.Bottom:
                        startY = _height - maxHeight;
                        break;
                    default:
                        break;
                }

                foreach(List<Letter> row in textAligned)
                {
                    double maxWidth = 0;

                    if(row.Count > 0)
                        maxWidth = (row[row.Count - 1].modelBase.width + row[row.Count - 1]._x) * FontSize;

                    if(maxWidth != 0)
                    {
                        double startX = 0;

                        switch (_THA)
                        {
                            case HorizontalAlignment.Left:
                                startX = 0;
                                break;

                            case HorizontalAlignment.Center:
                                startX = (_width - maxWidth) / 2;
                                break;

                            case HorizontalAlignment.Right:
                                startX = _width - maxWidth;
                                break;
                        }

                        
                        foreach (Letter letter in row)
                        {
                            if (startX != 0)
                                letter.HorOffset = startX;

                            if (startY != 0)
                                letter.VertOffset = startY;

                            retValue.Add(letter);
                        }
                    }
                }

                lock (Engine.ToRender)
                {
                    for (int i = 0; i < _text.Count; i++)
                        Engine.ToRender.Remove(_text[i]);
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

        public void Render()
        {
            modelRastered?.Render(this);
        }
    }
}
