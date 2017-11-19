using static Rlx.Functions;

namespace Rlx
{
    public static class GenericExtensions
    {
        public static Option<T> ToOption<T>(this T instance)
            => instance == null ? None<T>() : Some(instance);

        public static Option<T> ToOption<T>(this T? instance) where T : struct
            => instance.HasValue ? Some(instance.Value) : None<T>();
    }
}
