using DKEngine.Core.UI;
using MarIO.Assets.Models;
using MarIO.Assets.Models.Miscellaneous;
using System;
using System.Collections.Generic;
using System.Drawing;
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

        public static void AddAsFloatingCoin(this Coin coin)
        {
            Shared.FloatingCoins.Add(coin);
            Shared.FloatingCoinsStartPosition.Add(coin.Transform.Position.Y);
        }

        public static Color ToColor(this uint color)
        {
            byte a = (byte)(color >> 24);
            byte r = (byte)(color >> 16);
            byte g = (byte)(color >> 8);
            byte b = (byte)(color >> 0);
            return Color.FromArgb(a, r, g, b);
        }
    }
}
