using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rlx
{
    public struct Result<TValue, TError> : IEnumerable<TValue>, IEquatable<Result<TValue, TError>>
    {
        readonly TValue _value;
        readonly IEqualityComparer<TValue> _valueComparer;

        readonly TError _error;
        readonly IEqualityComparer<TError> _errorComparer;

        public Result(TValue value) : this(value, EqualityComparer<TValue>.Default) { }
        public Result(TValue value, IEqualityComparer<TValue> equalityComparer)
        {
            IsOk = true;
            _value = value;
            _valueComparer = equalityComparer;

            _error = default(TError);
            _errorComparer = null;
        }

        public Result(TError error) : this(error, EqualityComparer<TError>.Default) { }
        public Result(TError error, IEqualityComparer<TError> equalityComparer)
        {
            IsOk = false;
            _value = default(TValue);
            _valueComparer = null;

            _error = error;
            _errorComparer = equalityComparer;
        }

        public bool IsOk { get; }
        public bool IsError => !IsOk;

        public Option<TValue> Ok()
        {
            if (IsOk) return Functions.Some(_value);
            return Functions.None<TValue>();
        }

        public Option<TError> Error()
        {
            if (IsOk) return Functions.None<TError>();
            return Functions.Some(_error);
        }

        public Result<TResult, TError> Map<TResult>(Func<TValue, TResult> fn)
        {
            if (IsOk) return new Result<TResult, TError>(fn(_value));
            return new Result<TResult, TError>(_error, _errorComparer);
        }

        public ResultTask<TResult, TError> Map<TResult>(Func<TValue, Task<TResult>> fn)
        {
            if (IsOk) return new ResultTask<TResult, TError>(fn(_value));
            return new ResultTask<TResult, TError>(Task.FromResult(_error));
        }

        public Result<TValue, TResult> MapError<TResult>(Func<TError, TResult> fn)
        {
            if (IsOk) return new Result<TValue, TResult>(_value);
            return new Result<TValue, TResult>(fn(_error));
        }

        public ResultTask<TValue, TResult> MapError<TResult>(Func<TError, Task<TResult>> fn)
        {
            if (IsOk) return new ResultTask<TValue, TResult>(Task.FromResult(_value));
            return new ResultTask<TValue, TResult>(fn(_error));
        }

        public Result<TValue, TError> Or(Result<TValue, TError> result)
        {
            if (IsOk) return this;
            return result;
        }

        public Result<TValue, TError> OrElse(Func<TError, Result<TValue, TError>> fn)
        {
            if (IsOk) return this;
            return fn(_error);
        }

        public ResultTask<TValue, TError> OrElse(Func<TError, ResultTask<TValue, TError>> fn)
        {
            if (IsOk) return new ResultTask<TValue, TError>(Task.FromResult(_value));
            return fn(_error);
        }

        public Result<TResult, TError> And<TResult>(Result<TResult, TError> result)
        {
            if (IsOk) return result;
            return new Result<TResult, TError>(_error);
        }

        public Result<TResult, TError> AndThen<TResult>(Func<TValue, Result<TResult, TError>> fn)
        {
            if (IsOk) return fn(_value);
            return new Result<TResult, TError>(_error);
        }

        public ResultTask<TResult, TError> AndThen<TResult>(Func<TValue, ResultTask<TResult, TError>> fn)
        {
            if (IsOk) return fn(_value);
            return new ResultTask<TResult, TError>(Task.FromResult(_error));
        }

        public TValue Unwrap()
        {
            if (IsOk) return _value;
            throw new RlxException(_error.ToString());
        }

        public TError UnwrapError()
        {
            if (IsOk) throw new RlxException(_value.ToString());
            return _error;
        }

        public TValue UnwrapOr(TValue optionB)
        {
            if (IsOk) return _value;
            return optionB;
        }

        public TValue UnwrapOrElse(Func<TError, TValue> fn)
        {
            if (IsOk) return _value;
            return fn(_error);
        }

        public TValue UnwrapOrDefault()
        {
            if (IsOk) return _value;
            return default(TValue);
        }

        public TValue Expect(string message)
        {
            if (IsOk) return _value;
            throw new RlxException($"{message}: {_error}");
        }

        public TError ExpectError(string message)
        {
            if (IsOk) throw new RlxException($"{message}: {_value}");
            return _error;
        }
        
        public override bool Equals(object obj)
            => obj is Result<TValue, TError> r ? Equals(r) : false;

        public bool Equals(Result<TValue, TError> other)
        {
            if (IsOk && other.IsOk) return _valueComparer.Equals(_value, other._value);
            if (IsError && other.IsError) return _errorComparer?.Equals(_error, other._error) ?? false;
            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + IsOk.GetHashCode();
                if (IsOk) hash = hash * 23 + _valueComparer.GetHashCode(_value);
                else hash = hash * 23 + _errorComparer?.GetHashCode(_error) ?? 1;
                return hash;
            }
        }

        public IEnumerator<TValue> GetEnumerator()
        {
            if (IsOk) yield return _value;
        }

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }

    public static class ResultExtensions
    {
        public static T UnwrapEither<T>(this Result<T, T> result) =>
            result.UnwrapOrElse(error => error);

        public static Task<T> UnwrapEitherAsync<T>(this ResultTask<T, T> result) =>
            result.UnwrapOrElseAsync(error => error);
    }
}
