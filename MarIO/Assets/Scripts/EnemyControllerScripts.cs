using DKEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DKEngine.Core.Components;
using DKEngine;

namespace MarIO.Assets.Scripts
{
    class GoombaController : Script
    {
        const int Speed = 40;
        const int FloatSpeed = 100;
        const int Acceleration = 40;

        int CurrentSpeed = 0;
        float vertSpeed = 0;
        bool IsFalling = false;

        public GoombaController(GameObject Parent) : base(Parent)
        { }

        protected override void OnColliderEnter(Collider e)
        { }

        protected override void Start()
        {
            CurrentSpeed = -Speed;
        }

        protected override void Update()
        {
            if (this.Parent.Collider.Collision(Collider.Direction.Left))
            {
                CurrentSpeed = Speed;
            }

            if (this.Parent.Collider.Collision(Collider.Direction.Right))
            {
                CurrentSpeed = -Speed;
            }

            if (!Parent.Collider.Collision(Collider.Direction.Down))
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
                        vertSpeed += Engine.deltaTime * Acceleration;
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

            this.Parent.Transform.Position += new Vector3(CurrentSpeed * Engine.deltaTime, vertSpeed * Engine.deltaTime, 0);
        }
    }
}
