using Microsoft.AspNetCore.Mvc.ModelBinding;
using Xunit;
using static Rlx.Functions;

namespace Rlx.MvcCore.Tests
{
    public class ModelStateDictionaryExtensionsTests
    {
        [Fact]
        public void ToOption_ShouldReturnNoneGivenZeroErrorsCount()
        {
            var modelState = new ModelStateDictionary();
            var actual = modelState.ToOption();

            var expected = None<ModelStateDictionary>();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToOption_ShouldReturnSomeGivenPositiveErrorsCount()
        {
            var modelState = new ModelStateDictionary();
            modelState.AddModelError("", "Some Error");
            var actual = modelState.ToOption();

            var expected = Some(modelState);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToResult_ShouldReturnUnitGivenZeroErrorsCount()
        {
            var modelState = new ModelStateDictionary();
            var actual = modelState.ToResult();

            var expected = Ok<Unit, ModelStateDictionary>(Unit.Value);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToResult_ShouldReturnErrorGivenPositiveErrorsCount()
        {
            var modelState = new ModelStateDictionary();
            modelState.AddModelError("", "Some Error");
            var actual = modelState.ToResult();

            var expected = Error<Unit, ModelStateDictionary>(modelState);
            Assert.Equal(expected, actual);
        }
    }
}
