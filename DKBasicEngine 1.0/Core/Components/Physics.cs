using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DKBasicEngine_1_0
{
    public class Physics
    {
        public int Jumpheight { get; set; }
        public bool Jumped { get; set; }
        public int Jumplength { get; set; }
        public bool ForceJump { get; set; }

        public enum PhysicState
        {
            Jump,
            Fall
        };

        Collider colliderReference;
        GameObject Parent;

        public Physics(GameObject Parent, Collider colliderReference)
        {
            this.Parent = Parent;
            this.colliderReference = colliderReference;
        }

        public void Jump()
        {
            double StartPositon = Parent.Y;
            int MinJump = 10;

            do
            {
                if (StartPositon - Parent.Y == Jumpheight)
                {
                    //Fall(world, enemies);
                    Jumped = false;
                    return;
                }
                else if (colliderReference.Collision(Collider.Direction.Up))
                {
                    Fall();
                    Jumped = false;
                    return;
                }
                else if (!ForceJump && StartPositon - Parent.Y > MinJump)
                {
                    Fall();
                    Jumped = false;
                    return;
                }

                else Parent.Y -= 1;

                Thread.Sleep(Jumplength);
            } while (true);
        }

        public void Fall()
        {
            do
            {
                Parent.Y += 1;

                Thread.Sleep(Jumplength);
            } while (!colliderReference.Collision(Collider.Direction.Down));

            Jumped = false;
        }
    }
}
