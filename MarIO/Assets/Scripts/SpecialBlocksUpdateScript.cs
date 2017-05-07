using DKEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DKEngine.Core.Components;

namespace MarIO.Assets.Scripts
{
    class SpecialBlocksUpdateScript : Script
    {
        public SpecialBlocksUpdateScript(GameObject Parent)
            : base(Parent)
        { }

        protected override void OnColliderEnter(Collider e)
        { }

        protected override void Start()
        {

        }

        protected override void Update()
        {
            while(Shared.SpecialActions.Count > 0)
            {
                Shared.SpecialActions.Pop().SpecialAction();
            }
        }
    }
}
