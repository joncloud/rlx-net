using System.Collections.Generic;
using System.Linq;
using static Rlx.Functions;

namespace Rlx
{
    public static class LinqExtensions
    {
        public static Option<T> FirstOrOption<T>(this IEnumerable<T> source)
        {
            using (var enumerator = source.GetEnumerator())
            {
                if (enumerator.MoveNext()) return enumerator.Current.ToOption();
                return None<T>();
            }
        }

        public static Option<T> FirstOrOption<T>(this IQueryable<T> source) =>
             source.Take(1).AsEnumerable().FirstOrOption();
    }
}
