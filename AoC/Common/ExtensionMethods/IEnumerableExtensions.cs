﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC.Common.ExtensionMethods
{
    public static class IEnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var i in enumerable) action(i);
        }

        public static IList<T> CloneAsList<T>(this IEnumerable<T> enumerable)
        {
            var copy = new List<T>();
            enumerable.ForEach(copy.Add);
            return copy;
        }

        public static IList<List<T>> GetPermutations<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable.Count() == 1) return new List<List<T>>() { new List<T>() { enumerable.Single() } };

            var permutations = new List<List<T>>();

            for (var pos = 0; pos < enumerable.Count(); ++pos)
            {
                var listCopy = enumerable.CloneAsList().ToList();
                var initial = listCopy[pos];
                listCopy.RemoveAt(pos);

                var sublistPermutations = GetPermutations(listCopy);

                sublistPermutations.ForEach(l =>
                {
                    l.Insert(0, initial);
                    permutations.Add(l.CloneAsList().ToList());
                });
            }

            return permutations;
        }
    }
}
