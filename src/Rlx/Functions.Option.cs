using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rlx
{
    public static partial class Functions
    {
        public static Option<T> None<T>()
            => Option<T>.None;

        public static OptionTask<T> NoneAsync<T>()
            => OptionTask<T>.None;

        public static Option<T> Some<T>(T value)
            => new Option<T>(value);

        public static Option<T> Some<T>(T value, IEqualityComparer<T> equalityComparer)
            => new Option<T>(value, equalityComparer);

        public static OptionTask<T> Some<T>(Task<T> task)
            => new OptionTask<T>(task);

        public static OptionTask<T> Some<T>(Func<Task<T>> fn)
            => new OptionTask<T>(fn());
    }
}
