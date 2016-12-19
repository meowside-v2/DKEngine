using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
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

        public static float FindMaxZ(this List<I3Dimensional> list)
        {
            float z2 = float.MinValue;

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
            }
        }

        public static bool IsInView(this I3Dimensional obj)
        {
            float X = 0;   /*obj is IGraphics ? 0 : Engine._baseCam.Xoffset;*/
            float Y = 0;   /*obj is IGraphics ? 0 : Engine._baseCam.Yoffset;*/

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

        public static bool IsOnScreen(float x, float y)
        {
            return x >= 0 && x < Engine.Render.RenderWidth && y >= 0 && y < Engine.Render.RenderHeight;
        }

        public static Color MixPixel(Color top, Color bottom)
        {
            if(bottom.A > 0)
            {
                float opacityTop = (float)top.A / 255;

                byte newA = (byte)(top.A + bottom.A >= 255 ? 255 : top.A + bottom.A);
                byte A = (byte)(newA - top.A);

                float opacityBottom = (float)A / 255;

                byte R = (byte)(top.R * opacityTop + bottom.R * opacityBottom);
                byte G = (byte)(top.G * opacityTop + bottom.G * opacityBottom);
                byte B = (byte)(top.B * opacityTop + bottom.B * opacityBottom);

                return Color.FromArgb(newA, R, G, B);
            }

            return top;
        }

        public static TSource FirstOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            List<TSource> _source = new List<TSource>(source);

            for (int i = 0; i < _source.Count; i++)
            {
                if (predicate(_source[i]))
                    return _source[i];

            }

            return default(TSource);
        }

        public static List<T> Where<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            List<T> retValue = new List<T>();
            List<T> _source = source.ToList();

            for(int i = 0; i < _source.Count; i++)
            {
                if (predicate(_source[i]))
                    retValue.Add(_source[i]);
            }

            return retValue;
        }

        public static List<T> ToList<T>(this IEnumerable<T> source)
        {
            return new List<T>(source);
        }

        public static List<TResult> ToList<TResult>(this IEnumerable source)
        {
            return new List<TResult>(Cast<TResult>(source));
        }

        private static IEnumerable<TResult> Cast<TResult>(IEnumerable source)
        {
            foreach (TResult obj in source) yield return obj;
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

        public static TSource[] Convert2DArryto1D<TSource>(this TSource[,] source, int width, int height)
        {
            TSource[] retValue = new TSource[source.Length];

            for(int i = 0; i < height; i++)
            {
                for(int j = 0; j < width; j++)
                {
                    retValue[i * width + j] = source[j,i];
                }
            }

            return retValue;
        }

        public static void SpriteChange(Material source, IGraphics destination)
        {

        }
    }
}
