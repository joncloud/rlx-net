using System.Threading.Tasks;

namespace Rlx
{
    public static class ResultExtensions
    {
        public static T UnwrapEither<T>(this Result<T, T> result) =>
            result.UnwrapOrElse(error => error);

        public static Task<T> UnwrapEitherAsync<T>(this ResultTask<T, T> result) =>
            result.UnwrapOrElseAsync(error => error);
    }
}
