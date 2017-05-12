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
    internal class MainMenu : Scene
    {
        public MainMenu()
        {
            Name = "MainMenu";
        }

        public override void Init()
        {
            Group wall4 = new Group()
            {
                InitCollider = true,
                Name = "Wall_4",
                SizeInBlocks = new Vector3(17, 1, 0),
                Type = Block.BlockType.Ground2
            };
            wall4.Transform.Position = new Vector3(0, 16 * 11, 0);

            Group wall5 = new Group()
            {
                InitCollider = true,
                Name = "Wall_5",
                SizeInBlocks = new Vector3(16, 1, 0),
                Type = Block.BlockType.Ground2
            };
            wall5.Transform.Position = new Vector3(0, 16 * 8, 0);

            Block pipe1 = new Block()
            {
                Name = "Pipe_1_Play",
                Type = Block.BlockType.Pipe3
            };
            pipe1.Transform.Position = new Vector3(32, 16 * 6, 1);
            pipe1.SpecialAction = Play;

            Block pipe2 = new Block()
            {
                Name = "Pipe_2_Options",
                Type = Block.BlockType.Pipe3
            };
            pipe2.Transform.Position = new Vector3(112, 16 * 6, 1);

            Block pipe3 = new Block()
            {
                Name = "Pipe_3_Exit",
                Type = Block.BlockType.Pipe3
            };
            pipe3.Transform.Position = new Vector3(32 * 6, 16 * 6, 1);
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
                FontSize = 5,
                HAlignment = Text.HorizontalAlignment.Center,
                Name = "tx_MainMenuHeader",
                Text = "MARIO",
                TextHAlignment = Text.HorizontalAlignment.Center,
                TextShadow = true
            };
            //MainMenuHeader.IsGUI = true;
            MainMenuHeader.Transform.Position += new Vector3(0, 10, 0);
            MainMenuHeader.Transform.Dimensions = new Vector3(200, 50, 0);

            TextBlock PlayText = new TextBlock()
            {
                Name = "tx_Play",
                Text = "Play",
                TextHAlignment = Text.HorizontalAlignment.Center,
                TextShadow = true
            };
            PlayText.Transform.Position = new Vector3(9, 80, -1);
            PlayText.Transform.Dimensions = new Vector3(80, 20, 0);

            TextBlock OptionsText = new TextBlock()
            {
                Name = "tx_Options",
                Text = "Options",
                TextHAlignment = Text.HorizontalAlignment.Center,
                TextShadow = true,
                HAlignment = Text.HorizontalAlignment.Center
            };
            OptionsText.Transform.Position += new Vector3(0, 80, -1);
            OptionsText.Transform.Dimensions = new Vector3(80, 20, 0);

            TextBlock ExitText = new TextBlock()
            {
                Name = "tx_Exit",
                Text = "Exit",
                TextHAlignment = Text.HorizontalAlignment.Center,
                TextShadow = true,
                HAlignment = Text.HorizontalAlignment.Right
            };
            ExitText.Transform.Position += new Vector3(-8, 80, -1);
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
            mountain.Transform.Position = new Vector3(100, 120, -1);
            mountain.Transform.Scale = new Vector3(2, 2, 0);

            Block bush1 = new Block()
            {
                Name = "bush_1",
                Type = Block.BlockType.Bush3
            };
            bush1.Transform.Position = new Vector3(180, 150, -1);

            Block bush2 = new Block()
            {
                Name = "bush_2",
                Type = Block.BlockType.Bush2
            };
            bush2.Transform.Position = new Vector3(25, 150, -1);

            Block fence1 = new Block()
            {
                Name = "fence_1",
                Type = Block.BlockType.Fence
            };
            fence1.Transform.Position = new Vector3(90, 160, -1);

            Block fence2 = new Block()
            {
                Name = "fence_2",
                Type = Block.BlockType.Fence
            };
            fence2.Transform.Position = new Vector3(106, 160, -1);

            Block fence3 = new Block()
            {
                Name = "fence_3",
                Type = Block.BlockType.Fence
            };
            fence3.Transform.Position = new Vector3(122, 160, -1);

            //player.Transform.Position = new Vector3(20, 20, 0);

            Blocker leftSide = new Blocker()
            {
                Name = "LeftSideBlocker"
            };
            leftSide.Transform.Position = new Vector3(-10, 0, 0);
            leftSide.Transform.Dimensions = new Vector3(10, 128, 0);
            //leftSide.Model = new Material(Color.Black, leftSide);

            Blocker rightSide = new Blocker()
            {
                Name = "LeftSideBlocker"
            };
            rightSide.Transform.Position = new Vector3(256, 0, 0);
            rightSide.Transform.Dimensions = new Vector3(10, 128, 0);
            //rightSide.Model = new Material(Color.Black, leftSide);

            Block CoinBlockTest = new Block()
            {
                Name = "CoinBlockTest",
                Type = Block.BlockType.Ground1
            };
            CoinBlockTest.InitNewComponent<Collider>();
            CoinBlockTest.Collider.Area = new RectangleF(0, 0, 16, 16);
            CoinBlockTest.Transform.Position = new Vector3(30, 30, 0);

            BackgroundWorker BW = new BackgroundWorker();
            BW.InitNewComponent<Collider>();
            BW.Collider.Area = new RectangleF(-10, 160, 10, 10);
            BW.Collider.IsTrigger = true;
            BW.InitNewScript<MainMenuSpawnScript>();

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
            Shared.Mechanics.CoinsCount = 0;
            Shared.Mechanics.GameScore = 0;
            Shared.Mechanics.Lives = 3;
            Shared.Mechanics.TimeCounter.Reset();

            Engine.LoadScene<WorldScreen>(new object[] { (Action)(() => Engine.LoadScene<Level_1_1>()), $"world:get|{nameof(Level_1_1)}" });

            //Engine.LoadScene<Test>();
            //Engine.ChangeScene("test");
        }
    }
}