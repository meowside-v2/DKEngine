using DKEngine.Core;
using DKEngine.Core.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarIO.Assets.Models.Miscellaneous
{
    class Coin : GameObject
    {
        public Coin()
        { }

        public Coin(GameObject Parent)
            :base(Parent)
        { }

        protected override void Initialize()
        {
            this.Name = "coin";
            //this.TypeName = "coin";
            this.InitNewComponent<Animator>();
            this.Animator.AddAnimation("default", Database.GetGameObjectMaterial("coin"));
        }
    }
}
