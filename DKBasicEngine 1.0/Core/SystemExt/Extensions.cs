using System;
using System.Collections.Generic;
using System.Drawing;

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
            double z2 = double.MinValue;

            for(int i = 0; i < list.Count; i++)
            {
                if (list[i].Z > z2)
                    z2 = list[i].Z;
            }

            return z2;
        }

        public static List<I3Dimensional> GetGameObjectsInView(this List<I3Dimensional> list)
        {
            List<I3Dimensional> retValue = new List<I3Dimensional>();

            lock (list)
            {
                for(int i = 0; i < list.Count; i++)
                {
                    if (list[i].IsInView())
                        retValue.Add(list[i]);
                }

                return retValue;
                //return list.Where(item => item.IsInView()).ToList();
            }
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

        public static bool BufferIsFull(this byte[] buffer, byte value, int EachIndex)
        {
            for (int i = 0; i < buffer.Length; i += EachIndex)
            {
                if (buffer[i] != value)
                    return false;
            }

            return true;
        }

        public static void FillEach(this byte[] buffer, byte value, int EachIndex)
        {
            for(int i = 0; i < buffer.Length; i+= EachIndex)
            {
                buffer[i] = value;
            }
        }

        public static void Populate<T>(this T[] arr, T value)
        {
            for (int i = 0; i < arr.Length; i++)
                arr[i] = value;
        }

        public static void Populate<T>(this T[,] arr, int arrHeight, int arrWidth, T value)
        {
            for (int i = 0; i < arrHeight; i++)
                for (int j = 0; j < arrWidth; j++)
                    arr[j, i] = value;
        }

        public static void Populate<T>(this T[,,] arr, int arrHeight, int arrWidth, int arrDepth, T value)
        {
            for (int d = 0; d < arrDepth; d++)
                for (int i = 0; i < arrHeight; i++)
                    for (int j = 0; j < arrWidth; j++)
                        arr[j, i, d] = value;
        }

        public static bool IsOnScreen(double x, double y)
        {
            return x >= 0 && x < Engine.Render.RenderWidth && y >= 0 && y < Engine.Render.RenderHeight;
        }

        public static Color MixPixel(Color top, Color bottom)
        {
            double opacityTop = (double)1 / 255 * top.A;
            
            byte newA = (byte)(top.A + bottom.A >= 255 ? 255 : top.A + bottom.A);
            byte A = (byte)(newA - top.A);

            double opacityBottom = (double)1 / 255 * A;

            byte R = (byte)(top.R * opacityTop + bottom.R * opacityBottom);
            byte G = (byte)(top.G * opacityTop + bottom.G * opacityBottom);
            byte B = (byte)(top.B * opacityTop + bottom.B * opacityBottom);

            return Color.FromArgb(newA, R, G, B);
        }

        public static TSource FirstOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            List<TSource> _source = source.ToList();

            for (int i = 0; i < _source.Count; i++)
            {
                if (predicate(_source[i]))
                    return _source[i];

            }

            return default(TSource);
        }

        public static List<T> Where<T>(this List<T> list, Func<T, bool> predicate)
        {
            List<T> retValue = new List<T>();

            for(int i = 0; i < list.Count; i++)
            {
                if (predicate(list[i]))
                    retValue.Add(list[i]);
            }

            return retValue;
        }

        public static List<T> ToList<T>(this IEnumerable<T> source)
        {
            return new List<T>(source);
        }

        public static bool All<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            List<T> _source = source.ToList();

            for(int i = 0; i < _source.Count; i++)
            {
                if (!predicate(_source[i]))
                    return false;
            }

            return true;
        }
    }
}
