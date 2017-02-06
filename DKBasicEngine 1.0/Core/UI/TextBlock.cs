/*
* (C) 2017 David Knieradl 
*/

using DKBasicEngine_1_0.Core.Components;
using DKBasicEngine_1_0.Core.Scripts;
using System.Collections.Generic;
using System.Drawing;
using static DKBasicEngine_1_0.Core.UI.Text;

namespace DKBasicEngine_1_0.Core.UI
{
    public class TextBlock : GameObject, IText
    {
        internal bool _changed = false;
        
        internal float vertOffset = 0;
        internal float horiOffset = 0;

        internal HorizontalAlignment _HA  = HorizontalAlignment.Left;
        internal VerticalAlignment _VA    = VerticalAlignment.Top;
        internal HorizontalAlignment _THA = HorizontalAlignment.Left;
        internal VerticalAlignment _TVA   = VerticalAlignment.Top;

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

        internal string _textStr = "";
        internal List<Letter> _text = new List<Letter>();
        
        internal Color? _bg;
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

        protected float _FontSize = 1;
        public float FontSize
        {
            get { return _FontSize; }
            set
            {
                if (value <= 0)
                {
                    _FontSize = 0.01f;
                    _changed = true;
                }
                else
                {
                    _FontSize = value;
                    _changed = true;
                }
            }
        }
        
        public HorizontalAlignment HAlignment
        {
            set
            {
                _HA = value;

                if (_IsGUI)
                {
                    this.Transform.Position -= new Vector3(horiOffset, 0, 0);

                    switch (value)
                    {
                        case HorizontalAlignment.Left:
                            horiOffset = 0;
                            break;

                        case HorizontalAlignment.Center:
                            horiOffset = (Engine.Render.RenderWidth - this.Transform._ScaledDimensions.X) / 2;
                            break;

                        case HorizontalAlignment.Right:
                            horiOffset = Engine.Render.RenderWidth - this.Transform._ScaledDimensions.X;
                            break;
                    }

                    this.Transform.Position += new Vector3(horiOffset, 0, 0);
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
                    this.Transform.Position -= new Vector3(0, vertOffset, 0);

                    switch (value)
                    {
                        case VerticalAlignment.Top:
                            vertOffset = 0;
                            break;

                        case VerticalAlignment.Center:
                            vertOffset = (Engine.Render.RenderHeight - this.Transform._ScaledDimensions.Y) / 2;
                            break;

                        case VerticalAlignment.Bottom:
                            vertOffset = Engine.Render.RenderHeight - this.Transform._ScaledDimensions.Y;
                            break;
                    }

                    this.Transform.Position += new Vector3(0, vertOffset, 0);
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
        {
            this.Scripts.Add(new TextControlScript(this));
        }

        public TextBlock(GameObject Parent)
            : base(Parent)
        {
            this.Scripts.Add(new TextControlScript(this));
        }

        public override void Destroy()
        {
            if (Engine.CurrentScene.NewlyGenerated.Contains(this))
                Engine.CurrentScene.NewlyGenerated.Remove(this);
            //Engine.CurrentScene.AllGameObjects.Remove(this);
            Engine.ToRender.Remove(this);

            int ScriptsCount = this.Scripts.Count;
            for (int i = 0; i < ScriptsCount; i++)
                Scripts[i].Destroy();
            
            int _textCount = _text.Count;
            for (int i = 0; i < _textCount; i++)
                _text[0].Destroy();

            Animator.Destroy();

            Model = null;
            Animator = null;
        }
    }
}
