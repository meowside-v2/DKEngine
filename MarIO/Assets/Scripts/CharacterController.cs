using DKEngine;
using DKEngine.Core;
using DKEngine.Core.Components;
using MarIO.Assets.Models;
using System;
using static DKEngine.Core.Components.Transform;

namespace MarIO.Assets.Scripts
{
    public class CharacterController : Script
    {
        private Animator PlayerAnimator;
        private Mario Player;
        //private SoundSource SoundOutput;

        private float horiSpeed = 0;
        private float vertSpeed = 0;

        private const float MovementSpeed = 80f;
        private const float FloatSpeed = 300f;

        private const float Acceleration = 3.5f;

        private const float DeathAnimSpeed = 120f;

        private bool CanJump = true;
        private bool IsFalling = false;
        private bool Jumped = false;
        private bool IsFacingLeft = false;
        private bool EnemyKilledAnim = false;
        private bool FirstTimeDeadAnimPlay = true;

        private bool FirstTimePipeEnter = true;
        private float PipeEnterStartPosition;
        private float PipeEnterSpeed = 50f;
        
        private Mario.State LastState;
        private bool ChangingState = false;

        private string _idle
        {
            get
            {
                switch (Player.CurrentState)
                {
                    case Mario.State.Dead:
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
                switch (Player.CurrentState)
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
                switch (Player.CurrentState)
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
                switch (Player.CurrentState)
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
                        return LastState < Player.CurrentState ? _firePowerUp : _superPowerUp;
                    case Mario.State.Fire:
                        return LastState < Player.CurrentState ? "" : _firePowerUp;
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

            LastState = Player.CurrentState;

            Player.Animator.Play(Shared.Assets.Animations.MARIO_IDLE_RIGHT);
        }

        protected override void Update()
        {
            if(LastState != Player.CurrentState && Player.CurrentState != Mario.State.Dead)
            {
                if (!ChangingState)
                {
                    PlayerAnimator.Play(POWERUP);
                    Shared.Mechanics.FXSoundSource.PlaySound(Shared.Assets.Sounds.FX_1_UP_SOUND);
                    /*float YtoAdd = Player.CurrentState > Mario.State.Small ? -16 : 16;
                    Player.Transform.Position += new Vector3(0, YtoAdd, 0);*/
                    ChangingState = true;

                    Player.LeftTrigger.Collider.Enabled = false;
                    Player.RightTrigger.Collider.Enabled = false;
                    Player.TopTrigger.Collider.Enabled = false;
                    Player.BottomTrigger.Collider.Enabled = false;

                    Player.Collider.Enabled = false;

                    return;
                }

                if (PlayerAnimator.NumberOfPlays > 5)
                {
                    LastState = Player.CurrentState;

                    Player.LeftTrigger.Collider.Enabled = true;
                    Player.RightTrigger.Collider.Enabled = true;
                    Player.TopTrigger.Collider.Enabled = true;
                    Player.BottomTrigger.Collider.Enabled = true;

                    Player.Collider.Enabled = true;

                    ChangingState = false;
                }

                else
                    return;
            }

            else if (Player.KilledEnemy)
            {
                Shared.Mechanics.FXSoundSource.PlaySound(Shared.Assets.Sounds.FX_STOMP_SOUND);
                Player.KilledEnemy = false;
                EnemyKilledAnim = true;
                Jumped = true;
                IsFalling = false;
                vertSpeed = -FloatSpeed;
            }

            else if (Player.ChangeState)
            {
                if (FirstTimePipeEnter)
                {
                    Shared.Mechanics.FXSoundSource.PlaySound(Shared.Assets.Sounds.FX_PIPE_ENTER_SOUND);
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
            else if (Player.CurrentState > Mario.State.Dead)
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

                Shared.Mechanics.FXSoundSource.PlaySound(Shared.Assets.Sounds.FX_MARIO_DIE_SOUND);
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
                if (vertSpeed == 0)
                {
                    horiSpeed = 0;
                    Player.CurrentMovement = Mario.Movement.Crouching;
                }
            }
            else
            {
                Player.CurrentMovement = Mario.Movement.Standing;
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
                            Shared.Mechanics.FXSoundSource.PlaySound(Shared.Assets.Sounds.FX_MARIO_JUMP_SOUND);
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

        private void AnimationControl()
        {
            if (Player.CurrentState > Mario.State.Dead)
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