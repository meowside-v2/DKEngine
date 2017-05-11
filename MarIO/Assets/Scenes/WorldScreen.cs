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
    class WorldScreen : Scene
    {
        private Action _worldChange;
        private TimeSpan? _delay;
        private TextBlock World;
        private TextBlock Lives;
        private Delayer Delayer;
        private readonly TimeSpan _defautlTimeSpan = new TimeSpan(0, 0, 5);

        public Action WorldChange
        {
            set
            {
                _worldChange = value;
                Delayer.CalledAction = value;
            }
        }
        public TimeSpan? Delay
        {
            set
            {
                _delay = value;
                Delayer.TimeToWait = value ?? _defautlTimeSpan;
            }
        }
        
        public WorldScreen()
        {
            Name = nameof(WorldScreen);
        }

        public override void Init()
        {
            TextBlock _World = new TextBlock()
            {
                FontSize = 4,
                Foreground = Color.White,
                HAlignment = Text.HorizontalAlignment.Center,
                IsGUI = true,
                Name = "tx_const_world",
                Text = "WORLD",
                TextHAlignment = Text.HorizontalAlignment.Center
            };
            _World.Transform.Position = new Vector3(0, -20, 0);
            _World.Transform.Dimensions = new Vector3(100, 30, 0);

            World = new TextBlock()
            {
                FontSize = 4,
                Foreground = Color.White,
                HAlignment = Text.HorizontalAlignment.Center,
                IsGUI = true,
                Name = "tx_world",
                TextHAlignment = Text.HorizontalAlignment.Center
            };
            World.Transform.Position = new Vector3(0, 5, 0);
            World.Transform.Dimensions = new Vector3(100, 30, 0);

            GameObject holder = new GameObject();
            holder.Transform.Position = new Vector3(100, 100, 0);

            Heart _HeartIcon = new Heart(holder)
            {
                IsGUI = true,
                Name = "heart_icon"
            };
            
            _HeartIcon.Transform.Scale = new Vector3(2, 2, 0);

            Lives = new TextBlock(holder)
            {
                FontSize = 2,
                IsGUI = true,
                TextHAlignment = Text.HorizontalAlignment.Center
            };
            Lives.Transform.Dimensions = new Vector3(40, 15, 0);
            Lives.Transform.Position += new Vector3(16, 8, 0);

            Delayer = new Delayer()
            {
                TimeToWait = _delay ?? _defautlTimeSpan
            };

            new Camera()
            {
                BackGround = Shared.Mechanics.WorldChangeBackground.ToColor()
            };
        }
            
        public override void Set(params object[] args)
        {
            string[] stringParameters = args.Where(obj => obj is string).ToList().Cast<string>().ToArray();
            object[] otherParameters = args.Where(obj => !(obj is string)).ToArray();

            for (int i = 0; i < stringParameters.Length; i++)
            {
                string[] parameters = stringParameters[i].Split(':');

                switch (parameters[0])
                {
                    case "world":
                        World.Text = parameters[1];
                        break;

                    case "time":
                        Delay = TimeSpan.Parse(parameters[1]);
                        break;
                }
            }

            foreach (object item in otherParameters)
            {
                if(item is Action)
                {
                    WorldChange = ((Action)item);
                }
            }

            Lives.Text = string.Format($"*{Shared.Mechanics.Lives:00}");
        }

        public override void Unload()
        { }
    }
}
