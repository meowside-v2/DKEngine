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
            int StartPositon = Parent.Y;
            int MinJump = 10;

            do
            {
                if (StartPositon - Parent.Y == Parent.jumpheight)
                {
                    //Fall(world, enemies);
                    Parent.Jumped = false;
                    return;
                }
                else if (colliderReference.Collision(Collider.Direction.Up))
                {
                    Fall();
                    Parent.Jumped = false;
                    return;
                }
                else if (!Parent.ForceJump && StartPositon - Parent.Y > MinJump)
                {
                    Fall();
                    Parent.Jumped = false;
                    return;
                }

                else Parent.Y -= 1;

                Thread.Sleep(Parent.jumplength);
            } while (true);
        }

        public void Fall()
        {
            do
            {
                Parent.Y += 1;

                Thread.Sleep(Parent.jumplength);
            } while (!colliderReference.Collision(Collider.Direction.Down));

            Parent.Jumped = false;
        }
    }
}
