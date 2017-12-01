using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Rlx.MvcCore
{
    class ErrorWithContentResult<T> : ObjectResult
    {
        public ErrorWithContentResult(T error, int statusCode) : base(error) =>
            StatusCode = statusCode;

        public override void ExecuteResult(ActionContext context)
        {
            context.GetLogger<ErrorWithContentResult<T>>()
                .LogError("Encountered error {error}", Value);

            base.ExecuteResult(context);
        }
    }
}
