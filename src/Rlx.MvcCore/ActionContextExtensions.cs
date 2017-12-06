using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Rlx.MvcCore
{
    static class ActionContextExtensions
    {
        public static ILogger<T> GetLogger<T>(this ActionContext context) =>
            context.HttpContext.RequestServices.GetRequiredService<ILogger<T>>();
    }
}
