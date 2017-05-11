/*
* (C) 2017 David Knieradl
*/

using DKEngine.Core.Components;
using DKEngine.Core.Scripts;
using System.Collections.Generic;
using System.Drawing;
using static DKEngine.Core.UI.Text;

namespace DKEngine.Core.UI
{
    public class TextBlock : GameObject, IText
    {
        public virtual string Text
        {
            get { return _textStr; }
            set
            {
                if (value != _textStr)
                {
                    _textStr = value;
                    _changed = true;
                }
            }
        }

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

                //if (_IsGUI)
                //{
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
                //}

                //_changed = true;
            }
        }

        public VerticalAlignment VAlignment
        {
            set
            {
                _VA = value;

                //if (_IsGUI)
                //{
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
                //}

                //_changed = true;
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

        public bool TextShadow = false;

        internal List<Letter> _text = new List<Letter>();
        internal float _FontSize = 1;
        internal Color? _bg;
        internal string _textStr = "";

        internal HorizontalAlignment _HA = HorizontalAlignment.Left;
        internal VerticalAlignment _VA = VerticalAlignment.Top;
        internal HorizontalAlignment _THA = HorizontalAlignment.Left;
        internal VerticalAlignment _TVA = VerticalAlignment.Top;

        internal float vertOffset = 0;
        internal float horiOffset = 0;
        internal bool _changed = false;

        public TextBlock()
            : base()
        { }

        public TextBlock(GameObject Parent)
            : base(Parent)
        { }

        protected override void Initialize()
        {
            this.VAlignment = _VA;
            this.HAlignment = _HA;
            this.InitNewScript<TextControlScript>();
        }

        internal override void Render()
        { Model?.Render(this, _bg); }

        public override void Destroy()
        {
            try
            {
                Engine.CurrentScene.NewlyGeneratedComponents.Pop();
            }
            catch { }

            //Engine.CurrentScene.AllGameObjects.Remove(this);
            //Engine.RenderGameObjects.Remove(this);

            int ScriptsCount = this.Scripts.Count;
            for (int i = 0; i < ScriptsCount; i++)
                Scripts[0].Destroy();

            int _textCount = _text.Count;
            for (int i = 0; i < _textCount; i++)
            {
                _text[0].Destroy();
                _text.RemoveAt(0);
            }

            Animator?.Destroy();

            Model = null;
            Animator = null;
        }
    }
}