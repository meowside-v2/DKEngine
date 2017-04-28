using MarIO.Assets.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarIO
{
    static class Shared
    {
        private static byte _coinsCount = 0;

        public static short Points { get; set; }
        public static byte Lives { get; set; }
        public static byte CoinsCount
        {
            get { return _coinsCount; }
            set
            {
                _coinsCount = value;
                if(_coinsCount > 99)
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
    }
}
