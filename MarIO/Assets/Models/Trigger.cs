using DKEngine.Core;
using DKEngine.Core.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarIO.Assets.Models
{
    class Trigger : GameObject
    {
        public Trigger()
            :base()
        { }

        public Trigger(GameObject Parent)
            :base(Parent)
        { }

        protected override void Init()
        {
            this.InitNewComponent<Collider>();
            this.Collider.IsTrigger = true;
        }
    }
}
