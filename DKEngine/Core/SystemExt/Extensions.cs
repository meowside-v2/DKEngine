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

            Destination.Add(Key, Target as DataValue);
        }

        public static void AddAll<T>(this List<T> list, params T[] stuff)
        {
            foreach (var item in stuff)
            {
                list.Add(item);
            }
        }

        public static float FindMaxZ(this List<GameObject> list)
        {
            return list.Max(obj => obj.Transform.Position.Z);
        }

        public static List<GameObject> GetGameObjectsInView(this IEnumerable<GameObject> list)
        {
            return list.Where(obj => obj.IsInView).ToList();
        }
    }
}