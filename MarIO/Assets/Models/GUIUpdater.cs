
using DKEngine.Core;
using DKEngine.Core.Components;
using DKEngine.Core.UI;
using MarIO.Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace MarIO.Assets.Models
{
    class GUIUpdater : GameObject
    {
        protected override void Init()
        {
            Name = "GUI";

            this.IsGUI = true;

            /*------------ TIME TEXT ----------------*/

            TextBlock _time = new TextBlock(this)
            {
                IsGUI = true,
                TextShadow = true,
                Text = "TIME",
                FontSize = 2
            };
            _time.Transform.Dimensions = new Vector3(100, 20, 1);
            _time.Transform.Position += new Vector3(16, 4, 128);

            TextBlock Time = new TextBlock(this)
            {
                Name = "txt_Time",
                IsGUI = true,
                TextShadow = true,
                Text = "",
                FontSize = 2
            };
            Time.Transform.Dimensions = new Vector3(100, 20, 1);
            Time.Transform.Position += new Vector3(22, 16, 128);

            TextBlock Score = new TextBlock(this)
            {
                Name = "txt_Score",
                Text = "",
                IsGUI = true,
                TextShadow = true,
                FontSize = 2,
                HAlignment = Text.HorizontalAlignment.Right,
                TextHAlignment = Text.HorizontalAlignment.Right
            };
            Score.Transform.Dimensions = new Vector3(100, 20, 1);
            Score.Transform.Position += new Vector3(-16, 4, 128);
            
            TextBlock _lives = new TextBlock(this)
            {
                Text = "LIVES",
                IsGUI = true,
                TextShadow = true,
                FontSize = 2
            };
            _lives.Transform.Dimensions = new Vector3(100, 20, 1);
            _lives.Transform.Position += new Vector3(100, 4, 128);

            /*------------ WORLD TEXT ----------------*/

            TextBlock _world = new TextBlock(this)
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

            TextBlock World = new TextBlock(this)
            {
                Name = "txt_World",
                Text = "",
                IsGUI = true,
                TextShadow = true,
                FontSize = 2,
                HAlignment = Text.HorizontalAlignment.Right,
                TextHAlignment = Text.HorizontalAlignment.Right
            };
            World.Transform.Dimensions = new Vector3(100, 20, 1);
            World.Transform.Position += new Vector3(-100, 16, 128);

            this.InitNewScript<GUIUpdateScript>();
        }
    }
}
