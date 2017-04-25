using DKEngine.Core;
using DKEngine.Core.Components;
using DKEngine.Core.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarIO.Assets.Models
{
    class GUIUpdater : GameObject
    {
        TextBlock Time;
        TextBlock Coins;
        TextBlock World;
        TextBlock Lives;

        protected override void Init()
        {
            Name = "GUI";

            TextBlock _time = new TextBlock()
            {
                IsGUI = true,
                TextShadow = true,
                Text = "TIME",
                FontSize = 2
            };
            _time.Transform.Dimensions = new Vector3(100, 20, 1);
            _time.Transform.Position += new Vector3(16, 4, 128);
            

            TextBlock _coins = new TextBlock()
            {
                Text = "COINS",
                IsGUI = true,
                TextShadow = true,
                FontSize = 2,
                HAlignment = Text.HorizontalAlignment.Right,
                TextHAlignment = Text.HorizontalAlignment.Right
            };
            _coins.Transform.Dimensions = new Vector3(100, 20, 1);
            _coins.Transform.Position += new Vector3(-16, 4, 128);

            TextBlock _lives = new TextBlock()
            {
                Text = "LIVES",
                IsGUI = true,
                TextShadow = true,
                FontSize = 2
            };
            _lives.Transform.Dimensions = new Vector3(100, 20, 1);
            _lives.Transform.Position += new Vector3(100, 4, 128);

            TextBlock _world = new TextBlock()
            {
                Text = "WORLD",
                IsGUI = true,
                TextShadow = true,
                FontSize = 2,
                HAlignment = Text.HorizontalAlignment.Right,
                TextHAlignment = Text.HorizontalAlignment.Right
            };
            _world.Transform.Dimensions = new Vector3(100, 20, 1);
            _world.Transform.Position += new Vector3(-100, 4, 128);
        }
    }
}
