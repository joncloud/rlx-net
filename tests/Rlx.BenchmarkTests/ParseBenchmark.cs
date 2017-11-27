using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.Linq;
using static Rlx.Functions;

namespace Rlx.BenchmarkTests
{
    [MemoryDiagnoser]
    public class ParseBenchmark
    {
        string[] _invalid = Enumerable.Repeat("a", 100).ToArray();
        string[] _valid = Enumerable.Repeat("1", 100).ToArray();

        [Benchmark]
        public int ParseSome()
        {
            int total = 0;
            foreach (string item in _valid)
            {
                total += Parse.Int32(item).UnwrapOrDefault();
            }
            return total;
        }

        [Benchmark]
        public int ParseNone()
        {
            int total = 0;
            foreach (string item in _invalid)
            {
                total += Parse.Int32(item).UnwrapOrDefault();
            }
            return total;
        }

        [Benchmark(Baseline = true)]
        public int TryParseInvalid()
        {
            int total = 0;
            foreach (string item in _invalid)
            {
                if (int.TryParse(item, out var num))
                {
                    total += num;
                }
            }
            return total;
        }

        [Benchmark]
        public int TryParseValid()
        {
            int total = 0;
            foreach (string item in _valid)
            {
                if (int.TryParse(item, out var num))
                {
                    total += num;
                }
            }
            return total;
        }
    }
}
