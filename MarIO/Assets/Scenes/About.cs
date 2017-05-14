using DKEngine;
using DKEngine.Core;
using DKEngine.Core.Components;
using DKEngine.Core.UI;
using MarIO.Assets.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarIO.Assets.Scenes
{
    class About : Scene
    {
        public override void Init()
        {
            new Camera()
            {
                BackGround = Shared.Mechanics.OverworldBackground.ToColor()
            };

            new Group()
            {
                SizeInBlocks = new Vector3(1, 20, 0),
                Type = Block.BlockType.Ground2,
                InitCollider = true
            }.Transform.Position = new Vector3(0, 0, 0);

            new Group()
            {
                SizeInBlocks = new Vector3(1, 20, 0),
                Type = Block.BlockType.Ground2,
                InitCollider = true
            }.Transform.Position = new Vector3(48, 0, 0);

            new Group()
            {
                SizeInBlocks = new Vector3(2, 1, 0),
                Type = Block.BlockType.Ground2,
                InitCollider = true
            }.Transform.Position = new Vector3(16, 224, 0);

            new Block()
            {
                InitCollider = true,
                Type = Block.BlockType.Pipe3,
                SpecialAction = GoBack
            }.Transform.Position = new Vector3(16, 192, 1);

            new Mario()
            {
                InitCollider = true,
                InitCharacterController = true
            }.Transform.Position = new Vector3(16, 80, 0);

            var _Mario = new TextBlock()
            {
                Foreground = Color.LawnGreen,
                FontSize = 6,
                HAlignment = Text.HorizontalAlignment.Center,
                IsGUI = true,
                Text = "MARIO",
                TextShadow = true,
                TextHAlignment = Text.HorizontalAlignment.Center
            };
            _Mario.Transform.Position += new Vector3(30, 20, 0);
            _Mario.Transform.Dimensions = new Vector3(200, 30, 0);

            var _author = new TextBlock()
            {
                FontSize = 2,
                HAlignment = Text.HorizontalAlignment.Center,
                IsGUI = true,
                Text = "BY DAVID KNIERADL 2017",
                TextShadow = true,
                TextHAlignment = Text.HorizontalAlignment.Center
            };
            _author.Transform.Position += new Vector3(30, 80, 0);
            _author.Transform.Dimensions = new Vector3(200, 30, 0);

            var _using = new TextBlock()
            {
                Foreground = Color.YellowGreen,
                FontSize = 3,
                HAlignment = Text.HorizontalAlignment.Center,
                IsGUI = true,
                Text = "Made with",
                TextShadow = true,
                TextHAlignment = Text.HorizontalAlignment.Center
            };
            _using.Transform.Position += new Vector3(30, 110, 0);
            _using.Transform.Dimensions = new Vector3(200, 30, 0);

            var _dkengine = new TextBlock()
            {
                FontSize = 2,
                HAlignment = Text.HorizontalAlignment.Center,
                IsGUI = true,
                Text = "DKENGINE",
                TextShadow = true,
                TextHAlignment = Text.HorizontalAlignment.Center
            };
            _dkengine.Transform.Position += new Vector3(30, 140, 0);
            _dkengine.Transform.Dimensions = new Vector3(200, 30, 0);

            var _naudio = new TextBlock()
            {
                FontSize = 2,
                HAlignment = Text.HorizontalAlignment.Center,
                IsGUI = true,
                Text = "NAUDIO",
                TextShadow = true,
                TextHAlignment = Text.HorizontalAlignment.Center
            };
            _naudio.Transform.Position += new Vector3(30, 155, 0);
            _naudio.Transform.Dimensions = new Vector3(200, 30, 0);

            var _ver = new TextBlock()
            {
                FontSize = 1,
                HAlignment = Text.HorizontalAlignment.Center,
                IsGUI = true,
                Text = "version 0.0.1 alpha",
                TextShadow = true,
                TextHAlignment = Text.HorizontalAlignment.Center
            };
            _ver.Transform.Position += new Vector3(30, 190, 0);
            _ver.Transform.Dimensions = new Vector3(200, 30, 0);

            new MusicPlayer();
            new SoundOutput();
            new BackgroundWorker();
        }

        public override void Unload()
        { }

        private void GoBack()
        {
            Engine.ChangeScene(nameof(MainMenu), true);
        }
    }
}
