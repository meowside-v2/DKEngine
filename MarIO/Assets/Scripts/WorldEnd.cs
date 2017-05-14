using DKEngine;
using DKEngine.Core;
using DKEngine.Core.Components;
using MarIO.Assets.Models;
using MarIO.Assets.Scenes;
using System;
using static DKEngine.Core.Components.Transform;

namespace MarIO.Assets.Scripts
{
    internal class WorldEnd : Script
    {
        private Mario Player;
        private CharacterController PlayerController;
        private Animator PlayerAnimator;

        private float horiSpeed = 0;
        private float vertSpeed = 0;
        private float Distance = 180;
        private float startX;

        private const float MovementSpeed = 80f;
        private const float FloatSpeed = 300f;

        private const float Acceleration = 3.5f;

        private readonly TimeSpan _delay = new TimeSpan(0, 0, 3);
        private TimeSpan Delay = new TimeSpan();

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

        private string IDLE
        {
            get
            {
                switch (Player.CurrentState)
                {
                    case Mario.State.Dead:
                    case Mario.State.Small:
                        return horiSpeed < 0 ? Shared.Assets.Animations.MARIO_IDLE_LEFT : Shared.Assets.Animations.MARIO_IDLE_RIGHT;

                    case Mario.State.Super:
                        return horiSpeed < 0 ? Shared.Assets.Animations.MARIO_SUPER_IDLE_LEFT : Shared.Assets.Animations.MARIO_SUPER_IDLE_RIGHT;

                    case Mario.State.Fire:
                        return horiSpeed < 0 ? Shared.Assets.Animations.MARIO_FIRE_IDLE_LEFT : Shared.Assets.Animations.MARIO_FIRE_IDLE_RIGHT;

                    /*case Mario.State.Invincible:
                        return IsFacingLeft ? Shared.Assets.Animations.MARIO_INVINCIBLE_IDLE_LEFT : Shared.Assets.Animations.MARIO_INVINCIBLE_IDLE_RIGHT;*/

                    default:
                        throw new Exception("JAK");
                }
            }
        }

        public WorldEnd(GameObject Parent) : base(Parent)
        {
            startX = Parent.Transform.Position.X;
        }

        protected override void OnColliderEnter(Collider e)
        {
            if (e.Parent is Mario)
                PlayerController.Enabled = false;
        }

        protected override void Start()
        {
            Player = GameObject.Find<Mario>("Player");
            PlayerAnimator = Component.Find<Animator>("Player_Animator");
            PlayerController = Script.Find<CharacterController>(nameof(CharacterController));
        }

        protected override void Update()
        {
            if (Shared.Mechanics.TimeLeft.TotalSeconds <= 0)
            {
                Player.CurrentState = Mario.State.Dead;
                Shared.Mechanics.TimeCounter.Stop();
            }

            if (!PlayerController.Enabled)
            {
                PlayerAnimator.Play(MOVE);

                if (!Player.Collider.Collision(Direction.Down))
                {
                    if (vertSpeed < FloatSpeed)
                    {
                        vertSpeed += Engine.DeltaTime * Acceleration * FloatSpeed;

                        if (vertSpeed > FloatSpeed)
                            vertSpeed = FloatSpeed;
                    }
                    else
                    {
                        vertSpeed = FloatSpeed;
                    }
                }

                if (Player.Transform.Position.X > startX + Distance)
                {
                    PlayerAnimator.Play(IDLE);
                    Delay += new TimeSpan(0, 0, 0, 0, (int)(Engine.DeltaTime * 1000));
                    if (_delay < Delay)
                    {
                        Shared.Mechanics.FXSoundSource.StopSound(Shared.Assets.Sounds.OVERWORLD_THEME_SOUND);
                        Engine.ChangeScene(nameof(GameOver), true);
                    }
                }
                else
                {
                    this.Player.Transform.Position += new Vector3(MovementSpeed * Engine.DeltaTime, vertSpeed, 0);
                }
            }
        }
    }
}