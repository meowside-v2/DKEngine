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
        public static short Points     = 0;
        public static byte  Lives      = 0;
        public static byte  CoinsCount = 0;

        public readonly static Stopwatch TimeCounter = new Stopwatch();

        private readonly static TimeSpan LevelTime = new TimeSpan(0, 10, 0);
        public static TimeSpan TimeLeft
        {
            get { return LevelTime - TimeCounter.Elapsed; }
        }

        public static Mario.State MarioCurrentState = Mario.State.Small;
    }
}
