using System;

namespace Rlx
{
    public static partial class Functions
    {
        public static Attempt<Unit> Try(Action fn) =>
            new Attempt<Unit>(() => { fn(); return Unit.Value; });

        public static Attempt<Unit> Try<T>(Action<T> fn, T arg) =>
            Try(() => fn(arg));

        public static Attempt<Unit> Try<T1, T2>(Action<T1, T2> fn, T1 arg1, T2 arg2) =>
            Try(() => fn(arg1, arg2));

        public static Attempt<Unit> Try<T1, T2, T3>(Action<T1, T2, T3> fn, T1 arg1, T2 arg2, T3 arg3) =>
            Try(() => fn(arg1, arg2, arg3));

        public static Attempt<Unit> Try<T1, T2, T3, T4>(Action<T1, T2, T3, T4> fn, T1 arg1, T2 arg2, T3 arg3, T4 arg4) =>
            Try(() => fn(arg1, arg2, arg3, arg4));

        public static Attempt<Unit> Try<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> fn, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) =>
           Try(() => fn(arg1, arg2, arg3, arg4, arg5));

        public static Attempt<Unit> Try<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> fn, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) =>
            Try(() => fn(arg1, arg2, arg3, arg4, arg5, arg6));

        public static Attempt<Unit> Try<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> fn, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) =>
            Try(() => fn(arg1, arg2, arg3, arg4, arg5, arg6, arg7));

        public static Attempt<Unit> Try<T1, T2, T3, T4, T5, T6, T7, T8>(Action<T1, T2, T3, T4, T5, T6, T7, T8> fn, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8) =>
            Try(() => fn(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8));

        public static Attempt<Unit> Try<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> fn, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9) =>
            Try(() => fn(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9));

        public static Attempt<Unit> Try<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> fn, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10) =>
            Try(() => fn(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10));

        public static Attempt<Unit> Try<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> fn, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11) =>
            Try(() => fn(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11));

        public static Attempt<Unit> Try<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> fn, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12) =>
            Try(() => fn(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12));

        public static Attempt<Unit> Try<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> fn, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13) =>
            Try(() => fn(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13));

        public static Attempt<Unit> Try<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> fn, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14) =>
            Try(() => fn(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14));

        public static Attempt<Unit> Try<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> fn, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15) =>
            Try(() => fn(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15));

        public static Attempt<Unit> Try<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> fn, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16) =>
            Try(() => fn(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16));
    }
}
