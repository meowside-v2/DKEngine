using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace DKBasicEngine_1_0
{
    public class Letter : ICore, I3Dimensional, IGraphics
    {
        TextBlock Parent;

        internal double _x { get; private set; } = 0;
        internal double _y { get; private set; } = 0;

        internal double VertOffset = 0;
        internal double HorOffset = 0;

        public bool HasShadow
        {
            get { return Parent.HasShadow; }
            set { }
        }

        public bool _IsVisble { get; set; }
        
        public double X
        {
            get { return HorOffset + _x * Parent.FontSize * Parent.ScaleX + Parent.X; }
            set { _x = value; }
        }
        public double Y
        {
            get { return VertOffset + _y * Parent.FontSize * Parent.ScaleY + Parent.Y; }
            set { _y = value; }
        }
        public double Z { get; set; }

        public double width
        {
            get { return Model.width * Parent.FontSize * Parent.ScaleX; }
            set { }
        }

        public double height
        {
            get { return Model.height * Parent.FontSize * Parent.ScaleX; }
            set { }
        }

        public double depth
        {
            get { return 0; }
            set { }
        }

        public double ScaleX
        {
            get { return Parent.FontSize * Parent.ScaleX; }
            set { }
        }

        public double ScaleY
        {
            get { return Parent.FontSize * Parent.ScaleY; }
            set { }
        }

        public double ScaleZ
        {
            get { return Parent.FontSize * Parent.ScaleZ; }
            set { }
        }

        public bool LockScaleRatio { get; set; } = true;

        public Animator Animator { get; set; }
        public Material Model { get; set; }
        private Material modelRastered = null;
        

        public Color? Foreground { get { return Parent.Foreground; } }
        

        public bool IsGUI
        {
            get { return Parent.IsGUI; }
            set { }
        }

        public Letter(TextBlock Parent, double x, double y, double z, Material sourceModel)
        {
            Model = sourceModel;

            this.X = x;
            this.Y = y;
            this.Z = z;

            this.Parent = Parent;

            Animator = new Animator(this);
            modelRastered = new Material(Model, this);

            this.Start();
        }

        public void Start()
        {
            Engine.ToRender.Add(this);
        }

        public void Update()
        {
            
        }

        public void Destroy()
        {

        }

        public void Render()
        {
            modelRastered?.Render(this);
        }
    }
}
