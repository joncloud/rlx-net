using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Rlx.MvcCore
{
    class ErrorNoContentResult<T> : StatusCodeResult
    {
        readonly T _error;
        public ErrorNoContentResult(T error, int statusCode) : base(statusCode) =>
            _error = error;

        public override void ExecuteResult(ActionContext context)
        {
            context.GetLogger<ErrorNoContentResult<T>>()
                .LogError("Encountered error {error}", _error);

            base.ExecuteResult(context);
        }
    }
}
