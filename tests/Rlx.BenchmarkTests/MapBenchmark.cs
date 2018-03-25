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
        {
            return Some(123).Map(ToSquared).Map(ToPlusOne).UnwrapOrDefault();
            int ToSquared(int x) => x * x;
            int ToPlusOne(int x) => x + 1;
        }
        
        [Benchmark]
        public int MapNone()
        {
            return None<int>().Map(ToSquared).Map(ToPlusOne).UnwrapOrDefault();
            int ToSquared(int x) => x * x;
            int ToPlusOne(int x) => x + 1;
        }

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
