using System;
using System.Threading.Tasks;
using static Rlx.Functions;

namespace Rlx
{
    public struct ResultTask<TValue, TError>
    {
        readonly Task<Result<TValue, TError>> _task;
        public ResultTask(Task<Result<TValue, TError>> task)
            => _task = task;

        public ResultTask(Task<TValue> task)
            : this(task.Select(value => Ok<TValue, TError>(value))) { }

        public ResultTask(Task<TError> task)
            : this(task.Select(error => Error<TValue, TError>(error))) { }

        public OptionTask<TValue> Ok()
        {
            var task = _task.Select(x => x.Ok());
            return new OptionTask<TValue>(task);
        }

        public OptionTask<TError> Error()
        {
            var task = _task.Select(x => x.Error());
            return new OptionTask<TError>(task);
        }

        public Task<Result<TValue, TError>> ToSync()
            => _task;

        public ResultTask<TResult, TError> Map<TResult>(Func<TValue, TResult> fn)
        {
            var task = _task.Select(x => x.Map(fn));
            return new ResultTask<TResult, TError>(task);
        }

        public ResultTask<TResult, TError> Map<TResult>(Func<TValue, Task<TResult>> fn)
        {
            var task = _task.Select(async x =>
            {
                if (x.IsOk)
                {
                    var y = await fn(x.Unwrap());
                    return Ok<TResult, TError>(y);
                }
                return Error<TResult, TError>(x.UnwrapError());
            });
            return new ResultTask<TResult, TError>(task);
        }

        public ResultTask<TValue, TResult> MapError<TResult>(Func<TError, TResult> fn)
        {
            var task = _task.Select(x => x.MapError(fn));
            return new ResultTask<TValue, TResult>(task);
        }

        public ResultTask<TValue, TResult> MapError<TResult>(Func<TError, Task<TResult>> fn)
        {
            var task = _task.Select(async x =>
            {
                if (x.IsOk)
                {
                    return Ok<TValue, TResult>(x.Unwrap());
                }
                var y = await fn(x.UnwrapError());
                return Error<TValue, TResult>(y);
            });
            return new ResultTask<TValue, TResult>(task);
        }

        public ResultTask<TValue, TError> OrElse(Func<TError, Result<TValue, TError>> fn)
        {
            var task = _task.Select(x => x.OrElse(fn));
            return new ResultTask<TValue, TError>(task);
        }

        public ResultTask<TValue, TError> OrElse(Func<TError, ResultTask<TValue, TError>> fn)
        {
            var task = _task.Select(x => x.OrElse(fn).ToSync());
            return new ResultTask<TValue, TError>(task);
        }

        public ResultTask<TResult, TError> AndThen<TResult>(Func<TValue, Result<TResult, TError>> fn)
        {
            var task = _task.Select(x => x.AndThen(fn));
            return new ResultTask<TResult, TError>(task);
        }

        public ResultTask<TResult, TError> AndThen<TResult>(Func<TValue, ResultTask<TResult, TError>> fn)
        {
            var task = _task.Select(x => x.AndThen(fn).ToSync());
            return new ResultTask<TResult, TError>(task);
        }

        public Task<TValue> UnwrapAsync()
            => _task.Select(x => x.Unwrap());

        public Task<TError> UnwrapErrorAsync()
            => _task.Select(x => x.UnwrapError());

        public Task<TValue> UnwrapOrAsync(TValue optionB)
            => _task.Select(x => x.UnwrapOr(optionB));

        public Task<TValue> UnwrapOrElseAsync(Func<TError, TValue> fn)
            => _task.Select(x => x.UnwrapOrElse(fn));

        public Task<TValue> UnwrapOrDefaultAsync()
            => _task.Select(x => x.UnwrapOrDefault());

        public Task<TValue> ExpectAsync(string message)
            => _task.Select(x => x.Expect(message));

        public Task<TError> ExpectErrorAsync(string message)
            => _task.Select(x => x.ExpectError(message));
    }
}
