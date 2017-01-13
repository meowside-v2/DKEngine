using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace DKBasicEngine_1_0
{
    public class TextBlock : GameObject, ICore, IText, IGraphics
    {
        protected bool _changed = false;
        
        protected float _width = 0;
        protected float _height = 0;

        protected float vertOffset = 0;
        protected float horiOffset = 0;

        protected HorizontalAlignment _HA;
        protected VerticalAlignment _VA;

        protected HorizontalAlignment _THA;
        protected VerticalAlignment _TVA;

        protected string _textStr = "";

        internal List<Letter> _text = new List<Letter>();
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
        
        public float FontSize = 1;

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

        public bool TextShadow = false;
        internal override float X { get { return Position.X + horiOffset; } }
        internal override float Y { get { return Position.Y + vertOffset; } }
        internal override float Z { get { return Position.Z; } }
        
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
                            horiOffset = (Engine.Render.RenderWidth - this.Width) / 2;
                            break;

                        case HorizontalAlignment.Right:
                            horiOffset = Engine.Render.RenderWidth - this.Width;
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
                            vertOffset = (Engine.Render.RenderHeight - this.Height) / 2;
                            break;

                        case VerticalAlignment.Bottom:
                            vertOffset = Engine.Render.RenderHeight - this.Height;
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

        public TextBlock()
        { }

        public TextBlock(GameObject Parent)
            : base(Parent)
        { }
        
        public override void Start()
        {
            if (Text.Length > 0)
            {
                TextControl();
            }
        }

        public override void Update()
        {
            if (_changed)
            {
                TextControl();
            }
        }

        public override void Destroy()
        {
            Engine.ToUpdate.Remove(this);
            Engine.ToRender.Remove(this);

            int _textCount = _text.Count;

            for (int i = 0; i < _textCount; i++)
                _text[0].Destroy();
                
            Model = null;
            Animator = null;
        }

        private void TextControl()
        {
            int _textCount = _text.Count;

            for (int i = 0; i < _textCount; i++)
                _text[0].Destroy();

            List<Letter> retValue = new List<Letter>();

            List<List<Letter>> textAligned = new List<List<Letter>>() { new List<Letter>() };

            int Xoffset = 0;
            int Yoffset = 0;

            if (Width > 0)
            {
                for (int i = 0; i < _textStr.Length; i++)// (char letter in Text)
                {
                    if (_textStr[i] == ' ')
                    {
                        Xoffset += 3;
                    }

                    else
                    {
                        if (_textStr[i] == '\r' || _textStr[i] == '\n')
                        {
                            Xoffset = 0;
                            Yoffset += 6;

                            textAligned.Add(new List<Letter>());

                            continue;
                        }

                        Material newLetterMaterial = Database.GetLetter(_textStr[i]);

                        if (Xoffset * ScaleX * FontSize + newLetterMaterial.Width * ScaleX * FontSize > this.Width)
                        {
                            Xoffset = 0;
                            Yoffset += 6;

                            textAligned.Add(new List<Letter>());
                        }

                        textAligned[Yoffset / 6].Add(new Letter(this,
                                                                new Position(Xoffset, Yoffset, 1),
                                                                newLetterMaterial));

                        Xoffset += newLetterMaterial.Width + 1;
                    }
                }
            }

            int textAlignedCount = textAligned.Count;
            float maxHeight = textAlignedCount * 6 * FontSize;
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


            for (int i = 0; i < textAlignedCount; i++)
            {
                float maxWidth = 0;
                int textAlignedRowCount = textAligned[i].Count;

                if (textAlignedRowCount > 0)
                    maxWidth = (textAligned[i][textAlignedCount - 1].Model.Width + textAligned[i][textAlignedCount - 1].Position.X) * FontSize;

                if (maxWidth != 0)
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


                    for (int j = 0; j < textAlignedRowCount; j++)//foreach (Letter letter in row)
                    {
                        if (startX != 0)
                            textAligned[i][j].HorOffset = startX;

                        if (startY != 0)
                            textAligned[i][j].VertOffset = startY;

                        retValue.Add(textAligned[i][j]);
                    }
                }
            }

            _text = retValue;

            VAlignment = _VA;
            HAlignment = _HA;

            _changed = false;
        }
    }
}
