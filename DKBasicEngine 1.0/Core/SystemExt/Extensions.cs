using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKBasicEngine_1_0
{
    public static class Extensions
    {
        public static void AddAll<T>(this List<T> list, params T[] stuff)
        {
            foreach (var item in stuff)
            {
                list.Add(item);
            }
        }

        public static double FindMaxZ(this List<I3Dimensional> list)
        {
            return list.Max(item => item.Z);
        }

        public static List<I3Dimensional> GetGameObjectsInView(this List<I3Dimensional> list, int Width, int Height, int XOffset, int YOffset)
        {
            List<I3Dimensional> retValue;

            lock (list)
            {
                retValue = list.Where(item => IsInView(item, XOffset, YOffset, Width + 5, Height + 5)).ToList();
            }

            return retValue;
        }

        public static bool IsInView(this I3Dimensional obj, double x, double y, int Width, int Height)
        {
            return (obj.X + obj.width >= x - 5 && obj.X < x + Width && obj.Y + obj.height >= y - 5 && obj.Y < y + Height);
        }

        public static bool IsEscapeSequence(this char letter)
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
