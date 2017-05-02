using DKEngine;
using DKEngine.Core;
using DKEngine.Core.Components;
using MarIO.Assets.Models;
using System;

namespace MarIO.Assets.Scripts
{
    internal class CharacterController : Script
    {
        private Animator PlayerAnimator;
        private Mario Player;
        private Camera TargetCam;
        private Vector3 Offset;

        //Parabola test;

        private float PositionX;
        private float MaxCameraDistance;

        private float horiSpeed = 0;
        private float vertSpeed = 0;

        private float MovementSpeed = 80f;
        private float FloatSpeed = 120f;

        private float Acceleration = 2f;

        private float DeathAnimSpeed = 50;

        private bool CanJump = true;
        private bool IsFalling = false;
        private bool Jumped = false;
        private bool IsFacingLeft = false;
        private bool EnemyKilledAnim = false;
        private bool FirstTimeDeadAnimPlay = true;
        //bool

        private string IDLE
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

        private string MOVE
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

        private string JUMP
        {
            get
            {
                switch (Shared.MarioCurrentState)
                {
                    case Mario.State.Small:
                        return horiSpeed != 0 ? (horiSpeed > 0 ? Shared.MARIO_JUMP_RIGHT : Shared.MARIO_JUMP_LEFT)
                                              : (IsFacingLeft ? Shared.MARIO_JUMP_LEFT : Shared.MARIO_JUMP_RIGHT);

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

            /*test = new Parabola(Player)
            {
                Y = FloatSpeed,
                Time = new TimeSpan(0, 0, 1)
            };*/

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

            if (FirstTimeDeadAnimPlay)
            {
                Player.Collider.Enabled = false;
                Player.BottomTrigger.Collider.Enabled = false;
                Player.LeftTrigger.Collider.Enabled = false;
                Player.RightTrigger.Collider.Enabled = false;
                Player.TopTrigger.Collider.Enabled = false;

                /*Player.Collider.Destroy();

                foreach (GameObject child in Player.Child)
                {
                    child.Collider.Destroy();
                }*/

                vertSpeed = -FloatSpeed;

                FirstTimeDeadAnimPlay = false;
            }
            else
            {
                vertSpeed += Engine.DeltaTime * DeathAnimSpeed * Acceleration;
            }
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

            if (Engine.Input.IsKeyDown(ConsoleKey.A) || horiSpeed < 0)
            {
                Left();
            }

            if (Engine.Input.IsKeyDown(ConsoleKey.W) || Jumped)
            {
                Jump();
            }

            if (Engine.Input.IsKeyDown(ConsoleKey.D) || horiSpeed > 0)
            {
                Right();
            }

            if (!Player.Collider.Collision(Collider.Direction.Down))
            {
                Fall();
            }
        }

        private void Jump()
        {
            if (Engine.Input.IsKeyDown(ConsoleKey.W))
            {
                if (CanJump)
                {
                    if (EnemyKilledAnim)
                    {
                        vertSpeed += Engine.DeltaTime * Acceleration * FloatSpeed * 2;

                        if (vertSpeed <= 0)
                        {
                            IsFalling = true;
                            EnemyKilledAnim = false;
                        }
                    }
                    else if (!IsFalling)
                    {
                        if (vertSpeed == 0 && !Jumped)
                        {
                            vertSpeed = -FloatSpeed * 1.5f;
                            Jumped = true;
                        }
                        else if (!Player.Collider.Collision(Collider.Direction.Up) && vertSpeed < 0)
                        {
                            vertSpeed += Engine.DeltaTime * Acceleration * FloatSpeed;
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
                if (EnemyKilledAnim)
                {
                    vertSpeed += Engine.DeltaTime * Acceleration * FloatSpeed * 4;

                    if (vertSpeed <= 0)
                    {
                        IsFalling = true;
                        EnemyKilledAnim = false;
                    }
                }
                else if (!IsFalling)
                {
                    vertSpeed = -vertSpeed * Acceleration;
                    IsFalling = true;
                    EnemyKilledAnim = false;
                }
            }
        }

        private void Left()
        {
            if (Engine.Input.IsKeyDown(ConsoleKey.A))
            {
                IsFacingLeft = true;
                if (!Player.Collider.Collision(Collider.Direction.Left) && horiSpeed > -MovementSpeed)
                {
                    horiSpeed -= Engine.DeltaTime * Acceleration * MovementSpeed;
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
                horiSpeed += Engine.DeltaTime * Acceleration * MovementSpeed * 4;

                if (horiSpeed >= 0 || Player.Collider.Collision(Collider.Direction.Left))
                {
                    horiSpeed = 0;
                }
            }
        }

        private void Right()
        {
            if (Engine.Input.IsKeyDown(ConsoleKey.D))
            {
                IsFacingLeft = false;
                if (!Player.Collider.Collision(Collider.Direction.Right) && horiSpeed < MovementSpeed)
                {
                    horiSpeed += Engine.DeltaTime * Acceleration * MovementSpeed;
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
                horiSpeed -= Engine.DeltaTime * Acceleration * MovementSpeed * 2;

                if (horiSpeed <= 0 || Player.Collider.Collision(Collider.Direction.Right))
                {
                    horiSpeed = 0;
                }
            }
        }

        private void Fall()
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
                    vertSpeed += Engine.DeltaTime * Acceleration * FloatSpeed;
                }
                else
                {
                    vertSpeed = FloatSpeed;
                }
            }
        }

        private void AnimationControl()
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