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

        public static List<I3Dimensional> GetGameObjectsInView(this List<I3Dimensional> list)
        {
            List<I3Dimensional> retValue;

            lock (list)
            {
                retValue = list.Where(item => item.IsInView()).ToList();
            }

            return retValue;
        }

        public static bool IsInView(this I3Dimensional obj)
        {
            double X = Engine._baseCam != null ? Engine._baseCam.RenderXOffset : 0;
            double Y = Engine._baseCam != null ? Engine._baseCam.RenderYOffset : 0;

            return (obj.X + obj.width >= X && obj.X < X + Engine.Render.RenderWidth && obj.Y + obj.height >= Y && obj.Y < Y + Engine.Render.RenderHeight);
        }

        public static bool IsUnsupportedEscapeSequence(this char letter)
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

                /*case '\n':
                    return true;*/

                /*case '\r':
                    return true;*/

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
