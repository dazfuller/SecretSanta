using System;
using System.Collections.Generic;

namespace SecretSanta
{
    public static class IListExtensionMethods
    {
        public static void Swap<T>(this IList<T> list, int a, int b)
        {
            if (a == b) return;
            
            var temp = list[a];
            list[a] = list[b];
            list[b] = temp;
        }

        public static void Shuffle<T>(this IList<T> list, Random random)
        {
            for (var i = 0; i < list.Count; i++)
            {
                list.Swap(i, random.Next(i, list.Count));
            }
        }
    }
}
