using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKBasicEngine_1_0.Core.SystemExt
{
    class Dialog : ICore, I3Dimensional, IGraphics
    {
        protected float _scaleX = 1;
        protected float _scaleY = 1;
        protected float _scaleZ = 1;

        protected float _width = 0;
        protected float _height = 0;

        public I3Dimensional Parent { get; private set; }

        public float width
        {
            get { return _width * ScaleX; }
            set
            {
                if (value != _width)
                {
                    if (value < 0)
                        _width = 0;

                    else
                        _width = value;
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
                        _height = 0;

                    else
                        _height = value;
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
        
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }


        public Material Model { get; set; }
        public Animator Animator { get; set; }
        private bool _IsGUI = false;
        public bool IsGUI
        {
            get { return Parent != null ? ((IGraphics)Parent).IsGUI : _IsGUI; }
            set { _IsGUI = value; }
        }
        public bool HasShadow { get; set; }

        public TextBlock Caption;
        public readonly List<I3Dimensional> Content = new List<I3Dimensional>();

        public Dialog(string Caption)
        {
            this.Caption = new TextBlock(null, this);
            this.Caption.Text = Caption;
        }

        public void ChangeContent(params I3Dimensional[] stuff)
        {

        }

        public void Destroy() { }
        public void Render() { }
        public void Start() { }
        public void Update() { }
    }
}
