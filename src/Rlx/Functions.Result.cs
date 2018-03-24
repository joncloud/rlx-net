using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rlx
{
    public static partial class Functions
    {
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
