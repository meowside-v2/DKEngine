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
    class BlockAnimatorScript : Script
    {
        float AnimationHeight = 10;

        public BlockAnimatorScript(GameObject Parent)
            : base(Parent)
        { }

        protected override void OnColliderEnter(Collider e)
        {

        }

        protected override void Start()
        {

        }

        protected override void Update()
        {
            if(Shared.BlocksToUpdate.Count > 0)
            {
                for(int i = 0; i < Shared.BlocksToUpdate.Count; i++)
                {
                    float StartBlockY = Shared.BlocksStartPositions[i];
                    Block CurrentBlock = Shared.BlocksToUpdate[i];

                    //if(CurrentBlock)
                }
            }
        }
    }
}
