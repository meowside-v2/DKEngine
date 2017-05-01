using MarIO.Assets.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MarIO
{
    internal static class Shared
    {
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

        public static List<Block> BlocksToUpdate = new List<Block>();
        public static List<float> BlocksStartPositions = new List<float>();

        #region Animations

        #region Mario

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

        #endregion Mario

        #endregion Animations
    }
}