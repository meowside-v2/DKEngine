/*
* (C) 2017 David Knieradl 
*/


using DKEngine.Core.Components;
using System.Collections.Generic;
using System.Linq;

namespace DKEngine.Core.Ext
{
    public static class Extensions
    {
        public static void AddSafe<DataValue>(this Dictionary<string, DataValue> Destination, Component Target)
            where DataValue : Component
        {
            string Key = Target.Name;

            if (Engine.LoadingScene.ComponentCount.ContainsKey(Target.Name))
            {
                Target.Name = string.Format("{0}_(Copy {1})", Key, Engine.LoadingScene.ComponentCount[Target.Name]++);
                Key = Target.Name;
            }
            
            else
            {
                Engine.LoadingScene.ComponentCount.Add(Key, 1);
            }

            /*try
            {
                position = Engine.LoadingScene.ComponentCount[Key];
            }
            catch
            {
                Engine.LoadingScene.ComponentCount[Key] = 0;
            }*/

            Destination.Add(Key, Target as DataValue);
        }

        public static void AddAll<T>(this List<T> list, params T[] stuff)
        {
            foreach (var item in stuff)
            {
                list.Add(item);
            }
        }

        public static float FindMaxZ(this List<GameObject> list) //where T : I3Dimensional
        {
            return list.Max(obj => obj.Transform.Position.Z);
            /*float z2 = float.MinValue;

            int listCount = list.Count;
            for(int i = 0; i < listCount; i++)
            {
                if (list[i].Z > z2)
                    z2 = list[i].Z;
            }

            return z2;*/
        }
        
        public static List<GameObject> GetGameObjectsInView(this IEnumerable<GameObject> list)
        {
            return list.Where(obj => obj.IsInView).ToList();
            
            /*List<GameObject> retValue = new List<GameObject>();
            
            int listCount = list.Count;
            for (int i = 0; i < listCount; i++)
            {
                if (list[i].IsInView())
                    retValue.Add(list[i]);
            }

            return retValue;*/
        }

        /*public static bool IsInView(this GameObject obj)
        {
            float X = obj.IsGUI ? 0 : Engine.BaseCam != null ? Engine.BaseCam.X : 0;
            float Y = obj.IsGUI ? 0 : Engine.BaseCam != null ? Engine.BaseCam.Y : 0;

            return (obj.Transform.Position.X + obj.Transform.Dimensions.Width >= X && obj.Transform.Position.X < X + Engine.Render.RenderWidth && obj.Transform.Position.Y + obj.Transform.Dimensions.Height >= Y && obj.Transform.Position.Y < Y + Engine.Render.RenderHeight);
        }*/

        /*public static bool IsUnsupportedEscapeSequence(this char letter)
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
        }*/
        
        /*public static T FirstOrDefault<T>(this IEnumerable<T> source, Func<T, bool> predicate)
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

        public static void InitCollider(this Collider Collider, Collider newCollider)
        {
            if (Collider != null)
                Engine.Collidable.Remove(Collider);

            Collider = newCollider;
        }*/
    }
}
