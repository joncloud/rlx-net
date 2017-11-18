using System;
using System.Threading.Tasks;

namespace Rlx
{
    static class TaskExtensions
    {
        public static async Task<TResult> Select<TSource, TResult>(this Task<TSource> task, Func<TSource, TResult> selector)
        {
            var result = await task;
            return selector(result);
        }
        public static async Task<TResult> Select<TSource, TResult>(this Task<TSource> task, Func<TSource, Task<TResult>> selector)
        {
            var task2 = await task;
            return await selector(task2);
        }
    }
}
