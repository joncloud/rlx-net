﻿using System;
using System.Threading.Tasks;
using static Rlx.Functions;

namespace Rlx
{
    public struct OptionTask<T>
    {
        public static readonly OptionTask<T> None = new OptionTask<T>(Task.FromResult(Option<T>.None));
        readonly Task<Option<T>> _task;
        
        public OptionTask(Task<T> task)
            => _task = task.Select(x => Some(x));

        public OptionTask(Task<Option<T>> task)
            => _task = task;

        public Task<Option<T>> ToSync()
            => _task;

        public Task<T> ExpectAsync(string message)
            => _task.Select(x => x.Expect(message));

        public Task<T> UnwrapAsync()
            => _task.Select(x => x.Unwrap());

        public Task<T> UnwrapOrAsync(T def)
            => _task.Select(x => x.UnwrapOr(def));

        public Task<T> UnwrapOrElseAsync(Func<T> fn)
            => _task.Select(x => x.UnwrapOrElse(fn));

        public Task<T> UnwrapOrDefaultAsync()
            => _task.Select(x => x.UnwrapOrDefault());

        public OptionTask<TResult> Map<TResult>(Func<T, TResult> fn)
            => new OptionTask<TResult>(_task.Select(x => x.Map(fn)));

        public OptionTask<TResult> Map<TResult>(Func<T, Task<TResult>> fn)
        {
            var task = _task.Select(async x =>
            {
                if (x.IsSome)
                {
                    var y = await fn(x.Unwrap());
                    return Some(y);
                }
                return None<TResult>();
            });
            return new OptionTask<TResult>(task);
        }

        public Task<TResult> MapOr<TResult>(TResult def, Func<T, TResult> fn)
            => _task.Select(x => x.MapOr(def, fn));

        public Task<TResult> MapOrElse<TResult>(Func<TResult> def, Func<T, TResult> fn)
            => _task.Select(x => x.MapOrElse(def, fn));

        public Task<TResult> MapOrElse<TResult>(Func<Task<TResult>> def, Func<T, Task<TResult>> fn)
            => _task.Select(x => x.MapOrElse(def, fn));

        public OptionTask<TResult> And<TResult>(OptionTask<TResult> optionB)
        {
            var task = _task.Select(async x => x.And(await optionB.ToSync()));
            return new OptionTask<TResult>(task);
        }

        public OptionTask<TResult> AndThen<TResult>(Func<T, Option<TResult>> fn)
        {
            var task = _task.Select(x => x.AndThen(fn));
            return new OptionTask<TResult>(task);
        }

        public OptionTask<TResult> AndThen<TResult>(Func<T, OptionTask<TResult>> fn)
        {
            var task = _task.Select(x => x.AndThen(fn).ToSync());
            return new OptionTask<TResult>(task);
        }

        public OptionTask<T> Or(OptionTask<T> optionB)
        {
            var task = _task.Select(async x => x.Or(await optionB.ToSync()));
            return new OptionTask<T>(task);
        }

        public OptionTask<T> OrElse(Func<Option<T>> fn)
        {
            var task = _task.Select(x => x.OrElse(fn));
            return new OptionTask<T>(task);
        }

        public OptionTask<T> OrElse(Func<OptionTask<T>> fn)
        {
            var task = _task.Select(x => x.OrElse(fn).ToSync());
            return new OptionTask<T>(task);
        }

        public ResultTask<T, TError> OkOr<TError>(TError error)
        {
            var task = _task.Select(x => x.OkOr(error));
            return new ResultTask<T, TError>(task);
        }

        public ResultTask<T, TError> OkOrElse<TError>(Func<TError> error)
        {
            var task = _task.Select(x => x.OkOrElse(error));
            return new ResultTask<T, TError>(task);
        }

        public ResultTask<T, TError> OkOrElse<TError>(Func<Task<TError>> error)
        {
            var task = _task.Select(x => x.OkOrElse(error).ToSync());
            return new ResultTask<T, TError>(task);
        }

        public OptionTask<Unit> Consume(Action<T> fn) =>
            Map(value => { fn(value); return Unit.Value; });

        public OptionTask<Unit> Consume(Func<T, Task> fn) =>
            Map(value => fn(value).Select(() => Unit.Value));
    }
}
