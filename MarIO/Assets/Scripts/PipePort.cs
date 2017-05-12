using DKEngine.Core;
using DKEngine.Core.Components;
using MarIO.Assets.Models;

namespace MarIO.Assets.Scripts
{
    public class PipePort : Script
    {
        private Mario Player;
        public Block Pipe;

        public PipePort(GameObject Parent) : base(Parent)
        { }

        protected override void OnColliderEnter(Collider e)
        {
            if (Pipe.SpecialAction != null)
            {
                if (e.Parent == Player)
                {
                    switch (Pipe.PipeEnterDirection)
                    {
                        case Transform.Direction.Up:
                            break;

                        case Transform.Direction.Left:
                            break;

                        case Transform.Direction.Down:
                            if (Player.CurrentMovement == Mario.Movement.Crouching)
                            {
                                Player.PipeEnter(Pipe);
                            }
                            break;

                        case Transform.Direction.Right:
                            if (Player.CurrentMovement == Mario.Movement.Standing)
                            {
                                Player.PipeEnter(Pipe);
                            }
                            break;

                        default:
                            break;
                    }
                }
            }
        }

        protected override void Start()
        {
            Player = GameObject.Find<Mario>("Player");
            Pipe = (Block)Parent;
        }

        protected override void Update()
        { }
    }
}