using DKEngine.Core;
using DKEngine.Core.Components;
using DKEngine.Core.UI;
using MarIO.Assets.Models;
using MarIO.Assets.Models.Miscellaneous;
using System;
using System.Drawing;
using System.Linq;

namespace MarIO.Assets.Scenes
{
    public class WorldScreen : Scene
    {
        private static readonly TimeSpan _defautlTimeSpan = new TimeSpan(0, 0, 5);

        private TextBlock World;
        private TextBlock Lives;
        private Delayer Delayer;

        private static string RemainingLives = "";
        private static string WorldName = "";
        public static Action WorldChange;
        public static TimeSpan? Delay;

        public WorldScreen()
        {
            Name = nameof(WorldScreen);
        }

        public override void Init()
        {

            TextBlock _World = new TextBlock()
            {
                FontSize = 5,
                Foreground = Color.White,
                HAlignment = Text.HorizontalAlignment.Center,
                IsGUI = true,
                Name = "tx_const_world",
                Text = "WORLD",
                TextHAlignment = Text.HorizontalAlignment.Center,
                VAlignment = Text.VerticalAlignment.Center
            };
            _World.Transform.Position += new Vector3(0, -40, 0);
            _World.Transform.Dimensions = new Vector3(120, 30, 0);

            World = new TextBlock()
            {
                FontSize = 4,
                Foreground = Color.White,
                HAlignment = Text.HorizontalAlignment.Center,
                IsGUI = true,
                Name = "tx_world",
                TextHAlignment = Text.HorizontalAlignment.Center,
                VAlignment = Text.VerticalAlignment.Center,
                Text = WorldName
            };
            World.Transform.Position += new Vector3(0, -5, 0);
            World.Transform.Dimensions = new Vector3(100, 30, 0);

            GameObject holder = new GameObject();
            holder.Transform.Position = new Vector3(120, 140, 0);

            Heart _HeartIcon = new Heart(holder)
            {
                IsGUI = true,
                Name = "heart_icon"
            };

            _HeartIcon.Transform.Scale = new Vector3(3, 3, 0);

            Lives = new TextBlock(holder)
            {
                FontSize = 3.5f,
                IsGUI = true,
                TextHAlignment = Text.HorizontalAlignment.Center,
                Text = RemainingLives
            };
            Lives.Transform.Dimensions = new Vector3(40, 15, 0);
            Lives.Transform.Position += new Vector3(32, 8, 0);

            Delayer = new Delayer()
            {
                CalledAction = WorldChange,
                TimeToWait = Delay ?? _defautlTimeSpan
            };

            new Camera()
            {
                BackGround = Shared.Mechanics.WorldChangeBackground.ToColor()
            };

            if (Shared.Mechanics.MarioCurrentState == Mario.State.Dead)
                Shared.Mechanics.MarioCurrentState = Mario.State.Small;
        }

        public override void Set(params object[] args)
        {
            if (args == null)
                return;

            string[] stringParameters = args.Where(obj => obj is string).ToList().Cast<string>().ToArray();
            object[] otherParameters = args.Where(obj => !(obj is string)).ToArray();

            for (int i = 0; i < stringParameters.Length; i++)
            {
                string[] parameters = stringParameters[i].Split(':');

                switch (parameters[0])
                {
                    case "world":
                        if(parameters[1].Split('|')[0] == "get")
                        {
                            WorldName = MapBase.LevelsNames[parameters[1].Split('|')[1]];
                        }
                        else
                        {
                            WorldName = parameters[1];
                        }
                        
                        break;

                    case "time":
                        Delay = TimeSpan.Parse(parameters[1]);
                        break;
                }
            }

            foreach (object item in otherParameters)
            {
                if (item is Action)
                {
                    WorldChange = ((Action)item);
                }
            }

            RemainingLives = string.Format($"*{Shared.Mechanics.Lives:00}");
        }

        public override void Unload()
        { }
    }
}