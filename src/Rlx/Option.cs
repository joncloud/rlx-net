using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rlx
{
    public struct Option<T> : IEnumerable<T>, IEquatable<Option<T>>
    {
        public static readonly Option<T> None = new Option<T>();

        readonly T _value;
        readonly IEqualityComparer<T> _equalityComparer;
        public Option(T value) : this(value, EqualityComparer<T>.Default) { }
        public Option(T value, IEqualityComparer<T> equalityComparer)
        {
            IsSome = true;
            _value = value;
            _equalityComparer = equalityComparer;
        }

        public bool IsSome { get; }
        public bool IsNone => !IsSome;
        public T Expect(string message)
        {
            if (IsSome) return _value;
            throw new RlxException(message);
        }

        public T Unwrap()
        {
            if (IsSome) return _value;
            throw new RlxException("Option does not contain value");
        }

        public T UnwrapOr(T def)
        {
            if (IsSome) return _value;
            return def;
        }

        public T UnwrapOrElse(Func<T> fn)
        {
            if (IsSome) return _value;
            return fn();
        }

        public T UnwrapOrDefault()
        {
            if (IsSome) return _value;
            return default(T);
        }

        public Option<TResult> Map<TResult>(Func<T, TResult> fn)
        {
            if (IsSome) return new Option<TResult>(fn(_value));
            return Option<TResult>.None;
        }

        public Option<TResult> Map<TResult>(Func<T, TResult> fn, IEqualityComparer<TResult> equalityComparer)
        {
            if (IsSome) return new Option<TResult>(fn(_value), equalityComparer);
            return Option<TResult>.None;
        }

        public OptionTask<TResult> Map<TResult>(Func<T, Task<TResult>> fn)
        {
            if (IsSome) return new OptionTask<TResult>(fn(_value));
            return new OptionTask<TResult>();
        }

        public TResult MapOr<TResult>(TResult def, Func<T, TResult> fn)
        {
            if (IsSome) return fn(_value);
            return def;
        }

        public TResult MapOrElse<TResult>(Func<TResult> def, Func<T, TResult> fn)
        {
            if (IsSome) return fn(_value);
            return def();
        }

        public Task<TResult> MapOrElse<TResult>(Func<Task<TResult>> def, Func<T, Task<TResult>> fn)
        {
            if (IsSome) return fn(_value);
            return def();
        }

        public Option<TResult> And<TResult>(Option<TResult> optionB)
        {
            if (IsSome) return optionB;
            return Option<TResult>.None;
        }

        public OptionTask<TResult> And<TResult>(OptionTask<TResult> optionB)
        {
            if (IsSome) return optionB;
            return OptionTask<TResult>.None;
        }

        public Option<TResult> AndThen<TResult>(Func<T, Option<TResult>> fn)
        {
            if (IsSome) return fn(_value);
            return Option<TResult>.None;
        }

        public OptionTask<TResult> AndThen<TResult>(Func<T, OptionTask<TResult>> fn)
        {
            if (IsSome) return fn(_value);
            return OptionTask<TResult>.None;
        }

        public Result<T, TError> OkOr<TError>(TError error)
        {
            if (IsSome) return Functions.Ok<T, TError>(_value);
            return Functions.Error<T, TError>(error);
        }

        public Result<T, TError> OkOrElse<TError>(Func<TError> error)
        {
            if (IsSome) return Functions.Ok<T, TError>(_value);
            return Functions.Error<T, TError>(error());
        }
        
        public Option<T> Or(Option<T> optionB)
        {
            if (IsSome) return this;
            return optionB;
        }

        public OptionTask<T> Or(OptionTask<T> optionB)
        {
            if (IsSome) return new OptionTask<T>(Task.FromResult(_value));
            return optionB;
        }

        public Option<T> OrElse(Func<Option<T>> fn)
        {
            if (IsSome) return this;
            return fn();
        }

        public OptionTask<T> OrElse(Func<OptionTask<T>> fn)
        {
            if (IsSome) return new OptionTask<T>(Task.FromResult(_value));
            return fn();
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (IsSome) yield return _value;
        }

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        public override bool Equals(object obj)
            => obj is Option<T> o && Equals(o);

        public bool Equals(Option<T> other)
        {
            if (IsSome & other.IsSome) return _equalityComparer.Equals(_value, other._value);
            return IsNone & other.IsNone;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + _equalityComparer.GetHashCode(_value);
                return hash;
            }
        }
    }
}
