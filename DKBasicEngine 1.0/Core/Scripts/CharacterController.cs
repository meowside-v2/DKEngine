using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DKEngine.Core.Components;

namespace DKEngine.Core.Scripts
{
    public class CharacterController : Script
    {
        float horiSpeed = 0;
        float vertSpeed = 0;

        protected float MovementSpeed = 1;
        protected float FloatSpeed = 1;

        protected float Acceleration = 1;

        protected bool CanJump = true;
        bool IsFalling = false;
        bool Jumped = false;

        bool Landed = false;
        bool CollisionLeft = false;
        bool CollisionRight = false;
        bool CollisionTop = false;

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
                    horiSpeed -= Engine.deltaTime * Acceleration;
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
            else if(horiSpeed < 0)
            {
                horiSpeed += Engine.deltaTime * Acceleration * 2;

                if (horiSpeed > 0)
                    horiSpeed = 0;
            }


            if (Engine.Input.IsKeyDown(ConsoleKey.D))
            {
                if (!(CollisionRight = Parent.Collider.Collision(Collider.Direction.Right)) && horiSpeed < MovementSpeed)
                {
                    horiSpeed += Engine.deltaTime * Acceleration;
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
                horiSpeed -= Engine.deltaTime * Acceleration * 2;

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
                            vertSpeed += Engine.deltaTime * Acceleration * 2;
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
                        vertSpeed += Engine.deltaTime * Acceleration;
                    }
                    else
                    {
                        vertSpeed = FloatSpeed;
                    }
                }
            }

            this.Parent.Transform.Position += new Vector3(/*0, 1, 0*/ horiSpeed * Engine.deltaTime, vertSpeed * Engine.deltaTime, 0);
        }
    }
}
