using DKEngine.Core;
using DKEngine.Core.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarIO.Assets.Models.Miscellaneous
{
    class Heart : GameObject
    {
        public Heart()
        { }

        public Heart(GameObject Parent)
            :base(Parent)
        { }

        protected override void Init()
        {
            this.Name = "heart";
            //this.TypeName = "coin";
            this.InitNewComponent<Animator>();
            this.Animator.AddAnimation("default", Database.GetGameObjectMaterial("heart"));
        }
    }
}
