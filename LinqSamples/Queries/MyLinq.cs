using System;
using System.Collections.Generic;

namespace Queries
{
    public static class MyLinq
    {
        public static IEnumerable<double> Random()
        {
            var random = new Random();
            while (true)
            {
                yield return random.NextDouble();
            }
        }

        public static IEnumerable<T> Filter<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            //            var result = new List<T>();

            foreach (var s in source)
            {
                if (predicate(s))
                {
                    //                    result.Add(s);
                    yield return s;
                }
            }

            //            return result;
        }
    }
}
