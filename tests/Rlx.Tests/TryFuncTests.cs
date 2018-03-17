using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Xunit;
using static Rlx.Functions;

namespace Rlx.Tests
{
    public class TryFuncTests
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
            Guid id = Guid.NewGuid();
            var parameters = CreateParameters(count).ToList();
            var body = Expression.Block(CreateMethodBody());
            var fnType = GetFuncType(count);
            var fn = Expression.Lambda(fnType, body, parameters).Compile();

            Result<Guid, Exception> result = InvokeFuncTest(fn, count);

            Assert.Equal(Ok<Guid, Exception>(id), result);

            IEnumerable<Expression> CreateMethodBody()
            {
                for (int i = 0; i < count; i++)
                {
                    var call = Expression.Call(
                        typeof(TryActionTests).GetMethod(nameof(AssertEqualInt), BindingFlags.Static | BindingFlags.NonPublic),
                        Expression.Constant(i, typeof(int)),
                        parameters[i]
                    );
                    yield return call;
                }
                yield return Expression.Constant(id, typeof(Guid));
            }
        }

        [MemberData(nameof(AllFunctionArgumentCounts))]
        [Theory]
        public void ShouldReturnSomeGivenExceptionThrown(int count)
        {
            var exception = new Exception();
            var body = Expression.Block(
                Expression.Throw(Expression.Constant(exception), typeof(Exception)),
                Expression.Constant(Guid.NewGuid(), typeof(Guid))
            );
            var parameters = CreateParameters(count);
            var fnType = GetFuncType(count);
            var fn = Expression.Lambda(fnType, body, parameters).Compile();

            Result<Guid, Exception> result = InvokeFuncTest(fn, count);

            Assert.Equal(Error<Guid, Exception>(exception), result);
        }

        static Type GetFuncType(int count)
        {
            if (count == 0) return typeof(Func<Guid>);
            var typeArguments = Enumerable.Repeat(typeof(int), count)
                .Append(typeof(Guid))
                .ToArray();
            return TypeHelper.GetType($"System.Func`{count + 1}")
                .MakeGenericType(typeArguments);
        }

        static Attempt<Guid> InvokeFuncTest(Delegate fn, int count)
        {
            var fnType = fn.GetType();

            var tryMethod = GetTryMethod(
                fnType.GetGenericTypeDefinition(),
                count
            );

            var parameters = GetTryParameters(fn, count).ToArray();
            var invocationResult = tryMethod.Invoke(null, parameters);
            return Assert.IsType<Attempt<Guid>>(invocationResult);
        }

        static MethodInfo GetTryMethod(Type fnType, int count)
        {
            var methods = typeof(TryFunctions).GetMethods();
            foreach (var method in methods)
            {
                var parameters = method.GetParameters();
                if (parameters.Length == count + 1 && parameters[0].ParameterType.Name == fnType.Name)
                {
                    var typeArguments = Enumerable.Repeat(typeof(int), count)
                        .Append(typeof(Guid))
                        .ToArray();
                    return method.MakeGenericMethod(typeArguments);
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
