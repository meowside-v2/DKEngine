using DKEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DKEngine.Core.Components;
using MarIO.Assets.Models;
using System.Diagnostics;
using System.Drawing;

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
                Debug.WriteLine("Zabil jsi {0}", e.Parent.TypeName.ToString());
                e.Parent.Destroy();
            }
        }

        protected override void Start()
        { }

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
                Debug.WriteLine("Zabilo Tě {0}", e.Parent.TypeName.ToString());
            }
        }

        protected override void Start()
        { }

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
                Debug.WriteLine("Zabilo Tě {0}", e.Parent.TypeName.ToString());
            }
        }

        protected override void Start()
        { }

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
                Debug.WriteLine("Zabilo Tě {0}", e.Parent.TypeName.ToString());
            }
        }

        protected override void Start()
        { }

        protected override void Update()
        { }
    }
}
