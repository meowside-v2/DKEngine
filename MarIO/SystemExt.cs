using DKEngine.Core.UI;
using MarIO.Assets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarIO
{
    static class SystemExt
    {
        public static void AddAsFloatingText(this TextBlock txBlock)
        {
            Shared.FloatingTexts.Add(txBlock);
            Shared.FloatingTextStartPosition.Add(txBlock.Transform.Position.Y);
        }

        public static void AnimateBlockCollision(this Block block)
        {
            block.State = Block.CollisionState.Up;

            Shared.BlocksToUpdate.Add(block);
            Shared.BlocksStartPositions.Add(block.Transform.Position.Y);
        }
    }
}
