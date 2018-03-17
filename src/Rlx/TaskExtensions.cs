using System;
using System.Threading.Tasks;

namespace Rlx
{
    static class TaskExtensions
    {
        public static async Task<TResult> Select<TResult>(this Task task, Func<TResult> selector)
        {
            await task;
            return selector();
        }

        public static async Task<TResult> Select<TSource, TResult>(this Task<TSource> task, Func<TSource, TResult> selector)
        {
            var result = await task;
            return selector(result);
        }

        public static async Task<TResult> Select<TResult>(this Task task, Func<Task<TResult>> selector)
        {
            await task;
            return await selector();
        }

        public static async Task<TResult> Select<TSource, TResult>(this Task<TSource> task, Func<TSource, Task<TResult>> selector)
        {
            var source = await task;
            return await selector(source);
        }
    }
}
