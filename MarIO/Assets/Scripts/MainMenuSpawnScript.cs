using DKEngine.Core;
using DKEngine.Core.Components;
using MarIO.Assets.Models;

namespace MarIO.Assets.Scripts
{
    public class MainMenuSpawnScript : Script
    {
        private Vector3 Position;

        public MainMenuSpawnScript(GameObject Parent) : base(Parent)
        { }

        protected override void OnColliderEnter(Collider e)
        {
            e.Parent.Transform.Position = Position;
        }

        protected override void Start()
        {
            Position = new Vector3(320, 176, 0);

            Goomba e = new Goomba()
            {
                Name = "Bot"
            };
            e.Transform.Position = Position;
        }

        protected override void Update()
        { }
    }
}