﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKBasicEngine_1_0
{
    public class Shared
    {
        public static int RenderWidth = 640;
        public static int RenderHeight = 360;

        public static bool IsEscapeSequence(char letter)
        {
            switch (letter)
            {
                case '\0':
                    return true;

                case '\a':
                    return true;

                case '\b':
                    return true;

                case '\f':
                    return true;

                case '\n':
                    return true;

                case '\r':
                    return true;

                case '\t':
                    return true;

                case '\v':
                    return true;

                case '\'':
                    return true;

                case '\"':
                    return true;
            }

            return false;
        }
    }
}
