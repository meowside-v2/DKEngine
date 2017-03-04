using DKBasicEngine_1_0.Core.Components;
using DKBasicEngine_1_0.Core.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DKBasicEngine_1_0.Core.UI.Text;
using static DKBasicEngine_1_0.Core.UI.TextBlock;

namespace DKBasicEngine_1_0.Core.Scripts
{
    internal sealed class TextControlScript : Script
    {
        internal TextBlock _Parent { get { return (TextBlock)Parent; } }

        public TextControlScript(TextBlock Parent)
            : base(Parent)
        { }

        protected internal override void Start()
        {
            if (_Parent.Text.Length > 0)
            {
                Text();
            }
        }

        protected internal override void Update()
        {
            if (_Parent._changed)
            {
                Text();
            }
        }

        private void Text()
        {
            int _textCount = _Parent._text.Count;
            for (int i = _textCount - 1; i >= 0; i--)
                _Parent._text[i].Destroy();

            _Parent.VAlignment = _Parent._VA;
            _Parent.HAlignment = _Parent._HA;

            List<Letter> retValue = new List<Letter>();
            List<List<Letter>> textAligned = new List<List<Letter>>() { new List<Letter>() };

            float Xoffset = 0; //this.Transform.Position.X + horiOffset;
            float Yoffset = 0; //this.Transform.Position.Y + vertOffset;
            int rows = 0;

            if (_Parent.Transform.Dimensions.X > 0)
            {
                for (int i = 0; i < _Parent._textStr.Length; i++)// (char letter in Text)
                {
                    if (_Parent._textStr[i] == ' ')
                    {
                        Xoffset += 3 * _Parent.Transform.Scale.X * _Parent.FontSize;
                    }

                    else
                    {
                        if (_Parent._textStr[i] == '\r' || _Parent._textStr[i] == '\n')
                        {
                            Xoffset = 0;
                            Yoffset += 6 * _Parent.Transform.Scale.Y * _Parent.FontSize;
                            rows++;

                            textAligned.Add(new List<Letter>());

                            continue;
                        }

                        Material newLetterMaterial = Database.GetLetter(_Parent._textStr[i]);

                        if (Xoffset + newLetterMaterial.Width * _Parent.FontSize > _Parent.Transform.Dimensions.X)
                        {
                            Xoffset = 0;
                            Yoffset += 6 * _Parent.Transform.Scale.Y * _Parent.FontSize;
                            rows++;

                            textAligned.Add(new List<Letter>());
                        }

                        /*textAligned[Yoffset / 6].Add(new Letter(this,
                                                                new Position(Xoffset, Yoffset, 1),
                                                                newLetterMaterial));*/

                        Letter l = new Letter(_Parent);

                        l.Transform.Position += new Vector3(Xoffset, Yoffset, 1);
                        l.Model = newLetterMaterial;
                        l.Transform.Scale *= _Parent.FontSize;
                        l.Name = _Parent._textStr[i].ToString();
                        l.HasShadow = _Parent.TextShadow;
                        textAligned[rows].Add(l);

                        Xoffset += (l.Transform.Dimensions.X + 1) * l.Transform.Scale.X;
                    }
                }
            }

            int textAlignedCount = textAligned.Count;
            float maxHeight = textAlignedCount * 6 * _Parent.FontSize * _Parent.Transform.Scale.Y;
            float startY = 0;

            switch (_Parent._TVA)
            {
                case VerticalAlignment.Top:
                    startY = 0;
                    break;
                case VerticalAlignment.Center:
                    startY = (_Parent.Transform.Dimensions.Y * _Parent.Transform.Scale.Y * _Parent.FontSize - maxHeight) / 2;
                    break;
                case VerticalAlignment.Bottom:
                    startY = _Parent.Transform.Dimensions.Y * _Parent.Transform.Scale.Y * _Parent.FontSize - maxHeight;
                    break;
                default:
                    break;
            }


            for (int i = 0; i < textAlignedCount; i++)
            {
                float maxWidth = 0;
                int textAlignedRowCount = textAligned[i].Count;

                if (textAlignedRowCount > 0)
                    maxWidth = textAligned[i][textAlignedRowCount - 1].Model.Width * _Parent.Transform.Scale.X * _Parent.FontSize + textAligned[i][textAlignedRowCount - 1].Transform.Position.X - textAligned[i][0].Transform.Position.X;

                if (maxWidth != 0)
                {
                    float startX = 0;

                    switch (_Parent._THA)
                    {
                        case HorizontalAlignment.Left:
                            startX = 0;
                            break;

                        case HorizontalAlignment.Center:
                            startX = (_Parent.Transform.Dimensions.X * _Parent.Transform.Scale.X - maxWidth) / 2;
                            break;

                        case HorizontalAlignment.Right:
                            startX = _Parent.Transform.Dimensions.X * _Parent.Transform.Scale.X - maxWidth;
                            break;
                    }


                    for (int j = 0; j < textAlignedRowCount; j++)//foreach (Letter letter in row)
                    {
                        if (startX != 0 || startY != 0)
                            textAligned[i][j].Transform.Position += new Vector3(startX, startY, 0);

                        retValue.Add(textAligned[i][j]);
                    }
                }
            }

            _Parent._text = retValue;

            _Parent._changed = false;
        }

        protected internal override void OnColliderEnter(Collider e)
        { }
    }
}
