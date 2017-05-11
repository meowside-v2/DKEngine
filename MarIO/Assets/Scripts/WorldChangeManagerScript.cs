using DKEngine.Core;
using DKEngine.Core.Components;
using MarIO.Assets.Models;

namespace MarIO.Assets.Scripts
{
    internal class WorldChangeManagerScript : Script
    {
        public Block CurrentlyEnteredPipeScript;

        public WorldChangeManagerScript(GameObject Parent) : base(Parent)
        {
            Name = "worldManager";
        }

        protected override void OnColliderEnter(Collider e)
        { }

        protected override void Start()
        { }

        protected override void Update()
        {
            CurrentlyEnteredPipeScript?.SpecialAction();
            CurrentlyEnteredPipeScript = null;
        }
    }
}