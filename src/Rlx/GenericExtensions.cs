using System.Threading.Tasks;
using static Rlx.Functions;

namespace Rlx
{
    public static class GenericExtensions
    {
        public static Option<T> ToOption<T>(this T instance)
            => instance == null ? None<T>() : Some(instance);

        public static Option<T> ToOption<T>(this T? instance) where T : struct
            => instance.HasValue ? Some(instance.Value) : None<T>();

        public static OptionTask<T> ToOption<T>(this Task<T> task)
            => new OptionTask<T>(task.Select(instance => instance.ToOption()));

        public static OptionTask<T> ToOption<T>(this Task<T?> task) where T : struct
            => new OptionTask<T>(task.Select(instance => instance.ToOption()));
    }
}
