using System;
using System.Collections;
using System.Collections.Generic;

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

            int listCount = list.Count;
            for(int i = 0; i < listCount; i++)
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
                int listCount = list.Count;
                for (int i = 0; i < listCount; i++)
                {
                    if (list[i].IsInView())
                        retValue.Add(list[i]);
                }

                return retValue;
            }
        }

        public static bool IsInView(this I3Dimensional obj)
        {
            float X = Engine._baseCam != null ? Engine._baseCam.Xoffset : 0;
            float Y = Engine._baseCam != null ? Engine._baseCam.Yoffset : 0;

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

        public static bool IsOnScreen(float x, float y)
        {
            return x >= 0 && x < Engine.Render.RenderWidth && y >= 0 && y < Engine.Render.RenderHeight;
        }


        public static TSource FirstOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            List<TSource> _source = new List<TSource>(source);

            int _sourceCount = _source.Count;
            for (int i = 0; i < _sourceCount; i++)
            {
                if (predicate(_source[i]))
                    return _source[i];

            }

            return default(TSource);
        }

        public static List<T> Where<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            List<T> retValue = new List<T>();
            List<T> _source = new List<T>(source);

            int _sourceCount = _source.Count;
            for (int i = 0; i < _sourceCount; i++)
            {
                if (predicate(_source[i]))
                    retValue.Add(_source[i]);
            }

            return retValue;
        }

        public static T[] ToArray<T>(this List<T> source)
        {
            int size = source.Count;
            T[] retValue = new T[size];

            for (int i = 0; i < size; i++)
                retValue[i] = source[i];

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
            List<T> _source = new List<T>(source);

            int _sourceCount = _source.Count; 
            for(int i = 0; i < _sourceCount; i++)
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

        public static bool Contains<T>(this IEnumerable<T> source, T value)
        {
            List<T> _source = new List<T>(source);

            int _sourceCount = _source.Count;
            for (int i = 0; i < _sourceCount; i++)
            {
                if (_source[i].Equals(value))
                    return true;
            }

            return false;
        }
    }
}
