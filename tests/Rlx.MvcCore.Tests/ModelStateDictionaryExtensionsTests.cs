using Microsoft.AspNetCore.Mvc.ModelBinding;
using Xunit;
using static Rlx.Functions;

namespace Rlx.MvcCore.Tests
{
    public class ModelStateDictionaryExtensionsTests
    {
        static ModelStateDictionary Empty() => new ModelStateDictionary();
        static ModelStateDictionary PositiveCount()
        {
            var modelState = new ModelStateDictionary();
            modelState.SetModelValue("abc", "value", "value");
            return modelState;
        }
        static ModelStateDictionary PositiveErrorCount()
        {
            var modelState = new ModelStateDictionary();
            modelState.AddModelError("", "Some Error");
            return modelState;
        }

        [Fact]
        public void ToOption_ShouldReturnNoneGivenZeroErrorsCount()
        {
            var modelState = Empty();
            var actual = modelState.ToOption();

            var expected = None<ModelStateDictionary>();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToOption_ShouldReturnSomeGivenPositiveErrorsCount()
        {
            var modelState = PositiveErrorCount();
            var actual = modelState.ToOption();

            var expected = Some(modelState);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToOption_ShouldReturnNoneGivenPositiveCount()
        {
            var modelState = PositiveCount();
            var actual = modelState.ToOption();

            var expected = None<ModelStateDictionary>();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToResult_ShouldReturnUnitGivenZeroErrorsCount()
        {
            var modelState = Empty();
            var actual = modelState.ToResult();

            var expected = Ok<Unit, ModelStateDictionary>(Unit.Value);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToResult_ShouldReturnErrorGivenPositiveErrorsCount()
        {
            var modelState = PositiveErrorCount();
            var actual = modelState.ToResult();

            var expected = Error<Unit, ModelStateDictionary>(modelState);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToResult_ShouldReturnNoneGivenPositiveCount()
        {
            var modelState = PositiveCount();
            var actual = modelState.ToResult();

            var expected = Ok<Unit, ModelStateDictionary>(Unit.Value);
            Assert.Equal(expected, actual);
        }
    }
}
