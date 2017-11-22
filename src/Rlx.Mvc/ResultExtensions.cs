using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Rlx.Mvc
{
    public static class ResultExtensions
    {
        public static IActionResult ToActionResult<TValue, TError>(this Result<TValue, TError> result)
            => result.ToActionResult(_ => StatusCodes.Status200OK, _ => StatusCodes.Status400BadRequest);

        public static IActionResult ToActionResult<TValue, TError>(this Result<TValue, TError> result, Func<TValue, int> ok, Func<TError, int> err)
            => result.Map<IActionResult>(value => new ObjectResult(value) { StatusCode = ok(value) })
                .UnwrapOrElse(error => new ObjectResult(error) { StatusCode = err(error) });

        public static Task<IActionResult> ToActionResult<TValue, TError>(this ResultTask<TValue, TError> result)
            => result.ToActionResult(_ => StatusCodes.Status200OK, _ => StatusCodes.Status400BadRequest);

        public static Task<IActionResult> ToActionResult<TValue, TError>(this ResultTask<TValue, TError> result, Func<TValue, int> ok, Func<TError, int> err)
            => result.Map<IActionResult>(value => new ObjectResult(value) { StatusCode = ok(value) })
                .UnwrapOrElseAsync(error => new ObjectResult(error) { StatusCode = err(error) });
    }
}
