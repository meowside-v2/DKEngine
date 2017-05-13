using DKEngine.Core;
using DKEngine.Core.Components;
using MarIO.Assets.Models;
using MarIO.Assets.Models.Miscellaneous;
using System.Diagnostics;

namespace MarIO.Assets.Scripts
{
    public class BottomMarioChecker : Script
    {
        private Mario Mario;

        public BottomMarioChecker(GameObject Parent) : base(Parent)
        { }

        protected override void OnColliderEnter(Collider e)
        {
            if (e.Parent is Enemy)
            {
                Enemy tmp = e.Parent as Enemy;
                Debug.WriteLine(string.Format("Zabil jsi {0}", tmp.Name));
                tmp.IsDestroyed = true;
                Mario.KilledEnemy = true;
            }
            else if (e.Parent is PowerUp)
            {
                ((PowerUp)e.Parent).OnPickedUp?.Invoke();
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
                Mario.CurrentState--;
            }
            else if (e.Parent is Block)
            {
                Block tmp = e.Parent as Block;

                if (tmp.State == Block.CollisionState.Stay)
                {
                    tmp.AnimateBlockCollision();
                }

                tmp.GetContent();
            }
            else if (e.Parent is PowerUp)
            {
                ((PowerUp)e.Parent).OnPickedUp?.Invoke();
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
                Mario.CurrentState--;
                //Mario?.Destroy();
            }
            else if (e.Parent is PowerUp)
            {
                ((PowerUp)e.Parent).OnPickedUp?.Invoke();
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
                Mario.CurrentState--;
                //Mario?.Destroy();
            }
            else if (e.Parent is PowerUp)
            {
                ((PowerUp)e.Parent).OnPickedUp?.Invoke();
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