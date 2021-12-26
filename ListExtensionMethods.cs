using System;
using System.Collections.Generic;

namespace SecretSanta
{
    public static class ListExtensionMethods
    {
        private static void Swap<T>(this IList<T> list, int a, int b)
        {
            if (a == b) return;
            (list[a], list[b]) = (list[b], list[a]);
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