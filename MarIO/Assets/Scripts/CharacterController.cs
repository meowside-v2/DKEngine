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
        Animator PlayerAnimator;
        Mario Player;
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
        bool IsFacingLeft = false;
        bool EnemyKilledAnim = false;

        string IDLE
        {
            get
            {
                switch (Shared.MarioCurrentState)
                {
                    case Mario.State.Small:
                        return IsFacingLeft ? Shared.MARIO_IDLE_LEFT : Shared.MARIO_IDLE_RIGHT;
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
        string MOVE
        {
            get
            {
                switch (Shared.MarioCurrentState)
                {
                    case Mario.State.Small:
                        return horiSpeed >= 0 ? Shared.MARIO_MOVE_RIGHT : Shared.MARIO_MOVE_LEFT;
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
        string JUMP
        {
            get
            {
                switch (Shared.MarioCurrentState)
                {
                    case Mario.State.Small:
                        return horiSpeed != 0 ? (horiSpeed > 0 ? Shared.MARIO_JUMP_RIGHT : Shared.MARIO_JUMP_LEFT)
                                              : (IsFacingLeft  ? Shared.MARIO_JUMP_LEFT  : Shared.MARIO_JUMP_RIGHT);
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

            Player = GameObject.Find<Mario>("Player");
            PlayerAnimator = Component.Find<Animator>("Player_Animator");
            TargetCam = Component.Find<Camera>("Camera");
            TargetCam.Position = new Vector3(0, -180, 0);

            Player.Animator.Play("idle");
        }

        protected override void Update()
        {
            if (Player.KilledEnemy)
            {
                Player.KilledEnemy = false;
                EnemyKilledAnim = true;
                Jumped = true;
                IsFalling = false;
                vertSpeed = -FloatSpeed;
            }

            if (!Player.IsDestroyed)
            {
                Movement();
            }
            else
            {
                DeadAnimation();
            }
            
            Player.Transform.Position = Player.Transform.Position.Add(horiSpeed * Engine.DeltaTime, vertSpeed * Engine.DeltaTime, 0);
            
            CameraControl();
            AnimationControl();
        }

        private void DeadAnimation()
        {
            horiSpeed = 0;
            Player.Collider.Destroy();
        }

        private void CameraControl()
        {
            if (Player.Transform.Position.X - TargetCam.Position.X > MaxCameraDistance)
            {
                TargetCam.Position.X += Player.Transform.Position.X - PositionX;
            }

            if (Player.Transform.Position.X < TargetCam.Position.X)
            {
                Player.Transform.Position = Player.Transform.Position.Add(TargetCam.Position.X - Player.Transform.Position.X, 0, 0);
                horiSpeed = 0f;
            }

            PositionX = Player.Transform.Position.X;
        }

        private void Movement()
        {
            if (Player.Collider.Collision(Collider.Direction.Down))
            {
                IsFalling = false;
                Jumped = false;

                vertSpeed = 0;
            }


            if (Engine.Input.IsKeyDown(ConsoleKey.A))
            {
                IsFacingLeft = true;
                if (!Player.Collider.Collision(Collider.Direction.Left) && horiSpeed > -MovementSpeed)
                {
                    horiSpeed -= Engine.DeltaTime * Acceleration;
                }
                else if (Player.Collider.Collision(Collider.Direction.Left))
                {
                    horiSpeed = 0;
                }
                else
                {
                    horiSpeed = -MovementSpeed;
                }
            }
            else if (horiSpeed < 0)
            {
                IsFacingLeft = true;
                horiSpeed += Engine.DeltaTime * Acceleration * 2;

                if (horiSpeed >= 0 || Player.Collider.Collision(Collider.Direction.Left))
                {
                    horiSpeed = 0;
                }
            }

            if (Engine.Input.IsKeyDown(ConsoleKey.W))
            {
                Jump();
            }
            else if (Jumped)
            {
                if (EnemyKilledAnim)
                {
                    vertSpeed += Engine.DeltaTime * Acceleration * 4;

                    if(vertSpeed <= 0)
                    {
                        IsFalling = true;
                    }
                }
                else if (!IsFalling)
                {
                    vertSpeed = -vertSpeed;
                    IsFalling = true;
                }
            }

            if (Engine.Input.IsKeyDown(ConsoleKey.D))
            {
                IsFacingLeft = false;
                if (!Player.Collider.Collision(Collider.Direction.Right) && horiSpeed < MovementSpeed)
                {
                    horiSpeed += Engine.DeltaTime * Acceleration;
                }
                else if (Player.Collider.Collision(Collider.Direction.Right))
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
                IsFacingLeft = false;
                horiSpeed -= Engine.DeltaTime * Acceleration * 2;

                if (horiSpeed <= 0 || Player.Collider.Collision(Collider.Direction.Right))
                {
                    horiSpeed = 0;
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
                        vertSpeed += Engine.DeltaTime * Acceleration;
                    }
                    else
                    {
                        vertSpeed = FloatSpeed;
                    }
                }
            }
        }

        public void Jump()
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
                    else if (!Player.Collider.Collision(Collider.Direction.Up) && vertSpeed < 0)
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

        public void AnimationControl()
        {
            if (!Player.IsDestroyed)
            {
                if (Jumped)
                {
                    PlayerAnimator.Play(JUMP);
                }
                else
                {
                    if (horiSpeed != 0)
                        PlayerAnimator.Play(MOVE);

                    else
                        PlayerAnimator.Play(IDLE);
                }
            }
            else
            {
                PlayerAnimator.Play(Shared.MARIO_DEAD);
            }
        }
    }
}
