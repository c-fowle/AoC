using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC.Common.Helpers
{
    public static class MathHelper
    {
        public static IList<int> GetFactors(int n) => GetFactors((long)n).Select(i => (int)i).ToList();
        public static IList<long> GetFactors(long n)
        {
            var factors = new List<long>();
            for (var factor = 1; factor <= Math.Sqrt(n); ++factor)
            {
                if ((n % factor) != 0) continue;
                factors.Add(factor);
                factors.Add(n / factor);
            }
            return factors;
        }

        public static int? GetHighestCommonFactor(int a, int b) => (int?)GetHighestCommonFactor((long)a, (long)b);
        public static long? GetHighestCommonFactor(long a, long b)
        {
            var modA = Math.Abs(a);
            var modB = Math.Abs(b);

            if (modA == 0 && modB == 0) return null;
            if (modA == 0 && modB != 0) return modB;
            if (modA != 0 && modB == 0) return modA;
            if (modA == modB) return modA;

            var bFactors = GetFactors(b);
            return GetFactors(a).Where(f => bFactors.Contains(f)).OrderByDescending(i => i).FirstOrDefault();
        }

        public static long GetLowestCommonMultiple(int a, int b) => GetLowestCommonMultiple((long)a, (long)b);
        public static long GetLowestCommonMultiple(long a, long b)
        {
            var hcf = GetHighestCommonFactor(a, b);
            if (!hcf.HasValue) return a * b;
            return (a / hcf.Value) * b;
        }
    }
}
