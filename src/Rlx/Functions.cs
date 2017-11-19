using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rlx
{
    public static class Functions
    {
        public static Option<T> None<T>()
            => Option<T>.None;

        public static OptionAsync<T> NoneAsync<T>()
            => OptionAsync<T>.None;

        public static Option<T> Some<T>(T value)
            => new Option<T>(value);

        public static Option<T> Some<T>(T value, IEqualityComparer<T> equalityComparer)
            => new Option<T>(value, equalityComparer);

        public static OptionAsync<T> Some<T>(Task<T> task)
            => new OptionAsync<T>(task);

        public static OptionAsync<T> Some<T>(Func<Task<T>> fn)
            => new OptionAsync<T>(fn());

        public static Result<TValue, TError> Ok<TValue, TError>(TValue value)
            => new Result<TValue, TError>(value);

        public static Result<TValue, TError> Ok<TValue, TError>(TValue value, IEqualityComparer<TValue> equalityComparer)
            => new Result<TValue, TError>(value, equalityComparer);

        public static ResultAsync<TValue, TError> Ok<TValue, TError>(Task<TValue> task)
            => new ResultAsync<TValue, TError>(task);

        public static Result<TValue, TError> Error<TValue, TError>(TError error)
            => new Result<TValue, TError>(error);

        public static Result<TValue, TError> Error<TValue, TError>(TError error, IEqualityComparer<TError> equalityComparer)
            => new Result<TValue, TError>(error, equalityComparer);

        public static ResultAsync<TValue, TError> Error<TValue, TError>(Task<TError> error)
            => new ResultAsync<TValue, TError>(error);
    }
}
