using DKEngine;
using DKEngine.Core;
using DKEngine.Core.Components;
using MarIO.Assets.Models;

namespace MarIO.Assets.Scripts
{
    internal class GoombaController : Script
    {
        private const int Speed = 20;
        private const int FloatSpeed = 60;
        private const int Acceleration = 20;

        private int CurrentSpeed = 0;
        private float vertSpeed = 0;
        private bool IsFalling = false;

        private bool firstTimeDeadAnimation = true;

        private Enemy Target;

        public GoombaController(GameObject Parent) : base(Parent)
        {
            Target = (Enemy)Parent;
        }

        protected override void OnColliderEnter(Collider e)
        { }

        protected override void Start()
        {
            CurrentSpeed = -Speed;
        }

        protected override void Update()
        {
            if (!Target.IsDestroyed)
            {
                Movement();
            }
            else
            {
                DeadAnimation();
            }
        }

        private void Movement()
        {
            if (this.Parent.Collider.Collision(Collider.Direction.Left))
            {
                CurrentSpeed = Speed;
            }

            if (this.Parent.Collider.Collision(Collider.Direction.Right))
            {
                CurrentSpeed = -Speed;
            }

            if (!Parent.Collider.Collision(Collider.Direction.Down))
            {
                if (!IsFalling)
                {
                    vertSpeed = 0;
                    IsFalling = true;
                }
                else
                {
                    if (vertSpeed < FloatSpeed)
                    {
                        vertSpeed += Engine.DeltaTime * Acceleration;
                    }
                    else
                    {
                        vertSpeed = FloatSpeed;
                    }
                }
            }
            else if (IsFalling)
            {
                vertSpeed = 0;
                IsFalling = false;
            }

            this.Parent.Transform.Position += new Vector3(CurrentSpeed * Engine.DeltaTime, vertSpeed * Engine.DeltaTime, 0);
        }

        private void DeadAnimation()
        {
            if (firstTimeDeadAnimation)
            {
                Target.Collider.Destroy();
            }
        }
    }
}