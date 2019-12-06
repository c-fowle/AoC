using System;
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

        public static IEnumerable<TResult> ForEach<T, TResult>(this IEnumerable<T> list, Func<T, TResult> function)
        {
            var result = new List<TResult>();
            foreach (var i in list) result.Add(function(i));
            return result;
        }

        public static IList<T> CloneAsList<T>(this IEnumerable<T> list)
        {
            var copy = new List<T>();
            list.ForEach(copy.Add);
            return copy;
        }
    }
}
