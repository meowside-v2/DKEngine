using MarIO.Assets.Models;
using System;
using System.Collections.Generic;
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

        public static Mario.State MarioCurrentState = Mario.State.Small;
    }
}
