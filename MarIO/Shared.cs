using DKEngine.Core.Components;
using DKEngine.Core.UI;
using MarIO.Assets.Models;
using MarIO.Assets.Models.Miscellaneous;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MarIO
{
    public static class Shared
    {
        public static class Mechanics
        {
            public static SoundOutput FXPlayer;
            public static SoundSource FXSoundSource { get { return FXPlayer.SoundSource; } }

            private static byte _coinsCount = 0;

            public static string GameScoreStr { get { return string.Format($"{GameScore:00000000}"); } }
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

            private readonly static TimeSpan LevelTime = new TimeSpan(0, 5, 0);

            public static TimeSpan TimeLeft
            {
                get { return LevelTime - TimeCounter.Elapsed; }
            }

            //public static
            public static Type LastWorldType;

            public static Mario.State MarioCurrentState
            {
                get;
                set;
            } = Mario.State.Super;

            public const uint OverworldBackground = 0xFF30A0DD;
            public const uint WorldChangeBackground = 0x00000000;

            public const int GOOMBA_POINTS = 100;
            public const int COIN_SCORE = 100;
            public const int MUSHROOM_SCORE = 200;
            public const int FLOWER_SCORE = 300;
            public const int STAR_SCORE = 500;
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
                public const string OVERWORLD_THEME = @".\Assets\Sounds\Overworld_Theme.mp3";
                public const string MARIO_JUMP_FX = @".\Assets\Sounds\smb_jump-small.mp3";
                public const string PIPE_ENTER_FX = @".\Assets\Sounds\smb_pipe.mp3";
                public const string COIN_GET_FX = @".\Assets\Sounds\smb_coin.mp3";
                public const string UP_1_FX = @".\Assets\Sounds\smb_1-up.mp3";
                public const string BREAK_BLOCK_FX = @".\Assets\Sounds\smb_breakblock.mp3";
                public const string MARIO_DIE_FX = @".\Assets\Sounds\smb_mariodie.mp3";
                public const string POWER_UP_FX = @".\Assets\Sounds\smb_powerup.mp3";
                public const string STOMP_FX = @".\Assets\Sounds\smb_stomp.mp3";

                public static readonly Sound OVERWORLD_THEME_SOUND = new Sound(OVERWORLD_THEME);
                public static readonly Sound FX_MARIO_JUMP_SOUND = new Sound(MARIO_JUMP_FX);
                public static readonly Sound FX_PIPE_ENTER_SOUND = new Sound(PIPE_ENTER_FX);
                public static readonly Sound FX_1_UP_SOUND = new Sound(UP_1_FX);
                public static readonly Sound FX_BREAK_BLOCK_SOUND = new Sound(BREAK_BLOCK_FX);
                public static readonly Sound FX_MARIO_DIE_SOUND = new Sound(MARIO_DIE_FX);
                public static readonly Sound FX_POWER_UP_SOUND = new Sound(POWER_UP_FX);
                public static readonly Sound FX_STOMP_SOUND = new Sound(STOMP_FX);
            }

            public static class Animations
            {
                #region Mario

                private const string POWERUP_LEFT = "powerup_left";
                private const string POWERUP_LEFT_MAT = "mario_powerup_left";

                private const string POWERUP_RIGHT = "powerup_right";
                private const string POWERUP_RIGHT_MAT = "mario_powerup_right";

                private const string CROUCHING_LEFT = "crouch_left";
                private const string CROUCHING_LEFT_MAT = "mario_crouch_left";

                private const string CROUCHING_RIGHT = "crouch_right";
                private const string CROUCHING_RIGHT_MAT = "mario_crouch_right";

                /*--------------- SMALL -----------------*/

                public const string MARIO_IDLE_LEFT = "idle_left";
                public const string MARIO_IDLE_LEFT_MAT = "mario_left";

                public const string MARIO_IDLE_RIGHT = "idle_right";
                public const string MARIO_IDLE_RIGHT_MAT = "mario_right";

                public const string MARIO_MOVE_LEFT = "move_left";
                public const string MARIO_MOVE_LEFT_MAT = "mario_move_left";

                public const string MARIO_MOVE_RIGHT = "move_right";
                public const string MARIO_MOVE_RIGHT_MAT = "mario_move_right";

                public const string MARIO_JUMP_LEFT = "jump_left";
                public const string MARIO_JUMP_LEFT_MAT = "mario_jump_left";

                public const string MARIO_JUMP_RIGHT = "jump_right";
                public const string MARIO_JUMP_RIGHT_MAT = "mario_jump_right";

                public const string MARIO_DEAD = "dead";
                public const string MARIO_DEAD_MAT = "mario_dead";

                public const string MARIO_CROUCHING_LEFT = CROUCHING_LEFT;
                public const string MARIO_CROUCHING_LEFT_MAT = MARIO_IDLE_LEFT_MAT;

                public const string MARIO_CROUCHING_RIGHT = CROUCHING_RIGHT;
                public const string MARIO_CROUCHING_RIGHT_MAT = MARIO_IDLE_RIGHT_MAT;

                /*--------------- SUPER -----------------*/

                public const string MARIO_SUPER_IDLE_LEFT = "super_" + MARIO_IDLE_LEFT;
                public const string MARIO_SUPER_IDLE_LEFT_MAT = "super_" + MARIO_IDLE_LEFT_MAT;

                public const string MARIO_SUPER_IDLE_RIGHT = "super_" + MARIO_IDLE_RIGHT;
                public const string MARIO_SUPER_IDLE_RIGHT_MAT = "super_" + MARIO_IDLE_RIGHT_MAT;

                public const string MARIO_SUPER_MOVE_LEFT = "super_" + MARIO_MOVE_LEFT;
                public const string MARIO_SUPER_MOVE_LEFT_MAT = "super_" + MARIO_MOVE_LEFT_MAT;

                public const string MARIO_SUPER_MOVE_RIGHT = "super_" + MARIO_MOVE_RIGHT;
                public const string MARIO_SUPER_MOVE_RIGHT_MAT = "super_" + MARIO_MOVE_RIGHT_MAT;

                public const string MARIO_SUPER_JUMP_LEFT = "super_" + MARIO_JUMP_LEFT;
                public const string MARIO_SUPER_JUMP_LEFT_MAT = "super_" + MARIO_JUMP_LEFT_MAT;

                public const string MARIO_SUPER_JUMP_RIGHT = "super_" + MARIO_JUMP_RIGHT;
                public const string MARIO_SUPER_JUMP_RIGHT_MAT = "super_" + MARIO_JUMP_RIGHT_MAT;

                public const string MARIO_SUPER_POWERUP_LEFT = "super_" + POWERUP_LEFT;
                public const string MARIO_SUPER_POWERUP_LEFT_MAT = "super_" + POWERUP_LEFT_MAT;

                public const string MARIO_SUPER_POWERUP_RIGHT = "super_" + POWERUP_RIGHT;
                public const string MARIO_SUPER_POWERUP_RIGHT_MAT = "super_" + POWERUP_RIGHT_MAT;

                public const string MARIO_SUPER_CROUCHING_LEFT = "super_" + CROUCHING_LEFT;
                public const string MARIO_SUPER_CROUCHING_LEFT_MAT = "super_" + CROUCHING_LEFT_MAT;

                public const string MARIO_SUPER_CROUCHING_RIGHT = "super_" + CROUCHING_RIGHT;
                public const string MARIO_SUPER_CROUCHING_RIGHT_MAT = "super_" + CROUCHING_RIGHT_MAT;

                /*--------------- FIRE -----------------*/

                public const string MARIO_FIRE_IDLE_LEFT = "fire_" + MARIO_IDLE_LEFT;
                public const string MARIO_FIRE_IDLE_LEFT_MAT = "fire_" + MARIO_IDLE_LEFT_MAT;

                public const string MARIO_FIRE_IDLE_RIGHT = "fire_" + MARIO_IDLE_RIGHT;
                public const string MARIO_FIRE_IDLE_RIGHT_MAT = "fire_" + MARIO_IDLE_RIGHT_MAT;

                public const string MARIO_FIRE_MOVE_LEFT = "fire_" + MARIO_MOVE_LEFT;
                public const string MARIO_FIRE_MOVE_LEFT_MAT = "fire_" + MARIO_MOVE_LEFT_MAT;

                public const string MARIO_FIRE_MOVE_RIGHT = "fire_" + MARIO_MOVE_RIGHT;
                public const string MARIO_FIRE_MOVE_RIGHT_MAT = "fire_" + MARIO_MOVE_RIGHT_MAT;

                public const string MARIO_FIRE_JUMP_LEFT = "fire_" + MARIO_JUMP_LEFT;
                public const string MARIO_FIRE_JUMP_LEFT_MAT = "fire_" + MARIO_JUMP_LEFT_MAT;

                public const string MARIO_FIRE_JUMP_RIGHT = "fire_" + MARIO_JUMP_RIGHT;
                public const string MARIO_FIRE_JUMP_RIGHT_MAT = "fire_" + MARIO_JUMP_RIGHT_MAT;

                public const string MARIO_FIRE_POWERUP_LEFT = "fire_" + POWERUP_LEFT;
                public const string MARIO_FIRE_POWERUP_LEFT_MAT = "fire_" + POWERUP_LEFT_MAT;

                public const string MARIO_FIRE_POWERUP_RIGHT = "fire_" + POWERUP_RIGHT;
                public const string MARIO_FIRE_POWERUP_RIGHT_MAT = "fire_" + POWERUP_RIGHT_MAT;

                public const string MARIO_FIRE_CROUCHING_LEFT = "fire_" + CROUCHING_LEFT;
                public const string MARIO_FIRE_CROUCHING_LEFT_MAT = "fire_" + CROUCHING_LEFT_MAT;

                public const string MARIO_FIRE_CROUCHING_RIGHT = "fire_" + CROUCHING_RIGHT;
                public const string MARIO_FIRE_CROUCHING_RIGHT_MAT = "fire_" + CROUCHING_RIGHT_MAT;

                /*--------------- INVINCIBLE -----------------*/

                /*public const string MARIO_INVINCIBLE_IDLE_LEFT;
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
                public const string MARIO_INVINCIBLE_DEAD_MAT;*/

                #endregion Mario
            }
        }
    }
}