using DKEngine;
using DKEngine.Core;
using DKEngine.Core.Components;
using DKEngine.Core.UI;
using MarIO.Assets.Models;
using MarIO.Assets.Scripts;
using System;
using System.Drawing;

namespace MarIO.Assets.Scenes
{
    public class MainMenu : Scene
    {
        public MainMenu()
        {
            Name = nameof(MainMenu);
        }

        public override void Init()
        {
            Group wall4 = new Group()
            {
                InitCollider = true,
                Name = "Wall_4",
                SizeInBlocks = new Vector3(21, 2, 0),
                Type = Block.BlockType.Ground2
            };
            wall4.Transform.Position = new Vector3(0, 16 * 13, 0);

            Group wall5 = new Group()
            {
                InitCollider = true,
                Name = "Wall_5",
                SizeInBlocks = new Vector3(22, 1, 0),
                Type = Block.BlockType.Ground2
            };
            wall5.Transform.Position = new Vector3(0, 16 * 9, 0);

            Block pipe1 = new Block()
            {
                Name = "Pipe_1_Play",
                Type = Block.BlockType.Pipe3
            };
            pipe1.Transform.Position = new Vector3(32, 16 * 7, 1);
            pipe1.SpecialAction = Play;

            Block pipe2 = new Block()
            {
                Name = "Pipe_2_About",
                Type = Block.BlockType.Pipe3
            };
            pipe2.Transform.Position = new Vector3(143, 16 * 7, 1);

            Block pipe3 = new Block()
            {
                Name = "Pipe_3_Exit",
                Type = Block.BlockType.Pipe3
            };
            pipe3.Transform.Position = new Vector3(256, 16 * 7, 1);
            pipe3.SpecialAction = Exit;

            Camera baseCam = new Camera()
            {
                BackGround = Shared.Mechanics.OverworldBackground.ToColor()
            };

            Mario player = new Mario()
            {
                InitCharacterController = true,
                InitCollider = true
            };

            TextBlock MainMenuHeader = new TextBlock()
            {
                FontSize = 6,
                HAlignment = Text.HorizontalAlignment.Center,
                Name = "tx_MainMenuHeader",
                Text = "MARIO",
                TextHAlignment = Text.HorizontalAlignment.Center,
                TextShadow = true
            };
            MainMenuHeader.Transform.Position += new Vector3(0, 10, 0);
            MainMenuHeader.Transform.Dimensions = new Vector3(200, 50, 0);

            TextBlock PlayText = new TextBlock()
            {
                Name = "tx_Play",
                Text = "Play",
                TextHAlignment = Text.HorizontalAlignment.Center,
                TextShadow = true
            };
            PlayText.Transform.Position = new Vector3(9, 96, -1);
            PlayText.Transform.Dimensions = new Vector3(80, 20, 0);

            TextBlock OptionsText = new TextBlock()
            {
                Name = "tx_Options",
                Text = "About",
                TextHAlignment = Text.HorizontalAlignment.Center,
                TextShadow = true,
                HAlignment = Text.HorizontalAlignment.Center
            };
            OptionsText.Transform.Position += new Vector3(0, 96, -1);
            OptionsText.Transform.Dimensions = new Vector3(80, 20, 0);

            TextBlock ExitText = new TextBlock()
            {
                Name = "tx_Exit",
                Text = "Exit",
                TextHAlignment = Text.HorizontalAlignment.Center,
                TextShadow = true,
                HAlignment = Text.HorizontalAlignment.Right
            };
            ExitText.Transform.Position += new Vector3(-8, 96, -1);
            ExitText.Transform.Dimensions = new Vector3(80, 20, 0);

            Block cloud1 = new Block()
            {
                Name = "cloud_1",
                Type = Block.BlockType.Cloud3
            };
            cloud1.Transform.Position = new Vector3(-10, 20, -1);

            Block cloud2 = new Block()
            {
                Name = "cloud_2",
                Type = Block.BlockType.Cloud1
            };
            cloud2.Transform.Position = new Vector3(120, -15, -1);

            Block cloud3 = new Block()
            {
                Name = "cloud_3",
                Type = Block.BlockType.Cloud2
            };
            cloud3.Transform.Position = new Vector3(180, 34, -1);

            Block mountain = new Block()
            {
                Name = "mountain",
                Type = Block.BlockType.Mountain
            };
            mountain.Transform.Position = new Vector3(100, 152, -1);
            mountain.Transform.Scale = new Vector3(2, 2, 0);

            Block bush1 = new Block()
            {
                Name = "bush_1",
                Type = Block.BlockType.Bush3
            };
            bush1.Transform.Position = new Vector3(180, 182, -1);

            Block bush2 = new Block()
            {
                Name = "bush_2",
                Type = Block.BlockType.Bush2
            };
            bush2.Transform.Position = new Vector3(25, 182, -1);

            Block fence1 = new Block()
            {
                Name = "fence_1",
                Type = Block.BlockType.Fence
            };
            fence1.Transform.Position = new Vector3(90, 192, -1);

            Block fence2 = new Block()
            {
                Name = "fence_2",
                Type = Block.BlockType.Fence
            };
            fence2.Transform.Position = new Vector3(106, 192, -1);

            Block fence3 = new Block()
            {
                Name = "fence_3",
                Type = Block.BlockType.Fence
            };
            fence3.Transform.Position = new Vector3(122, 192, -1);

            Blocker leftSide = new Blocker()
            {
                Name = "LeftSideBlocker"
            };
            leftSide.Transform.Position = new Vector3(-10, -20, 0);
            leftSide.Transform.Dimensions = new Vector3(10, 148, 0);

            Blocker rightSide = new Blocker()
            {
                Name = "LeftSideBlocker"
            };
            rightSide.Transform.Position = new Vector3(320, -20, 0);
            rightSide.Transform.Dimensions = new Vector3(10, 148, 0);
            
            BackgroundWorker BW = new BackgroundWorker();
            BW.InitNewComponent<Collider>();
            BW.Collider.Area = new RectangleF(-10, 160, 10, 30);
            BW.Collider.IsTrigger = true;
            BW.InitNewScript<MainMenuSpawnScript>();

            new MusicPlayer();
            new SoundOutput();
        }

        public override void Set(params object[] Args)
        { }

        public override void Unload()
        { }

        private void Exit()
        {
            Environment.Exit(1);
        }

        private void Play()
        {
            Shared.Mechanics.MarioCurrentState = Mario.State.Small;
            Shared.Mechanics.CoinsCount = 0;
            Shared.Mechanics.GameScore = 0;
            Shared.Mechanics.Lives = 3;
            Shared.Mechanics.TimeCounter.Reset();

            Engine.ChangeScene(nameof(WorldScreen), true, new object[] { (Action)(() => Engine.ChangeScene(MapBase.LevelsNames[nameof(Level_1_1)], true)), $"world:get|{nameof(Level_1_1)}" });
        }
    }
}