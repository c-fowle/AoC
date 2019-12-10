﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC.Common.ExtensionMethods
{
    public static class IEnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> list, Action<T> action)
        {
            foreach (var i in list) action(i);
        }

        public static IList<T> CloneAsList<T>(this IEnumerable<T> list)
        {
            var copy = new List<T>();
            list.ForEach(copy.Add);
            return copy;
        }

        public static IList<List<T>> GetPermutations<T>(this IEnumerable<T> list)
        {
            if (list.Count() == 1) return new List<List<T>>() { new List<T>() { list.Single() } };

            var permutations = new List<List<T>>();

            for (var pos = 0; pos < list.Count(); ++pos)
            {
                var listCopy = list.CloneAsList().ToList();
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
