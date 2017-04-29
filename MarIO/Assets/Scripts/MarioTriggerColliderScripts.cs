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
        Mario Mario;

        public BottomMarioChecker(GameObject Parent) : base(Parent)
        { }

        protected override void OnColliderEnter(Collider e)
        {
            if(e.Parent is Enemy)
            {
                Debug.WriteLine(string.Format("Zabil jsi {0}", e.Parent.TypeName));
                e.Parent.Destroy();
                Mario.KilledEnemy = true;
            }
        }

        protected override void Start()
        {
            Mario = GameObject.Find<Mario>("Player");
        }

        protected override void Update()
        { }
    }

    class TopMarioChecker : Script
    {
        Mario Mario;

        public TopMarioChecker(GameObject Parent) : base(Parent)
        { }

        protected override void OnColliderEnter(Collider e)
        {
            if (e.Parent is Enemy)
            {
                Debug.WriteLine(string.Format("Zabilo Tě {0}", e.Parent.TypeName));
                Mario.IsDestroyed = true;
                //Mario?.Destroy();
            }
            else if(e.Parent is Block)
            {
                ((Block)e.Parent).ChangeState = true;
            }
        }

        protected override void Start()
        {
            Mario = GameObject.Find<Mario>("Player");
        }

        protected override void Update()
        { }
    }

    class LeftMarioChecker : Script
    {
        Mario Mario;

        public LeftMarioChecker(GameObject Parent) : base(Parent)
        { }

        protected override void OnColliderEnter(Collider e)
        {
            if (e.Parent is Enemy)
            {
                Debug.WriteLine(string.Format("Zabilo Tě {0}", e.Parent.TypeName));
                Mario.IsDestroyed = true;
                //Mario?.Destroy();
            }
        }

        protected override void Start()
        {
            Mario = GameObject.Find<Mario>("Player");
        }

        protected override void Update()
        { }
    }

    class RightMarioChecker : Script
    {
        Mario Mario;

        public RightMarioChecker(GameObject Parent) : base(Parent)
        { }

        protected override void OnColliderEnter(Collider e)
        {
            if (e.Parent is Enemy)
            {
                Debug.WriteLine(string.Format("Zabilo Tě {0}", e.Parent.TypeName));
                Mario.IsDestroyed = true;
                //Mario?.Destroy();
            }
        }

        protected override void Start()
        {
            Mario = GameObject.Find<Mario>("Player");
        }

        protected override void Update()
        { }
    }
}
