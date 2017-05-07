using DKEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DKEngine.Core.Components;
using MarIO.Assets.Models;

namespace MarIO.Assets.Scripts
{
    class WorldChangeManagerScript : Script
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
