﻿using DKEngine;
using DKEngine.Core;
using DKEngine.Core.Components;
using MarIO.Assets.Models;
using System;
using static DKEngine.Core.Components.Transform;

namespace MarIO.Assets.Scripts
{
    public class CharacterController : Script
    {
        private static Sound PIPE_ENTER_FX = new Sound(Shared.Assets.Sounds.PIPE_ENTER_FX);
        private static Sound JUMP_FX = new Sound(Shared.Assets.Sounds.MARIO_JUMP_FX);
        private static Sound STOMP_FX = new Sound(Shared.Assets.Sounds.STOMP_FX);

        private Animator PlayerAnimator;
        private Mario Player;
        //private SoundSource SoundOutput;

        private float horiSpeed = 0;
        private float vertSpeed = 0;

        private float MovementSpeed = 80f;
        private float FloatSpeed = 300f;

        private float Acceleration = 3.5f;

        private float DeathAnimSpeed = 80;

        private bool CanJump = true;
        private bool IsFalling = false;
        private bool Jumped = false;
        private bool IsFacingLeft = false;
        private bool EnemyKilledAnim = false;
        private bool FirstTimeDeadAnimPlay = true;

        private bool FirstTimePipeEnter = true;
        private float PipeEnterStartPosition;
        private float PipeEnterSpeed = 50f;
        
        private Mario.State LastState = Shared.Mechanics.MarioCurrentState;
        private bool ChangingState = false;

        private string _idle
        {
            get
            {
                switch (Shared.Mechanics.MarioCurrentState)
                {
                    case Mario.State.Small:
                        return IsFacingLeft ? Shared.Assets.Animations.MARIO_IDLE_LEFT : Shared.Assets.Animations.MARIO_IDLE_RIGHT;

                    case Mario.State.Super:
                        return IsFacingLeft ? Shared.Assets.Animations.MARIO_SUPER_IDLE_LEFT : Shared.Assets.Animations.MARIO_SUPER_IDLE_RIGHT;

                    case Mario.State.Fire:
                        return IsFacingLeft ? Shared.Assets.Animations.MARIO_FIRE_IDLE_LEFT : Shared.Assets.Animations.MARIO_FIRE_IDLE_RIGHT;

                    /*case Mario.State.Invincible:
                        return IsFacingLeft ? Shared.Assets.Animations.MARIO_INVINCIBLE_IDLE_LEFT : Shared.Assets.Animations.MARIO_INVINCIBLE_IDLE_RIGHT;*/

                    default:
                        throw new Exception("JAK");
                }
            }
        }
        private string _crouch
        {
            get
            {
                switch (Shared.Mechanics.MarioCurrentState)
                {
                    case Mario.State.Small:
                        return IsFacingLeft ? Shared.Assets.Animations.MARIO_CROUCHING_LEFT : Shared.Assets.Animations.MARIO_CROUCHING_RIGHT;

                    case Mario.State.Super:
                        return IsFacingLeft ? Shared.Assets.Animations.MARIO_SUPER_CROUCHING_LEFT : Shared.Assets.Animations.MARIO_SUPER_CROUCHING_RIGHT;

                    case Mario.State.Fire:
                        return IsFacingLeft ? Shared.Assets.Animations.MARIO_FIRE_CROUCHING_LEFT : Shared.Assets.Animations.MARIO_FIRE_CROUCHING_RIGHT;

                    /*case Mario.State.Invincible:
                        return IsFacingLeft ? Shared.Assets.Animations.MARIO_INVINCIBLE_IDLE_LEFT : Shared.Assets.Animations.MARIO_INVINCIBLE_IDLE_RIGHT;*/

                    default:
                        throw new Exception("JAK");
                }
            }
        }
        private string _superPowerUp
        {
            get { return IsFacingLeft ? Shared.Assets.Animations.MARIO_SUPER_POWERUP_LEFT : Shared.Assets.Animations.MARIO_SUPER_POWERUP_RIGHT; }
        }
        private string _firePowerUp
        {
            get { return IsFacingLeft ? Shared.Assets.Animations.MARIO_FIRE_POWERUP_LEFT : Shared.Assets.Animations.MARIO_FIRE_POWERUP_RIGHT; }
        }

        private string IDLE
        {
            get
            {
                return Player.CurrentMovement == Mario.Movement.Crouching ? _crouch : _idle;
            }
        }
        private string MOVE
        {
            get
            {
                switch (Shared.Mechanics.MarioCurrentState)
                {
                    case Mario.State.Small:
                        return horiSpeed >= 0 ? Shared.Assets.Animations.MARIO_MOVE_RIGHT : Shared.Assets.Animations.MARIO_MOVE_LEFT;

                    case Mario.State.Super:
                        return horiSpeed >= 0 ? Shared.Assets.Animations.MARIO_SUPER_MOVE_RIGHT : Shared.Assets.Animations.MARIO_SUPER_MOVE_LEFT;

                    case Mario.State.Fire:
                        return horiSpeed >= 0 ? Shared.Assets.Animations.MARIO_FIRE_MOVE_RIGHT : Shared.Assets.Animations.MARIO_FIRE_MOVE_LEFT;

                    /*case Mario.State.Invincible:
                        return horiSpeed >= 0 ? Shared.Assets.Animations.MARIO_INVINCIBLE_MOVE_RIGHT : Shared.Assets.Animations.MARIO_INVINCIBLE_MOVE_LEFT;*/

                    default:
                        throw new Exception("JAK");
                }
            }
        }
        private string JUMP
        {
            get
            {
                switch (Shared.Mechanics.MarioCurrentState)
                {
                    case Mario.State.Small:
                        return horiSpeed != 0 ? (horiSpeed > 0 ? Shared.Assets.Animations.MARIO_JUMP_RIGHT : Shared.Assets.Animations.MARIO_JUMP_LEFT)
                                              : (IsFacingLeft ? Shared.Assets.Animations.MARIO_JUMP_LEFT : Shared.Assets.Animations.MARIO_JUMP_RIGHT);

                    case Mario.State.Super:
                        return horiSpeed != 0 ? (horiSpeed > 0 ? Shared.Assets.Animations.MARIO_SUPER_JUMP_RIGHT : Shared.Assets.Animations.MARIO_SUPER_JUMP_LEFT)
                                              : (IsFacingLeft ? Shared.Assets.Animations.MARIO_SUPER_JUMP_LEFT : Shared.Assets.Animations.MARIO_SUPER_JUMP_RIGHT);

                    case Mario.State.Fire:
                        return horiSpeed != 0 ? (horiSpeed > 0 ? Shared.Assets.Animations.MARIO_FIRE_JUMP_RIGHT : Shared.Assets.Animations.MARIO_FIRE_JUMP_LEFT)
                                              : (IsFacingLeft ? Shared.Assets.Animations.MARIO_FIRE_JUMP_LEFT : Shared.Assets.Animations.MARIO_FIRE_JUMP_RIGHT);

                    /*case Mario.State.Invincible:
                        return horiSpeed != 0 ? (horiSpeed > 0 ? Shared.Assets.Animations.MARIO_INVINCIBLE_JUMP_RIGHT : Shared.Assets.Animations.MARIO_INVINCIBLE_JUMP_LEFT)
                                              : (IsFacingLeft ? Shared.Assets.Animations.MARIO_INVINCIBLE_JUMP_LEFT : Shared.Assets.Animations.MARIO_INVINCIBLE_JUMP_RIGHT);*/

                    default:
                        throw new Exception("JAK");
                }
            }
        }
        private string POWERUP
        {
            get
            {
                switch (LastState)
                {
                    case Mario.State.Small:
                        return _superPowerUp;
                    case Mario.State.Super:
                        return LastState < Shared.Mechanics.MarioCurrentState ? _firePowerUp : _superPowerUp;
                    case Mario.State.Fire:
                        return LastState < Shared.Mechanics.MarioCurrentState ? "" : _firePowerUp;
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
            Player = GameObject.Find<Mario>("Player");
            PlayerAnimator = Component.Find<Animator>("Player_Animator");
            //SoundOutput = Component.Find<SoundSource>("Player_SoundSource");

            Player.Animator.Play("idle");
        }

        protected override void Update()
        {
            if(LastState != Shared.Mechanics.MarioCurrentState)
            {
                if (!ChangingState)
                {
                    PlayerAnimator.Play(POWERUP);
                    float YtoAdd = Shared.Mechanics.MarioCurrentState > Mario.State.Small ? 16 : -16;
                    Player.Transform.Position += new Vector3(0, YtoAdd, 0);
                    ChangingState = true;

                    Player.LeftTrigger.Collider.Enabled = false;
                    Player.RightTrigger.Collider.Enabled = false;
                    Player.TopTrigger.Collider.Enabled = false;
                    Player.BottomTrigger.Collider.Enabled = false;

                    Player.Collider.Enabled = false;

                    return;
                }

                if(PlayerAnimator.NumberOfPlays > 5)
                {
                    LastState = Shared.Mechanics.MarioCurrentState;

                    Player.LeftTrigger.Collider.Enabled = true;
                    Player.RightTrigger.Collider.Enabled = true;
                    Player.TopTrigger.Collider.Enabled = true;
                    Player.BottomTrigger.Collider.Enabled = true;

                    Player.Collider.Enabled = true;
                }
            }

            if (Player.KilledEnemy)
            {
                Shared.Mechanics.FXSoundSource.PlaySound(STOMP_FX);
                Player.KilledEnemy = false;
                EnemyKilledAnim = true;
                Jumped = true;
                IsFalling = false;
                vertSpeed = -FloatSpeed;
            }

            if (Player.ChangeState)
            {
                if (FirstTimePipeEnter)
                {
                    Shared.Mechanics.FXSoundSource.PlaySound(PIPE_ENTER_FX);
                    Player.Collider.Enabled = false;
                    PipeEnterStartPosition = Player.PipeEnteredInDirection == Direction.Down ? Player.Transform.Position.Y : Player.Transform.Position.X;
                    horiSpeed = 0;
                    vertSpeed = 0;
                    FirstTimePipeEnter = false;
                }

                if (Player.PipeEnteredInDirection == Direction.Right)
                {
                    if (Player.Transform.Position.X < PipeEnterStartPosition + 16)
                    {
                        horiSpeed = PipeEnterSpeed;
                    }
                    else
                    {
                        Player.WorldManager.CurrentlyEnteredPipeScript = Player.EnteredPipe;
                    }
                }
                else if (Player.PipeEnteredInDirection == Direction.Down)
                {
                    if (Player.Transform.Position.Y < PipeEnterStartPosition + 16)
                    {
                        vertSpeed = PipeEnterSpeed;
                    }
                    else
                    {
                        Player.WorldManager.CurrentlyEnteredPipeScript = Player.EnteredPipe;
                    }
                }
            }
            else if (!Player.IsDestroyed)
            {
                Movement();
            }
            else
            {
                DeadAnimation();
            }

            Player.Transform.Position = Player.Transform.Position.Add(horiSpeed * Engine.DeltaTime, vertSpeed * Engine.DeltaTime, 0);

            //CameraControl();
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

                vertSpeed = -FloatSpeed;

                FirstTimeDeadAnimPlay = false;
            }
            else
            {
                vertSpeed += Engine.DeltaTime * DeathAnimSpeed * Acceleration;
            }
        }

        private void Movement()
        {
            if (Player.Collider.Collision(Direction.Down))
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

            if (Engine.Input.IsKeyDown(ConsoleKey.S))
            {
                Down();
            }

            if (!Player.Collider.Collision(Direction.Down))
            {
                Fall();
            }
        }

        private void Jump()
        {
            if (Engine.Input.IsKeyDown(ConsoleKey.W))
            {
                Player.CurrentMovement = Mario.Movement.Standing;

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
                            Shared.Mechanics.FXSoundSource.PlaySound(JUMP_FX);
                            vertSpeed = -FloatSpeed * 1.5f;
                            Jumped = true;
                        }
                        else if (!Player.Collider.Collision(Direction.Up) && vertSpeed < 0)
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
                    vertSpeed = -vertSpeed;
                    IsFalling = true;
                    EnemyKilledAnim = false;
                }
            }
        }

        private void Left()
        {
            if (Engine.Input.IsKeyDown(ConsoleKey.A))
            {
                Player.CurrentMovement = Mario.Movement.Standing;

                IsFacingLeft = true;
                if (!Player.Collider.Collision(Direction.Left) && horiSpeed > -MovementSpeed)
                {
                    horiSpeed -= Engine.DeltaTime * Acceleration * MovementSpeed;
                }
                else if (Player.Collider.Collision(Direction.Left))
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

                if (horiSpeed >= 0 || Player.Collider.Collision(Direction.Left))
                {
                    horiSpeed = 0;
                }
            }
        }

        private void Right()
        {
            if (Engine.Input.IsKeyDown(ConsoleKey.D))
            {
                Player.CurrentMovement = Mario.Movement.Standing;

                IsFacingLeft = false;
                if (!Player.Collider.Collision(Direction.Right) && horiSpeed < MovementSpeed)
                {
                    horiSpeed += Engine.DeltaTime * Acceleration * MovementSpeed;
                }
                else if (Player.Collider.Collision(Direction.Right))
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

                if (horiSpeed <= 0 || Player.Collider.Collision(Direction.Right))
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

        private void Down()
        {
            if (vertSpeed == 0)
            {
                horiSpeed = 0;
                Player.CurrentMovement = Mario.Movement.Crouching;
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
                PlayerAnimator.Play(Shared.Assets.Animations.MARIO_DEAD);
            }
        }
    }
}