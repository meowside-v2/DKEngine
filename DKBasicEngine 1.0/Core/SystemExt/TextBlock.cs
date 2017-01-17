/*
* (C) 2017 David Knieradl 
*/

using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace DKBasicEngine_1_0
{
    public class TextBlock : GameObject, IText
    {
        protected bool _changed = false;
        
        protected float vertOffset = 0;
        protected float horiOffset = 0;

        protected HorizontalAlignment _HA  = HorizontalAlignment.Left;
        protected VerticalAlignment _VA    = VerticalAlignment.Top;
        protected HorizontalAlignment _THA = HorizontalAlignment.Left;
        protected VerticalAlignment _TVA   = VerticalAlignment.Top;

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
        
        public virtual string Text
        {
            get { return _textStr; }
            set
            {
                _textStr = value;
                _changed = true;
            }
        }

        public HorizontalAlignment HAlignment
        {
            set
            {
                _HA = value;

                if (_IsGUI)
                {
                    this.Transform.Position -= new Position(horiOffset, 0, 0);

                    switch (value)
                    {
                        case HorizontalAlignment.Left:
                            horiOffset = 0;
                            break;

                        case HorizontalAlignment.Center:
                            horiOffset = (Engine.Render.RenderWidth - this.Transform.Dimensions.Width * Transform.Scale.X) / 2;
                            break;

                        case HorizontalAlignment.Right:
                            horiOffset = Engine.Render.RenderWidth - this.Transform.Dimensions.Width * Transform.Scale.X;
                            break;
                    }

                    this.Transform.Position += new Position(horiOffset, 0, 0);
                }
                    
                _changed = true;
            }
        }
        public VerticalAlignment VAlignment
        {
            set
            {
                _VA = value;

                if (_IsGUI)
                {
                    this.Transform.Position -= new Position(0, vertOffset, 0);

                    switch (value)
                    {
                        case VerticalAlignment.Top:
                            vertOffset = 0;
                            break;

                        case VerticalAlignment.Center:
                            vertOffset = (Engine.Render.RenderHeight - this.Transform.Dimensions.Height * Transform.Scale.Y) / 2;
                            break;

                        case VerticalAlignment.Bottom:
                            vertOffset = Engine.Render.RenderHeight - this.Transform.Dimensions.Height * Transform.Scale.Y;
                            break;
                    }

                    this.Transform.Position += new Position(0, vertOffset, 0);
                }
                    

                _changed = true;
            }
        }

        public HorizontalAlignment TextHAlignment
        {
            set
            {
                _THA = value;
                _changed = true;
            }
        }
        public VerticalAlignment TextVAlignment
        {
            set
            {
                _TVA = value;
                _changed = true;
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
            Engine.UpdateEvent -= this.UpdateHandler;
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
            for (int i = _textCount - 1; i >= 0; i--)
                _text[i].Destroy();

            VAlignment = _VA;
            HAlignment = _HA;

            List<Letter> retValue = new List<Letter>();
            List<List<Letter>> textAligned = new List<List<Letter>>() { new List<Letter>() };

            float Xoffset = 0; //this.Transform.Position.X + horiOffset;
            float Yoffset = 0; //this.Transform.Position.Y + vertOffset;
            int rows = 0;

            if (Transform.Dimensions.Width > 0)
            {
                for (int i = 0; i < _textStr.Length; i++)// (char letter in Text)
                {
                    if (_textStr[i] == ' ')
                    {
                        Xoffset += 3 * Transform.Scale.X * FontSize;
                    }

                    else
                    {
                        if (_textStr[i] == '\r' || _textStr[i] == '\n')
                        {
                            Xoffset = 0;
                            Yoffset += 6 * Transform.Scale.Y * FontSize;
                            rows++;

                            textAligned.Add(new List<Letter>());

                            continue;
                        }

                        Material newLetterMaterial = Database.GetLetter(_textStr[i]);

                        if (Xoffset + newLetterMaterial.Width * FontSize > this.Transform.Dimensions.Width)
                        {
                            Xoffset = 0;
                            Yoffset += 6 * Transform.Scale.Y * FontSize;
                            rows++;

                            textAligned.Add(new List<Letter>());
                        }

                        /*textAligned[Yoffset / 6].Add(new Letter(this,
                                                                new Position(Xoffset, Yoffset, 1),
                                                                newLetterMaterial));*/

                        Letter l = new Letter(this, newLetterMaterial);
                        l.Transform.Position = new Position(Xoffset + this.Transform.Position.X,
                                                            Yoffset + this.Transform.Position.Y,
                                                            1);
                        l.Transform.Scale = this.Transform.Scale;
                        textAligned[rows].Add(l);

                        Xoffset += (newLetterMaterial.Width + 1) * Transform.Scale.X * FontSize;
                    }
                }
            }

            int textAlignedCount = textAligned.Count;
            float maxHeight = textAlignedCount * 6 * FontSize * Transform.Scale.Y;
            float startY = 0;

            switch (_TVA)
            {
                case VerticalAlignment.Top:
                    startY = 0;
                    break;
                case VerticalAlignment.Center:
                    startY = (this.Transform.Dimensions.Height * this.Transform.Scale.Y * FontSize - maxHeight) / 2;
                    break;
                case VerticalAlignment.Bottom:
                    startY = this.Transform.Dimensions.Height * this.Transform.Scale.Y * FontSize - maxHeight;
                    break;
                default:
                    break;
            }


            for (int i = 0; i < textAlignedCount; i++)
            {
                float maxWidth = 0;
                int textAlignedRowCount = textAligned[i].Count;

                if (textAlignedRowCount > 0)
                    maxWidth = textAligned[i][textAlignedRowCount - 1].Model.Width * this.Transform.Scale.X * FontSize + textAligned[i][textAlignedRowCount - 1].Transform.Position.X - textAligned[i][0].Transform.Position.X;

                if (maxWidth != 0)
                {
                    float startX = 0;

                    switch (_THA)
                    {
                        case HorizontalAlignment.Left:
                            startX = 0;
                            break;

                        case HorizontalAlignment.Center:
                            startX = (this.Transform.Dimensions.Width * this.Transform.Scale.X - maxWidth) / 2;
                            break;

                        case HorizontalAlignment.Right:
                            startX = this.Transform.Dimensions.Width * this.Transform.Scale.X - maxWidth;
                            break;
                    }


                    for (int j = 0; j < textAlignedRowCount; j++)//foreach (Letter letter in row)
                    {
                        if (startX != 0)
                            textAligned[i][j].Transform.Position += new Position(startX, 0, 0);

                        if (startY != 0)
                            textAligned[i][j].Transform.Position += new Position(0, startY, 0);

                        retValue.Add(textAligned[i][j]);
                    }
                }
            }

            _text = retValue;
            
            _changed = false;
        }
    }
}
