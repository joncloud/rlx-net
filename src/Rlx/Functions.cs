using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rlx
{
    public static class Functions
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

        public static Result<TValue, TError> Ok<TValue, TError>(TValue value)
            => new Result<TValue, TError>(value);

        public static Result<TValue, TError> Ok<TValue, TError>(TValue value, IEqualityComparer<TValue> equalityComparer)
            => new Result<TValue, TError>(value, equalityComparer);

        public static ResultTask<TValue, TError> Ok<TValue, TError>(Task<TValue> task)
            => new ResultTask<TValue, TError>(task);

        public static Result<TValue, TError> Error<TValue, TError>(TError error)
            => new Result<TValue, TError>(error);

        public static Result<TValue, TError> Error<TValue, TError>(TError error, IEqualityComparer<TError> equalityComparer)
            => new Result<TValue, TError>(error, equalityComparer);

        public static ResultTask<TValue, TError> Error<TValue, TError>(Task<TError> error)
            => new ResultTask<TValue, TError>(error);
    }
}
