using Microsoft.AspNetCore.Mvc.ModelBinding;
using static Rlx.Functions;

namespace Rlx.MvcCore
{
    public static class ModelStateDictionaryExtensions
    {
        public static Option<ModelStateDictionary> ToOption(this ModelStateDictionary modelState) =>
            modelState.Count > 0
                ? Some(modelState)
                : None<ModelStateDictionary>();

        public static Result<Unit, ModelStateDictionary> ToResult(this ModelStateDictionary modelState) =>
            modelState.ToOption().ErrorOr(Unit.Value);
    }
}
