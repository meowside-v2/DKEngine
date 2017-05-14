using DKEngine;
using DKEngine.Core;
using DKEngine.Core.Components;
using DKEngine.Core.UI;
using MarIO.Assets.Models;
using MarIO.Assets.Models.Miscellaneous;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarIO.Assets.Scenes
{
    class GameOver : Scene
    {
        public GameOver()
        {
            Name = nameof(GameOver);
        }

        public override void Init()
        {
            TextBlock GameOver = new TextBlock()
            {
                FontSize = 5,
                Foreground = Color.White,
                HAlignment = Text.HorizontalAlignment.Center,
                IsGUI = true,
                Name = "tx_GameOver",
                Text = "GAME OVER",
                TextHAlignment = Text.HorizontalAlignment.Center,
                VAlignment = Text.VerticalAlignment.Center,
            };
            GameOver.Transform.Dimensions = new Vector3(200, 30, 0);
            GameOver.Transform.Position += new Vector3(0, -30, 0);

            TextBlock Score = new TextBlock()
            {
                FontSize = 2.5f,
                Foreground = Color.White,
                HAlignment = Text.HorizontalAlignment.Center,
                IsGUI = true,
                Name = "tx_Score",
                Text = Shared.Mechanics.GameScoreStr,
                TextHAlignment = Text.HorizontalAlignment.Center,
                VAlignment = Text.VerticalAlignment.Center
            };
            Score.Transform.Dimensions = new Vector3(100, 30, 0);
            Score.Transform.Position += new Vector3(0, 30, 0);

            GameObject holder = new GameObject();
            holder.Transform.Position = new Vector3(136, 156, 0);

            Coin CoinIcon = new Coin(holder)
            {
                IsGUI = true,
                Name = "coin_icon"
            };
            CoinIcon.Transform.Scale = new Vector3(2f, 2f, 0);

            TextBlock Coins = new TextBlock(holder)
            {
                FontSize = 2.5f,
                IsGUI = true,
                TextHAlignment = Text.HorizontalAlignment.Center,
                Text = string.Format($"*{Shared.Mechanics.CoinsCount:00}")
            };
            Coins.Transform.Dimensions = new Vector3(40, 15, 0);
            Coins.Transform.Position += new Vector3(12, 2, 0);

            new Delayer()
            {
                CalledAction = () => Engine.LoadScene<MainMenu>(),
                TimeToWait = new TimeSpan(0, 0, 5)
            };

            new Camera()
            {
                BackGround = Shared.Mechanics.WorldChangeBackground.ToColor()
            };

            Shared.Mechanics.MarioCurrentState = Mario.State.Super;
        }

        public override void Unload()
        { }
    }
}
