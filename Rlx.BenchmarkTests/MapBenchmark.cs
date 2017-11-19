using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using static Rlx.Functions;

namespace Rlx.BenchmarkTests
{
    [MemoryDiagnoser]
    public class MapBenchmark
    {
        [Benchmark]
        public int MapSome()
            => Some(123).Map(x => x * x).Map(x => x + 1).UnwrapOrDefault();
        
        [Benchmark]
        public int MapNone()
            => None<int>().Map(x => x * x).Map(x => x + 1).UnwrapOrDefault();

        [Benchmark]
        public int MapNullInt()
        {
            int? Calculate() => null;
            int? num = Calculate();
            if (num.HasValue) num = num * num;
            if (num.HasValue) num = num + 1;
            return num ?? 0;
        }

        [Benchmark(Baseline = true)]
        public int MapInt()
        {
            int? Calculate() => 123;
            int? num = Calculate();
            if (num.HasValue) num = num * num;
            if (num.HasValue) num = num + 1;
            return num ?? 0;
        }
    }
}
