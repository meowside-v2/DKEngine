using DKEngine.Core.Components;
using DKEngine.Core.UI;
using MarIO.Assets.Models;
using MarIO.Assets.Models.Miscellaneous;
using MarIO.Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MarIO
{
    static class Shared
    {
        static class Mechanics
        {
            public static SoundOutput FXPlayer;
            public static SoundSource FXSoundSource { get { return FXPlayer.SoundSource; } }

            private static byte _coinsCount = 0;

            public static short GameScore { get; set; } = 0;
            public static byte Lives { get; set; } = 3;

            public static byte CoinsCount
            {
                get { return _coinsCount; }
                set
                {
                    _coinsCount = value;
                    if (_coinsCount > 99)
                    {
                        Lives++;
                        _coinsCount = 0;
                    }
                }
            }

            public readonly static Stopwatch TimeCounter = new Stopwatch();

            private readonly static TimeSpan LevelTime = new TimeSpan(0, 10, 0);

            public static TimeSpan TimeLeft
            {
                get { return LevelTime - TimeCounter.Elapsed; }
            }

            public static Mario.State MarioCurrentState = Mario.State.Small;

            public const uint OverworldBackground = 0xFF20CCCC;
            public const uint WorldChangeBackground = 0x00000000;

            public const int GOOMBA_POINTS = 100;
            public const int COIN_SCORE = 100;
        }

        public static class AnimatedWorldReferences
        {
            public static List<Block> BlocksToUpdate = new List<Block>();
            public static List<float> BlocksStartPositions = new List<float>();

            public static List<TextBlock> FloatingTexts = new List<TextBlock>();
            public static List<float> FloatingTextStartPosition = new List<float>();

            public static Stack<Block> SpecialActions = new Stack<Block>();

            public static List<Coin> FloatingCoins = new List<Coin>();
            public static List<float> FloatingCoinsStartPosition = new List<float>();
        }

        public static class Assets
        {
            public static class Sounds
            {
                public const string OVERWORLD_THEME = @".\Assets\Sounds\Overworld_theme.mp3";
                public const string MARIO_JUMP_FX = @".\Assets\Sounds\smb_jump-small.mp3";
                public const string PIPE_ENTER_FX = @".\Assets\Sounds\smb_pipe.mp3";
                public const string COIN_GET_FX = @".\Assets\Sounds\smb_coin.mp3";
                public const string UP_1_FX = @".\Assets\Sounds\smb_1-up.mp3";
                public const string BREAK_BLOCK_FX = @".\Assets\Sounds\smb_breakblock.mp3";
                public const string MARIO_DIE_FX = @".\Assets\Sounds\smb_mariodie.mp3";
                public const string POWER_UP_FX = @".\Assets\Sounds\smb_powerup.mp3";
                public const string STOMP_FX = @".\Assets\Sounds\smb_stomp.mp3";
            }

            public static class Animations
            {
                #region Mario

                /*--------------- SMALL -----------------*/

                public const string MARIO_IDLE_LEFT = "idle_left";
                public const string MARIO_IDLE_LEFT_MAT = "mario_left";

                public const string MARIO_IDLE_RIGHT = "idle_right";
                public const string MARIO_IDLE_RIGHT_MAT = "mario_right";

                public const string MARIO_MOVE_LEFT = "left_move";
                public const string MARIO_MOVE_LEFT_MAT = "mario_move_left";

                public const string MARIO_MOVE_RIGHT = "right_move";
                public const string MARIO_MOVE_RIGHT_MAT = "mario_move_right";

                public const string MARIO_JUMP_LEFT = "left_jump";
                public const string MARIO_JUMP_LEFT_MAT = "mario_jump_left";

                public const string MARIO_JUMP_RIGHT = "right_jump";
                public const string MARIO_JUMP_RIGHT_MAT = "mario_jump_right";

                public const string MARIO_DEAD = "dead";
                public const string MARIO_DEAD_MAT = "mario_dead";


                /*--------------- SUPER -----------------*/

                public const string MARIO_SUPER_IDLE_LEFT;
                public const string MARIO_SUPER_IDLE_LEFT_MAT;

                public const string MARIO_SUPER_IDLE_RIGHT;
                public const string MARIO_SUPER_IDLE_RIGHT_MAT;

                public const string MARIO_SUPER_MOVE_LEFT;
                public const string MARIO_SUPER_MOVE_LEFT_MAT;

                public const string MARIO_SUPER_MOVE_RIGHT;
                public const string MARIO_SUPER_MOVE_RIGHT_MAT;

                public const string MARIO_SUPER_JUMP_LEFT;
                public const string MARIO_SUPER_JUMP_LEFT_MAT;

                public const string MARIO_SUPER_JUMP_RIGHT;
                public const string MARIO_SUPER_JUMP_RIGHT_MAT;

                public const string MARIO_SUPER_DEAD;
                public const string MARIO_SUPER_DEAD_MAT;


                /*--------------- FIRE -----------------*/

                public const string MARIO_FIRE_IDLE_LEFT;
                public const string MARIO_FIRE_IDLE_LEFT_MAT;

                public const string MARIO_FIRE_IDLE_RIGHT;
                public const string MARIO_FIRE_IDLE_RIGHT_MAT;

                public const string MARIO_FIRE_MOVE_LEFT;
                public const string MARIO_FIRE_MOVE_LEFT_MAT;

                public const string MARIO_FIRE_MOVE_RIGHT;
                public const string MARIO_FIRE_MOVE_RIGHT_MAT;

                public const string MARIO_FIRE_JUMP_LEFT;
                public const string MARIO_FIRE_JUMP_LEFT_MAT;

                public const string MARIO_FIRE_JUMP_RIGHT;
                public const string MARIO_FIRE_JUMP_RIGHT_MAT;

                public const string MARIO_FIRE_DEAD;
                public const string MARIO_FIRE_DEAD_MAT;


                /*--------------- INVINCIBLE -----------------*/

                public const string MARIO_INVINCIBLE_IDLE_LEFT;
                public const string MARIO_INVINCIBLE_IDLE_LEFT_MAT;

                public const string MARIO_INVINCIBLE_IDLE_RIGHT;
                public const string MARIO_INVINCIBLE_IDLE_RIGHT_MAT;

                public const string MARIO_INVINCIBLE_MOVE_LEFT;
                public const string MARIO_INVINCIBLE_MOVE_LEFT_MAT;

                public const string MARIO_INVINCIBLE_MOVE_RIGHT;
                public const string MARIO_INVINCIBLE_MOVE_RIGHT_MAT;

                public const string MARIO_INVINCIBLE_JUMP_LEFT;
                public const string MARIO_INVINCIBLE_JUMP_LEFT_MAT;

                public const string MARIO_INVINCIBLE_JUMP_RIGHT;
                public const string MARIO_INVINCIBLE_JUMP_RIGHT_MAT;

                public const string MARIO_INVINCIBLE_DEAD;
                public const string MARIO_INVINCIBLE_DEAD_MAT;

                #endregion Mario
            }
        }
    }
}