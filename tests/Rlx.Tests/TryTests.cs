using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Xunit;
using static Rlx.Functions;

namespace Rlx.Tests
{
    public class TryTests
    {
        public static IEnumerable<object[]> AllFunctionArgumentCounts
        {
            get
            {
                int count = 17;
                while (--count > -1)
                    yield return new object[] { count };
            }
        }

        internal static void AssertEqualInt(int expected, int actual) =>
            Assert.Equal(expected, actual);

        [MemberData(nameof(AllFunctionArgumentCounts))]
        [Theory]
        public void ShouldReturnNoneGivenNoExceptionThrown(int count)
        {
            var parameters = CreateParameters(count).ToList();
            var body = Expression.Block(CreateAssertions());
            var fnType = GetActionType(count);
            var fn = Expression.Lambda(fnType, body, parameters).Compile();

            var option = InvokeActionTest(fn, count);

            Assert.Equal(None<Exception>(), option);

            IEnumerable<MethodCallExpression> CreateAssertions()
            {
                for (int i = 0; i < count; i++)
                {
                    var call = Expression.Call(
                        typeof(TryTests).GetMethod(nameof(AssertEqualInt), BindingFlags.Static | BindingFlags.NonPublic),
                        Expression.Constant(i, typeof(int)),
                        parameters[i]
                    );
                    yield return call;
                }
            }
        }

        [MemberData(nameof(AllFunctionArgumentCounts))]
        [Theory]
        public void ShouldReturnSomeGivenExceptionThrown(int count)
        {
            var exception = new Exception();
            var body = Expression.Throw(Expression.Constant(exception), typeof(Exception));
            var parameters = CreateParameters(count);
            var fnType = GetActionType(count);
            var fn = Expression.Lambda(fnType, body, parameters).Compile();

            var option = InvokeActionTest(fn, count);

            Assert.Equal(Some(exception), option);
        }

        static Type GetActionType(int count)
        {
            if (count == 0) return typeof(Action);
            var typeArguments = Enumerable.Repeat(typeof(int), count).ToArray();
            return TypeHelper.GetType($"System.Action`{count}")
                .MakeGenericType(typeArguments);
        }

        static Option<Exception> InvokeActionTest(Delegate fn, int count)
        {
            var fnType = fn.GetType();

            var tryMethod = GetTryMethod(count);

            var parameters = GetTryParameters(fn, count).ToArray();
            var invocationResult = tryMethod.Invoke(null, parameters);
            return Assert.IsType<Option<Exception>>(invocationResult);
        }

        static MethodInfo GetTryMethod(int count)
        {
            var methods = typeof(TryFunctions).GetMethods();
            foreach (var method in methods)
            {
                var parameters = method.GetParameters();
                if (parameters.Length == count + 1)
                {
                    if (method.IsGenericMethod)
                    {
                        var typeArguments = Enumerable.Repeat(typeof(int), count).ToArray();
                        return method.MakeGenericMethod(typeArguments);
                    }
                    return method;
                }
            }
            return null;
        }

        static IEnumerable<object> GetTryParameters(Delegate fn, int count)
        {
            yield return fn;
            for (int i = 0; i < count; i++)
            {
                yield return i;
            }
        }

        static IEnumerable<Type> GetTryArgumentTypes(Type fnType, int count)
        {
            yield return fnType;
            for (int i = 0; i < count; i++)
            {
                yield return typeof(int);
            }
        }

        static IEnumerable<ParameterExpression> CreateParameters(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var expression = Expression.Parameter(typeof(int), $"p{i}");
                yield return expression;
            }
        }
    }
}
