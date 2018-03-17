using System;
using static Rlx.Functions;

namespace Rlx
{
    public struct Attempt<TResult>
    {
        readonly Func<TResult> _fn;
        public Attempt(Func<TResult> fn) => _fn = fn;

        public static implicit operator Result<TResult, Exception>(Attempt<TResult> attempt) =>
            attempt.Catch<Exception>();

        public Result<TResult, TException> Catch<TException>() 
            where TException : Exception
        {
            try { return Ok<TResult, TException>(_fn()); }
            catch (TException ex) { return Error<TResult, TException>(ex); }
        }
    }
}
