using System.Threading;

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

        
        GameObject Parent;
        Collider colliderReference;

        public Physics(GameObject Parent)
        {
            this.Parent = Parent;
            this.colliderReference = Parent.Collider;
        }

        public void Jump()
        {
            float StartPositon = Parent.Transform.Position.Y;
            int MinJump = 10;

            do
            {
                if (StartPositon - Parent.Transform.Position.Y == Jumpheight)
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
                else if (!ForceJump && StartPositon - Parent.Transform.Position.Y > MinJump)
                {
                    Fall();
                    Jumped = false;
                    return;
                }

                //else Parent.Transform.Position.Y -= 1;

                Thread.Sleep(Jumplength);
            } while (true);
        }

        public void Fall()
        {
            do
            {
                //Parent.Transform.Position.Y += 1;

                Thread.Sleep(Jumplength);
            } while (!colliderReference.Collision(Collider.Direction.Down));

            Jumped = false;
        }
    }
}
