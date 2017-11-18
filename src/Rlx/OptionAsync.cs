using System;
using System.Threading.Tasks;

namespace Rlx
{
    public struct OptionAsync<T>
    {
        public static readonly OptionAsync<T> None = new OptionAsync<T>();
        readonly Task<T> _task;
        
        public OptionAsync(Task<T> task)
        {
            IsSome = true;
            _task = task;
        }

        public bool IsSome { get; }
        public bool IsNone => !IsSome;

        public Task<Option<T>> ToSync()
        {
            if (IsSome) return _task.Select(x => new Option<T>(x));
            return Task.FromResult(Option<T>.None);
        }

        public Task<T> ExpectAsync(string message)
        {
            if (IsSome) return _task;
            throw new RlxException(message);
        }

        public Task<T> UnwrapAsync()
        {
            if (IsSome) return _task;
            throw new RlxException("AsyncOption does not contain value");
        }

        public Task<T> UnwrapOrAsync(T def)
        {
            if (IsSome) return _task;
            return Task.FromResult(def);
        }

        public Task<T> UnwrapOrDefaultAsync()
        {
            if (IsSome) return _task;
            return Task.FromResult(default(T));
        }

        public OptionAsync<TResult> Map<TResult>(Func<T, TResult> fn)
        {
            if (IsSome) return new OptionAsync<TResult>(_task.Select(fn));
            return new OptionAsync<TResult>();
        }

        public OptionAsync<TResult> Map<TResult>(Func<T, Task<TResult>> fn)
        {
            if (IsSome) return new OptionAsync<TResult>(_task.Select(fn));
            return new OptionAsync<TResult>();
        }

        public OptionAsync<TResult> MapOrElse<TResult>(Func<Task<TResult>> def, Func<T, Task<TResult>> fn)
        {
            if (IsSome) return new OptionAsync<TResult>(_task.Select(fn));
            return new OptionAsync<TResult>(def());
        }

        public OptionAsync<TResult> And<TResult>(OptionAsync<TResult> optionB)
        {
            if (IsSome) return optionB;
            return OptionAsync<TResult>.None;
        }

        public OptionAsync<T> Or(OptionAsync<T> optionB)
        {
            if (IsSome) return this;
            return optionB;
        }

        public OptionAsync<T> OrElse(Func<OptionAsync<T>> fn)
        {
            if (IsSome) return this;
            return fn();
        }
    }
}
