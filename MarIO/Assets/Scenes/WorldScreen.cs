using DKEngine.Core;
using DKEngine.Core.Components;
using DKEngine.Core.UI;
using MarIO.Assets.Models.Miscellaneous;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarIO.Assets.Scenes
{
    class WorldScreen : Scene
    {
        TextBlock World;

        public override void Init()
        {
            TextBlock _World = new TextBlock();
            _World.FontSize = 4;
            _World.Foreground = Color.White;
            _World.HAlignment = Text.HorizontalAlignment.Center;
            _World.IsGUI = true;
            _World.Name = "tx_const_world";
            _World.Text = "WORLD";
            _World.TextHAlignment = Text.HorizontalAlignment.Center;
            _World.Transform.Position = new Vector3(0, -20, 0);
            _World.Transform.Dimensions = new Vector3(100, 30, 0);

            World = new TextBlock();
            _World.FontSize = 4;
            _World.Foreground = Color.White;
            _World.HAlignment = Text.HorizontalAlignment.Center;
            _World.IsGUI = true;
            _World.Name = "tx_world";
            _World.Text = "WORLD";
            _World.TextHAlignment = Text.HorizontalAlignment.Center;
            _World.Transform.Position = new Vector3(0, 5, 0);
            _World.Transform.Dimensions = new Vector3(100, 30, 0);

            Heart _HeartIcon = new Heart();
            _HeartIcon.IsGUI = true;
            _HeartIcon.Name = "heart_icon";
            _HeartIcon.Transform.Position = new Vector3();
        }

        public override void Set(params string[] Args)
        {
            object[][] parameters = new object[Args.Length][];

            for (int i = 0; i < Args.Length; i++)
            {
                parameters[i] = Args[i].Split(':');

                switch (parameters[i][0])
                {
                    case "text":

                        break;
                }
            }
        }

        public override void Unload()
        { }
    }
}
