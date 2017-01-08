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

        public static float FindMaxZ(this List<GameObject> list) //where T : I3Dimensional
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

        public static List<GameObject> GetGameObjectsInView(this List<GameObject> list)
        {
            List<GameObject> retValue = new List<GameObject>();
            
            int listCount = list.Count;
            for (int i = 0; i < listCount; i++)
            {
                if (list[i].IsInView())
                    retValue.Add(list[i]);
            }

            return retValue;
        }

        public static bool IsInView(this I3Dimensional obj)
        {
            float X = Engine._baseCam != null ? Engine._baseCam.Xoffset : 0;
            float Y = Engine._baseCam != null ? Engine._baseCam.Yoffset : 0;

            return (obj.X + obj.Width >= X && obj.X < X + Engine.Render.RenderWidth && obj.Y + obj.Height >= Y && obj.Y < Y + Engine.Render.RenderHeight);
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
        
        public static T FirstOrDefault<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            List<T> _source = new List<T>(source);

            int _sourceCount = _source.Count;
            for (int i = 0; i < _sourceCount; i++)
            {
                if (predicate(_source[i]))
                    return _source[i];

            }

            return default(T);
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

        public static List<T> ToList<T>(this IEnumerable source)
        {
            return new List<T>(Cast<T>(source));
        }

        private static IEnumerable<T> Cast<T>(IEnumerable source)
        {
            foreach (T obj in source) yield return obj;
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
