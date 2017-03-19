using DKEngine.Core.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DKEngine.Core;
using DKEngine;
using DKEngine.Core.Components;

namespace MarIO.Assets.Scripts
{
    class CharacterController : Script
    {
        float horiSpeed = 0;
        float vertSpeed = 0;

        protected float MovementSpeed = 100f;
        protected float FloatSpeed = 50f;

        protected float Acceleration = 20f;

        protected bool CanJump = true;
        bool IsFalling = false;
        bool Jumped = false;

        const string IDLE = "idle";
        const string RIGHTMOVE = "right_move";
        const string LEFTMOVE = "left_move";
        const string RIGHTJUMP = "right_jump";
        const string LEFTJUMP = "left_jump";

        public CharacterController(GameObject Parent)
            : base(Parent)
        {
            this.Parent.InitNewComponent<Collider>();
        }

        protected override void OnColliderEnter(Collider e)
        { }

        protected override void Start()
        {
            Parent.Animator.Play("idle");
        }

        protected override void Update()
        {
            if (Parent.Collider.Collision(Collider.Direction.Down))
            {
                IsFalling = false;
                Jumped = false;

                vertSpeed = 0;

                if (horiSpeed > 0)
                    Parent.Animator.Play(RIGHTMOVE);

                else if (horiSpeed < 0)
                    Parent.Animator.Play(LEFTMOVE);

                else
                    Parent.Animator.Play(IDLE);
            }


            if (Engine.Input.IsKeyDown(ConsoleKey.A))
            {
                if (!Parent.Collider.Collision(Collider.Direction.Left) && horiSpeed > -MovementSpeed)
                {
                    horiSpeed -= Engine.deltaTime * Acceleration;
                    if (Parent.Animator.Current.Name != RIGHTJUMP && Parent.Animator.Current.Name != LEFTJUMP)
                        Parent.Animator.Play(LEFTMOVE);
                }
                else if (Parent.Collider.Collision(Collider.Direction.Left))
                {
                    horiSpeed = 0;
                    if (Parent.Animator.Current.Name != RIGHTJUMP && Parent.Animator.Current.Name != LEFTJUMP)
                        Parent.Animator.Play(IDLE);
                }
                else
                {
                    horiSpeed = -MovementSpeed;
                }
            }
            else if (horiSpeed < 0)
            {
                horiSpeed += Engine.deltaTime * Acceleration * 2;

                if (horiSpeed > 0 || Parent.Collider.Collision(Collider.Direction.Left))
                {
                    horiSpeed = 0;
                    if (Parent.Animator.Current.Name != RIGHTJUMP && Parent.Animator.Current.Name != LEFTJUMP)
                        Parent.Animator.Play(IDLE);
                    
                }  
            }


            if (Engine.Input.IsKeyDown(ConsoleKey.D))
            {
                if (!Parent.Collider.Collision(Collider.Direction.Right) && horiSpeed < MovementSpeed)
                {
                    horiSpeed += Engine.deltaTime * Acceleration;
                    if (Parent.Animator.Current.Name != RIGHTJUMP && Parent.Animator.Current.Name != LEFTJUMP)
                        Parent.Animator.Play(RIGHTMOVE);
                }
                else if (Parent.Collider.Collision(Collider.Direction.Right))
                {
                    horiSpeed = 0;
                    if (Parent.Animator.Current.Name != RIGHTJUMP && Parent.Animator.Current.Name != LEFTJUMP)
                        Parent.Animator.Play(IDLE);
                }
                else
                {
                    horiSpeed = MovementSpeed;
                }
            }
            else if (horiSpeed > 0)
            {
                horiSpeed -= Engine.deltaTime * Acceleration * 2;

                if (horiSpeed < 0 || Parent.Collider.Collision(Collider.Direction.Right))
                {
                    horiSpeed = 0;
                    if (Parent.Animator.Current.Name != RIGHTJUMP && Parent.Animator.Current.Name != LEFTJUMP)
                        Parent.Animator.Play(IDLE);
                }
            }

            if (Engine.Input.IsKeyDown(ConsoleKey.W))
            {
                if (CanJump)
                {
                    if (!IsFalling)
                    {
                        if (vertSpeed == 0 && !Jumped)
                        {
                            if(horiSpeed >= 0)
                            {
                                Parent.Animator.Play(RIGHTJUMP);
                            }
                            else
                            {
                                Parent.Animator.Play(LEFTJUMP);
                            }

                            vertSpeed = -FloatSpeed;
                            Jumped = true;
                        }
                        else if (!Parent.Collider.Collision(Collider.Direction.Up))
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
                if (!IsFalling)
                {
                    vertSpeed = -vertSpeed;
                    IsFalling = true;
                }
            }

            if (!Parent.Collider.Collision(Collider.Direction.Down))
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
            
            this.Parent.Transform.Position += new Vector3(horiSpeed * Engine.deltaTime, vertSpeed * Engine.deltaTime, 0);
        }
    }
}
