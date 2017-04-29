using DKEngine;
using DKEngine.Core;
using DKEngine.Core.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarIO.Assets.Models
{
    class Blocker : GameObject
    {
        public Blocker()
            :base()
        { }

        public Blocker(GameObject Parent)
            :base(Parent)
        { }

        protected override void Initialize()
        {
            this.InitNewComponent<Collider>();
        }
    }
}
