using DKEngine.Core.UI;
using MarIO.Assets.Models;
using MarIO.Assets.Models.Miscellaneous;
using System.Drawing;

namespace MarIO
{
    public static class SystemExt
    {
        public static void AddAsFloatingText(this TextBlock txBlock)
        {
            Shared.AnimatedWorldReferences.FloatingTexts.Add(txBlock);
            Shared.AnimatedWorldReferences.FloatingTextStartPosition.Add(txBlock.Transform.Position.Y);
        }

        public static void AnimateBlockCollision(this Block block)
        {
            block.State = Block.CollisionState.Up;

            Shared.AnimatedWorldReferences.BlocksToUpdate.Add(block);
            Shared.AnimatedWorldReferences.BlocksStartPositions.Add(block.Transform.Position.Y);
        }

        public static void AddAsFloatingCoin(this Coin coin)
        {
            Shared.AnimatedWorldReferences.FloatingCoins.Add(coin);
            Shared.AnimatedWorldReferences.FloatingCoinsStartPosition.Add(coin.Transform.Position.Y);
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