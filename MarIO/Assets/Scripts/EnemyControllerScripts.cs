using DKEngine;
using DKEngine.Core;
using DKEngine.Core.Components;
using DKEngine.Core.UI;
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

        private float DeadTimeCurrent = 0f;
        private const float DeadTime = 3f;

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
            if (Target.Collider.Collision(Collider.Direction.Left))
            {
                CurrentSpeed = Speed;
            }

            if (Target.Collider.Collision(Collider.Direction.Right))
            {
                CurrentSpeed = -Speed;
            }

            if (!Target.Collider.Collision(Collider.Direction.Down))
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

            Target.Transform.Position += new Vector3(CurrentSpeed * Engine.DeltaTime, vertSpeed * Engine.DeltaTime, 0);
        }

        private void DeadAnimation()
        {
            if (firstTimeDeadAnimation)
            {
                Shared.GameScore += Shared.GOOMBA_POINTS;
                TextBlock FloatingText = new TextBlock()
                {
                    Text = string.Format("{0}", Shared.GOOMBA_POINTS),
                    TextShadow = true
                };
                FloatingText.Transform.Position = Target.Transform.Position;
                FloatingText.AddAsFloatingText();

                Target.Collider.Enabled = false;
                Target.Animator.Play("dead");
                firstTimeDeadAnimation = false;
                Target.Transform.Position += new Vector3(0, 8, 0);

                
            }

            DeadTimeCurrent += Engine.DeltaTime;

            if(DeadTimeCurrent > DeadTime)
            {
                Target.Destroy();
            }
        }
    }
}