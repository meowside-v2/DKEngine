using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKBasicEngine_1_0
{
    public static class ListExtension
    {
        public static void AddAll<T>(this List<T> list, params T[] stuff)
        {
            foreach (var item in stuff)
            {
                list.Add(item);
            }
        }
    }
}
