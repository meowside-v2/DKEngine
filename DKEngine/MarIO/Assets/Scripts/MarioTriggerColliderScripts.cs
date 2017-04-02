using DKEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DKEngine.Core.Components;
using MarIO.Assets.Models;
using System.Diagnostics;

namespace MarIO.Assets.Scripts
{
    class BottomMarioChecker : Script
    {
        public BottomMarioChecker(GameObject Parent) : base(Parent)
        { }

        protected override void OnColliderEnter(Collider e)
        {
            if(e.Parent is Enemy)
            {
                e.Parent.Destroy();
                //Debug.WriteLine("Zabil jsi {0}", e.Parent.TypeName);
            }
        }

        protected override void Start()
        {
            this.Parent.Collider.Area = new System.Drawing.RectangleF(0,
                                                                      this.Parent.Transform.Dimensions.Y * this.Parent.Transform.Scale.Y,
                                                                      this.Parent.Transform.Dimensions.X * this.Parent.Transform.Scale.X,
                                                                      2);
        }

        protected override void Update()
        { }
    }

    class TopMarioChecker : Script
    {
        public TopMarioChecker(GameObject Parent) : base(Parent)
        { }

        protected override void OnColliderEnter(Collider e)
        {
            if (e.Parent is Enemy)
            {
                Debug.WriteLine("Zabilo Tě {0}", e.Parent.TypeName);
            }
        }

        protected override void Start()
        {
            this.Parent.Collider.Area = new System.Drawing.RectangleF(0,
                                                                      -2,
                                                                      this.Parent.Transform.Dimensions.X * this.Parent.Transform.Scale.X,
                                                                      2);
        }

        protected override void Update()
        { }
    }

    class LeftMarioChecker : Script
    {
        public LeftMarioChecker(GameObject Parent) : base(Parent)
        { }

        protected override void OnColliderEnter(Collider e)
        {
            if (e.Parent is Enemy)
            {
                Debug.WriteLine("Zabilo Tě {0}", e.Parent.TypeName);
            }
        }

        protected override void Start()
        {
            this.Parent.Collider.Area = new System.Drawing.RectangleF(-2,
                                                                      0,
                                                                      2,
                                                                      this.Parent.Transform.Dimensions.Y * this.Parent.Transform.Scale.Y);
        }

        protected override void Update()
        { }
    }

    class RightMarioChecker : Script
    {
        public RightMarioChecker(GameObject Parent) : base(Parent)
        { }

        protected override void OnColliderEnter(Collider e)
        {
            if (e.Parent is Enemy)
            {
                Debug.WriteLine("Zabilo Tě {0}", e.Parent.TypeName);
            }
        }

        protected override void Start()
        {
            this.Parent.Collider.Area = new System.Drawing.RectangleF(this.Parent.Transform.Dimensions.X * this.Parent.Transform.Scale.X,
                                                                      0,
                                                                      2,
                                                                      this.Parent.Transform.Dimensions.Y * this.Parent.Transform.Scale.Y);
        }

        protected override void Update()
        { }
    }
}
