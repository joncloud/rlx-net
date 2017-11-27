using System;
using static Rlx.Functions;

namespace Rlx
{
    public delegate Option<T> ParseOption<T>(string s);
    public delegate bool TryParse<T>(string s, out T t);

    public static class Parser
    {
        public static Option<T> Parse<T>(string s, TryParse<T> d)
            => d(s, out var t) ? Some(t) : None<T>();
        
        public static ParseOption<T> Compile<T>(TryParse<T> d)
            => s => Parse<T>(s, d);
    }

    public static class Parse
    {
        public static ParseOption<bool> Boolean = Parser.Compile<bool>(bool.TryParse);
        public static ParseOption<byte> Byte = Parser.Compile<byte>(byte.TryParse);
        public static ParseOption<DateTime> DateTime = Parser.Compile<DateTime>(System.DateTime.TryParse);
        public static ParseOption<double> Double = Parser.Compile<double>(double.TryParse);
        public static ParseOption<Guid> Guid = Parser.Compile<Guid>(System.Guid.TryParse);
        public static ParseOption<short> Int16 = Parser.Compile<short>(short.TryParse);
        public static ParseOption<int> Int32 = Parser.Compile<int>(int.TryParse);
        public static ParseOption<long> Int64 = Parser.Compile<long>(long.TryParse);
        public static ParseOption<float> Single = Parser.Compile<float>(float.TryParse);
    }
}