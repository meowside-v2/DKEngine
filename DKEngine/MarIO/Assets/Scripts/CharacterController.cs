using DKEngine.Core.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DKEngine.Core;
using DKEngine;
using DKEngine.Core.Components;
using MarIO.Assets.Models;

namespace MarIO.Assets.Scripts
{
    class CharacterController : Script
    {
        GameObject Player;
        Camera TargetCam;
        Vector3 Offset;

        float PositionX;
        float MaxCameraDistance;

        float horiSpeed = 0;
        float vertSpeed = 0;

        protected float MovementSpeed = 150f;
        protected float FloatSpeed = 120f;

        protected float Acceleration = 40f;

        protected bool CanJump = true;
        bool IsFalling = false;
        bool Jumped = false;

        string IDLE
        {
            get
            {
                switch (Shared.MarioCurrentState)
                {
                    case Mario.State.Small:
                        return "idle";
                    case Mario.State.Super:
                        return "idle";
                    case Mario.State.Fire:
                        return "idle";
                    case Mario.State.Invincible:
                        return "idle";
                    default:
                        throw new Exception("JAK");
                }
            }
        }
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
            MaxCameraDistance = Engine.Render.RenderWidth / 3;
            Offset = new Vector3(20, 0, 0);

            Player = GameObject.Find<GameObject>("Player");
            TargetCam = Component.Find<Camera>("Camera");
            TargetCam.Position = new Vector3(0, -180, 0);

            Player.Animator.Play("idle");
        }

        protected override void Update()
        {
            if (Player.Collider.Collision(Collider.Direction.Down))
            {
                IsFalling = false;
                Jumped = false;

                vertSpeed = 0;

                if (horiSpeed > 0)
                    Player.Animator.Play(RIGHTMOVE);

                else if (horiSpeed < 0)
                    Player.Animator.Play(LEFTMOVE);

                else
                    Player.Animator.Play(IDLE);
            }


            if (Engine.Input.IsKeyDown(ConsoleKey.A))
            {
                if (!Player.Collider.Collision(Collider.Direction.Left) && horiSpeed > -MovementSpeed)
                {
                    horiSpeed -= Engine.deltaTime * Acceleration;
                    if (Player.Animator.Current.Name != RIGHTJUMP && Player.Animator.Current.Name != LEFTJUMP)
                        Player.Animator.Play(LEFTMOVE);
                }
                else if (Player.Collider.Collision(Collider.Direction.Left))
                {
                    horiSpeed = 0;
                    if (Player.Animator.Current.Name != RIGHTJUMP && Player.Animator.Current.Name != LEFTJUMP)
                        Player.Animator.Play(IDLE);
                }
                else
                {
                    horiSpeed = -MovementSpeed;
                }
            }
            else if (horiSpeed < 0)
            {
                horiSpeed += Engine.deltaTime * Acceleration * 2;

                if (horiSpeed >= 0 || Player.Collider.Collision(Collider.Direction.Left))
                {
                    horiSpeed = 0;
                    if (Player.Animator.Current.Name != RIGHTJUMP && Player.Animator.Current.Name != LEFTJUMP)
                        Player.Animator.Play(IDLE);
                    
                }  
            }


            if (Engine.Input.IsKeyDown(ConsoleKey.D))
            {
                if (!Player.Collider.Collision(Collider.Direction.Right) && horiSpeed < MovementSpeed)
                {
                    horiSpeed += Engine.deltaTime * Acceleration;
                    if (Player.Animator.Current.Name != RIGHTJUMP && Player.Animator.Current.Name != LEFTJUMP)
                        Player.Animator.Play(RIGHTMOVE);
                }
                else if (Player.Collider.Collision(Collider.Direction.Right))
                {
                    horiSpeed = 0;
                    if (Player.Animator.Current.Name != RIGHTJUMP && Player.Animator.Current.Name != LEFTJUMP)
                        Player.Animator.Play(IDLE);
                }
                else
                {
                    horiSpeed = MovementSpeed;
                }
            }
            else if (horiSpeed > 0)
            {
                horiSpeed -= Engine.deltaTime * Acceleration * 2;

                if (horiSpeed <= 0 || Player.Collider.Collision(Collider.Direction.Right))
                {
                    horiSpeed = 0;
                    if (Player.Animator.Current.Name != RIGHTJUMP && Player.Animator.Current.Name != LEFTJUMP)
                        Player.Animator.Play(IDLE);
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
                                Player.Animator.Play(RIGHTJUMP);
                            }
                            else
                            {
                                Player.Animator.Play(LEFTJUMP);
                            }

                            vertSpeed = -FloatSpeed;
                            Jumped = true;
                        }
                        else if (!Player.Collider.Collision(Collider.Direction.Up) && vertSpeed < 0)
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

            if (!Player.Collider.Collision(Collider.Direction.Down))
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
            
            Player.Transform.Position += new Vector3(horiSpeed * Engine.deltaTime, vertSpeed * Engine.deltaTime, 0);

            if (Player.Transform.Position.X - TargetCam.Position.X > MaxCameraDistance)
            {
                TargetCam.Position.X += Player.Transform.Position.X - PositionX;
            }

            if (Player.Transform.Position.X < TargetCam.Position.X)
            {
                Player.Transform.Position += new Vector3(TargetCam.Position.X - Player.Transform.Position.X, 0, 0);
                horiSpeed = 0f;
                Player.Animator.Play(IDLE);
            }

            PositionX = Player.Transform.Position.X;
        }
    }
}
