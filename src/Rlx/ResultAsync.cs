using System;
using System.Threading.Tasks;

namespace Rlx
{
    public struct ResultAsync<TValue, TError>
    {
        public static readonly ResultAsync<TValue, TError> None = new ResultAsync<TValue, TError>();
        readonly Task<TValue> _value;
        readonly Task<TError> _error;
        public ResultAsync(Task<TValue> result)
        {
            IsOk = true;
            _value = result;
            _error = null;
        }

        public ResultAsync(Task<TError> error)
        {
            IsOk = false;
            _value = null;
            _error = error;
        }

        public bool IsOk { get; }
        public bool IsError => !IsOk;

        public OptionAsync<TValue> Ok()
        {
            if (IsOk) return Functions.Some(_value);
            return Functions.NoneAsync<TValue>();
        }

        public OptionAsync<TError> Error()
        {
            if (IsOk) return Functions.NoneAsync<TError>();
            return Functions.Some(_error);
        }

        public Task<Result<TValue, TError>> ToSync()
        {
            if (IsOk) return _value.Select(x => new Result<TValue, TError>(x));
            return _error.Select(x => new Result<TValue, TError>(x));
        }

        public ResultAsync<TResult, TError> Map<TResult>(Func<TValue, Task<TResult>> fn)
        {
            if (IsOk) return new ResultAsync<TResult, TError>(_value.Select(fn));
            return new ResultAsync<TResult, TError>(_error);
        }

        public ResultAsync<TValue, TResult> MapError<TResult>(Func<TError, Task<TResult>> fn)
        {
            if (IsOk) return new ResultAsync<TValue, TResult>(_value);
            return new ResultAsync<TValue, TResult>(_error.Select(fn));
        }

        public async Task<TValue> UnwrapAsync()
        {
            if (IsOk) return await _value;
            var error = await _error;
            throw new RlxException(error.ToString());
        }

        public async Task<TError> UnwrapErrorAsync()
        {
            if (IsOk)
            {
                var value = await _value;
                throw new RlxException(value.ToString());
            }
            return await _error;
        }

        public Task<TValue> UnwrapOrAsync(TValue optionB)
        {
            if (IsOk) return _value;
            return Task.FromResult(optionB);
        }

        public async Task<TValue> UnwrapOrElseAsync(Func<TError, TValue> fn)
        {
            if (IsOk) return await _value;
            var error = await _error;
            return fn(error);
        }

        public Task<TValue> UnwrapOrDefaultAsync()
        {
            if (IsOk) return _value;
            return Task.FromResult(default(TValue));
        }

        public async Task<TValue> ExpectAsync(string message)
        {
            if (IsOk) return await _value;
            var error = await _error;
            throw new RlxException($"{message}: {error}");
        }

        public async Task<TError> ExpectErrorAsync(string message)
        {
            if (IsOk)
            {
                var value = await _value;
                throw new RlxException($"{message}: {value}");
            }
            return await _error;
        }
    }
}
