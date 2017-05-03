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
    class MainMenu : Scene
    {
        public MainMenu()
        {
            Name = "MainMenu";
        }

        public override void Init()
        {
            /*Group wall1 = new Group()
            {
                InitCollider = true,
                Name = "Wall_1",
                SizeInBlocks = new Vector3(1, 12, 0),
                Type = Block.BlockType.Ground2
            };

            Group wall2 = new Group()
            {
                InitCollider = true,
                Name = "Wall_2",
                SizeInBlocks = new Vector3(1, 12, 0),
                Type = Block.BlockType.Ground2
            };
            wall2.Transform.Position = new Vector3(16 * 15, 0, 0);*/

            /*Group wall3 = new Group()
            {
                InitCollider = true,
                Name = "Wall_3",
                SizeInBlocks = new Vector3(16, 1, 0),
                Type = Block.BlockType.Ground2
            };
            wall3.Transform.Position = new Vector3(0, 0, 0);*/

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
            pipe1.Transform.Position = new Vector3(32, 16 * 6, 0);

            Block pipe2 = new Block()
            {
                Name = "Pipe_2_Options",
                Type = Block.BlockType.Pipe3
            };
            pipe2.Transform.Position = new Vector3(112, 16 * 6, 0);

            Block pipe3 = new Block()
            {
                Name = "Pipe_3_Exit",
                Type = Block.BlockType.Pipe3
            };
            pipe3.Transform.Position = new Vector3(32 * 6, 16 * 6, 0);

            Camera baseCam = new Camera()
            {
                BackGround = Color.FromArgb((byte)((Shared.OverworldBackground >> 24) % 256), (byte)((Shared.OverworldBackground >> 16) % 256), (byte)((Shared.OverworldBackground >> 8) % 256), (byte)((Shared.OverworldBackground >> 0) % 256))
            };

            Mario player = new Mario()
            {
                InitCharacterController = true
            };

            TextBlock MainMenuHeader = new TextBlock()
            {
                FontSize = 4,
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

            //player.Transform.Position = new Vector3(20, 20, 0);
        }

        public override void Set(params string[] Args)
        { }

        public override void Unload()
        { }
    }
}
