using Microsoft.AspNetCore.Http;
using Rlx;
using Rlx.MvcCore;
using System;
using System.Threading.Tasks;
using static Rlx.Functions;

namespace Microsoft.AspNetCore.Mvc
{
    public static class ResultExtensions
    {
        public static IActionResult ToActionResult<TValue, TError>(this Result<TValue, TError> result)
            => result.ToActionResult(_ => StatusCodes.Status200OK, _ => StatusCodes.Status500InternalServerError);

        public static IActionResult ToActionResult<TValue, TError>(this Result<TValue, TError> result, Func<TValue, int> ok, Func<TError, int> err)
            => result.ToActionResult(ok, err, _ => None<TError>()); // Default to none to prevent errors from potentially leaking out

        public static IActionResult ToActionResult<TValue, TError>(this Result<TValue, TError> result, Func<TValue, int> ok, Func<TError, int> err, Func<TError, Option<TError>> opt)
            => result.Map<IActionResult>(value => new ObjectResult(value) { StatusCode = ok(value) })
                .UnwrapOrElse(error => ToErrorActionResult(error, err, opt));

        public static Task<IActionResult> ToActionResult<TValue, TError>(this ResultTask<TValue, TError> result)
            => result.ToActionResult(_ => StatusCodes.Status200OK, _ => StatusCodes.Status500InternalServerError);

        public static Task<IActionResult> ToActionResult<TValue, TError>(this ResultTask<TValue, TError> result, Func<TValue, int> ok, Func<TError, int> err)
            => result.ToActionResult(ok, err, _ => None<TError>()); // Default to none to prevent errors from potentially leaking out

        public static Task<IActionResult> ToActionResult<TValue, TError>(this ResultTask<TValue, TError> result, Func<TValue, int> ok, Func<TError, int> err, Func<TError, Option<TError>> opt)
            => result.Map<IActionResult>(value => new ObjectResult(value) { StatusCode = ok(value) })
                .UnwrapOrElseAsync(error => ToErrorActionResult(error, err, opt));

        static IActionResult ToErrorActionResult<T>(T error, Func<T, int> code, Func<T, Option<T>> opt)
        {
            int statusCode = code(error);
            return opt(error).Map(_ => ErrorWithContent(error, statusCode)).UnwrapOrElse(() => ErrorNoContent(error, statusCode));
        }

        static IActionResult ErrorNoContent<T>(T error, int statusCode)
            => new ErrorNoContentResult<T>(error, statusCode);

        static IActionResult ErrorWithContent<T>(T error, int statusCode)
            => new ErrorWithContentResult<T>(error, statusCode);
    }
}
