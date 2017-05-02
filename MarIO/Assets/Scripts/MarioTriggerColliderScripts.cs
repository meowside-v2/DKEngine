using DKEngine.Core;
using DKEngine.Core.Components;
using MarIO.Assets.Models;
using System.Diagnostics;

namespace MarIO.Assets.Scripts
{
    internal class BottomMarioChecker : Script
    {
        private Mario Mario;

        public BottomMarioChecker(GameObject Parent) : base(Parent)
        { }

        protected override void OnColliderEnter(Collider e)
        {
            if (e.Parent is Enemy)
            {
                Enemy tmp = e.Parent as Enemy;
                Debug.WriteLine(string.Format("Zabil jsi {0}", tmp.TypeName));
                tmp.IsDestroyed = true;
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

    internal class TopMarioChecker : Script
    {
        private Mario Mario;

        public TopMarioChecker(GameObject Parent) : base(Parent)
        { }

        protected override void OnColliderEnter(Collider e)
        {
            if (e.Parent is Enemy)
            {
                Debug.WriteLine(string.Format("Zabilo Tě {0}", e.Parent.TypeName));
                Mario.IsDestroyed = true;
            }
            else if (e.Parent is Block)
            {
                Block tmp = e.Parent as Block;

                if (tmp.State == Block.CollisionState.Stay)
                {
                    tmp.State = Block.CollisionState.Up;

                    Shared.BlocksStartPositions.Add(tmp.Transform.Position.Y);
                    Shared.BlocksToUpdate.Add(tmp);
                }
            }
        }

        protected override void Start()
        {
            Mario = GameObject.Find<Mario>("Player");
        }

        protected override void Update()
        { }
    }

    internal class LeftMarioChecker : Script
    {
        private Mario Mario;

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

    internal class RightMarioChecker : Script
    {
        private Mario Mario;

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