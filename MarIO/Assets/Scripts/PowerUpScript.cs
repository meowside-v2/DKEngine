using DKEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DKEngine.Core.Components;
using MarIO.Assets.Models.Miscellaneous;
using MarIO.Assets.Models;
using DKEngine;
using static DKEngine.Core.Components.Transform;

namespace MarIO.Assets.Scripts
{
    class PowerUpScript : Script
    {
        PowerUp Target;
        bool CreatedForFirstTime = true;
        bool CreatedAnimation = true;
        float CreatedStartY;
        const float CreationAnimationSpeed = 20f;

        private const int Speed = 20;
        private const int FloatSpeed = 60;
        private const int Acceleration = 20;

        private int CurrentSpeed = 0;
        private float vertSpeed = 0;
        private bool IsFalling = false;
        private bool Jumped = false;

        public PowerUpScript(GameObject Parent) : base(Parent)
        {
            Target = Parent as PowerUp;
        }

        protected override void OnColliderEnter(Collider e)
        { }

        protected override void Start()
        {
            CurrentSpeed = -Speed;

            Target.PlayerReference = GameObject.Find<Mario>("Player");
        }

        protected override void Update()
        {
            if (CreatedForFirstTime)
            {
                CreatedStartY = Target.Transform.Position.Y;
                CreatedForFirstTime = false;
                return;
            }

            else if (CreatedAnimation)
            {
                if(CreatedStartY < Target.Transform.Position.Y)
                {
                    Target.Transform.Position += new Vector3(0, Engine.DeltaTime * CreationAnimationSpeed, 0);
                }
                else
                {
                    Target.Transform.Position = new Vector3(Target.Transform.Position.X, CreatedStartY + 16, Target.Transform.Position.Z);
                    CreatedAnimation = false;
                }

                return;
            }

            else
            {
                switch (Target.Type)
                {
                    case PowerUp.PowerUpType.Mushroom:
                        MushroomMovement();
                        break;
                    case PowerUp.PowerUpType.Flower:
                        CurrentSpeed = 0;
                        break;
                    case PowerUp.PowerUpType.Star:
                        StarMovement();
                        break;
                    default:
                        throw new Exception("JAK");
                }
            }
                
        }

        private void MushroomMovement()
        {
            if (Target.Collider.Collision(Direction.Left))
            {
                CurrentSpeed = Speed;
            }

            if (Target.Collider.Collision(Direction.Right))
            {
                CurrentSpeed = -Speed;
            }

            if (!Target.Collider.Collision(Direction.Down))
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

        private void StarMovement()
        {
            if (Target.Collider.Collision(Direction.Left))
            {
                CurrentSpeed = Speed;
            }

            if (Target.Collider.Collision(Direction.Right))
            {
                CurrentSpeed = -Speed;
            }

            if (!Target.Collider.Collision(Direction.Down))
            {
                if (vertSpeed == 0 && !Jumped)
                {
                    vertSpeed = -FloatSpeed * 1.5f;
                    Jumped = true;
                }
                else if (!Target.Collider.Collision(Direction.Up) && vertSpeed < 0)
                {
                    vertSpeed += Engine.DeltaTime * Acceleration * FloatSpeed;
                }
                else
                {
                    vertSpeed = 0;
                    IsFalling = true;
                }
            }
            else
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
        }
    }
}
