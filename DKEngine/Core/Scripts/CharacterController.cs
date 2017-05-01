using DKEngine.Core.Components;
using System;

namespace DKEngine.Core.Scripts
{
    public class CharacterController : Script
    {
        private float horiSpeed = 0;
        private float vertSpeed = 0;

        protected float MovementSpeed = 1;
        protected float FloatSpeed = 1;

        protected float Acceleration = 1;

        protected bool CanJump = true;
        private bool IsFalling = false;
        private bool Jumped = false;

        private bool Landed = false;
        private bool CollisionLeft = false;
        private bool CollisionRight = false;
        private bool CollisionTop = false;

        public CharacterController(GameObject Parent)
            : base(Parent)
        {
            this.Parent.InitNewComponent<Collider>();
            this.Parent.InitNewComponent<Animator>();
        }

        protected internal override void OnColliderEnter(Collider e)
        {
        }

        protected internal override void Start()
        {
            //throw new NotImplementedException();
        }

        protected internal override void Update()
        {
            if (Landed = Parent.Collider.Collision(Collider.Direction.Down))
            {
                IsFalling = false;
                Jumped = false;

                vertSpeed = 0;
            }

            if (Engine.Input.IsKeyDown(ConsoleKey.A))
            {
                if (!(CollisionLeft = Parent.Collider.Collision(Collider.Direction.Left)) && horiSpeed > -MovementSpeed)
                {
                    horiSpeed -= Engine.DeltaTime * Acceleration;
                }
                else if (CollisionLeft)
                {
                    horiSpeed = 0;
                }
                else
                {
                    horiSpeed = -MovementSpeed;
                }
            }
            else if (CollisionLeft = Parent.Collider.Collision(Collider.Direction.Left))
            {
                horiSpeed = 0;
            }
            else if (horiSpeed < 0)
            {
                horiSpeed += Engine.DeltaTime * Acceleration * 2;

                if (horiSpeed > 0)
                    horiSpeed = 0;
            }

            if (Engine.Input.IsKeyDown(ConsoleKey.D))
            {
                if (!(CollisionRight = Parent.Collider.Collision(Collider.Direction.Right)) && horiSpeed < MovementSpeed)
                {
                    horiSpeed += Engine.DeltaTime * Acceleration;
                }
                else if (CollisionRight)
                {
                    horiSpeed = 0;
                }
                else
                {
                    horiSpeed = MovementSpeed;
                }
            }
            else if (horiSpeed > 0)
            {
                horiSpeed -= Engine.DeltaTime * Acceleration * 2;

                if (horiSpeed < 0)
                    horiSpeed = 0;
            }
            else if (CollisionRight = Parent.Collider.Collision(Collider.Direction.Right))
            {
                horiSpeed = 0;
            }

            if (Engine.Input.IsKeyDown(ConsoleKey.W))
            {
                if (CanJump)
                {
                    if (!IsFalling)
                    {
                        if (vertSpeed == 0 && !Jumped)
                        {
                            vertSpeed = -FloatSpeed;
                            Jumped = true;
                        }
                        else if (!(CollisionTop = Parent.Collider.Collision(Collider.Direction.Up)))
                        {
                            vertSpeed += Engine.DeltaTime * Acceleration * 2;
                        }
                        else
                        {
                            vertSpeed = 0;
                            IsFalling = true;
                        }
                    }
                }
            }
            else if (Jumped)
            {
                /*if ((CollisionTop = Parent.Collider.Collision(Collider.Direction.Up)) && !IsFalling)
                {*/
                if (!IsFalling)
                {
                    vertSpeed = -vertSpeed;
                    IsFalling = true;
                }
                /*}
                else if (!CollisionTop)
                {
                    vertSpeed += Engine.deltaTime * Acceleration * 2;
                }*/
            }

            if (!Landed)
            {
                if (!IsFalling && !Jumped)
                {
                    vertSpeed = 0;
                    Jumped = true;
                    IsFalling = true;
                }
                else if (IsFalling)
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

            this.Parent.Transform.Position += new Vector3(/*0, 1, 0*/ horiSpeed * Engine.DeltaTime, vertSpeed * Engine.DeltaTime, 0);
        }
    }
}