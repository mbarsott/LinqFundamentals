using System.Collections.Generic;

namespace Features.Linq
{
    public static class MyLinq
    {
        public static int Count<T>(this IEnumerable<T> sequence)
        {
            int i = 0;
            foreach (var s in sequence)
            {
                i++;
            }

            return i;
        }
    }
}
