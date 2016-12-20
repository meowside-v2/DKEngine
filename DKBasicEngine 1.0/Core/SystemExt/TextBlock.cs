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

        protected float _x = 0;
        protected float _y = 0;

        protected float _width = 0;
        protected float _height = 0;

        protected float vertOffset = 0;
        protected float horiOffset = 0;

        protected HorizontalAlignment _HA;
        protected VerticalAlignment _VA;

        protected HorizontalAlignment _THA;
        protected VerticalAlignment _TVA;

        protected string _textStr = "";

        protected List<Letter> _text = new List<Letter>();

        protected float _scaleX = 1;
        protected float _scaleY = 1;
        protected float _scaleZ = 1;

        public Color? Foreground { get; set; }

        private Color? _bg;
        public Color? Background
        {
            get { return _bg; }
            set
            {
                _bg = value;

                if (value != null)
                    Model = new Material((Color)value, this);
            }
        }
        
        public float FontSize { get; set; } = 1;

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
        
        public float X
        {
            get { return _x + horiOffset; }
            set { _x = value; }
        }
        public float Y
        {
            get { return _y + vertOffset; }
            set { _y = value; }
        }
        public float Z { get; set; }
        
        public float width
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
        public float height
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
        public float depth
        {
            get { return 0; }
            set { }
        }

        public float ScaleX
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
        public float ScaleY
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
        public float ScaleZ
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

        public Material Model { get; set; }
        public Animator Animator { get; set; }

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

                float maxHeight = textAligned.Count * 6 * FontSize;

                float startY = 0;

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
                    float maxWidth = 0;

                    if(row.Count > 0)
                        maxWidth = (row[row.Count - 1].Model.width + row[row.Count - 1]._x) * FontSize;

                    if(maxWidth != 0)
                    {
                        float startX = 0;

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

        public void Destroy()
        {

        }

        public void Render()
        {
            Model?.Render(this);
        }
    }
}
