using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Rlx.BenchmarkTests
{
    class Program
    {
        static string Benchmarkify(string typeName)
            => typeName.EndsWith("Benchmark") ? typeName : typeName + "Benchmark";

        static string Namespacify(string typeName)
            => typeName.Contains(".") ? typeName : "Rlx.BenchmarkTests." + typeName;

        static bool IsBenchmarkType(Type type)
            => type.Name.EndsWith("Benchmark");

        static IEnumerable<Type> GetBenchmarkTypes()
            => typeof(Program).Assembly.GetExportedTypes().Where(IsBenchmarkType);

        static int Main(string[] args)
            => args.ElementAtOrDefault(0)
                .ToOption()
                .Map(Namespacify)
                .Map(Benchmarkify)
                .Map(typeName => typeof(Program).Assembly.GetType(typeName))
                .AndThen(type => type.ToOption())
                .Map(type => BenchmarkRunner.Run(type))
                .MapOrElse(HandleError, _ => 0);

        static int HandleError()
        {
            string GetSimpleName(string typeName)
                => typeName.Substring(0, typeName.IndexOf("Benchmark"));

            var names = GetBenchmarkTypes()
                .Select(type => GetSimpleName(type.Name))
                .OrderBy(x => x)
                .ToList();

            Console.WriteLine("Enter one of the following benchmarks as arguments");
            foreach (var name in names)
            {
                Console.Write("\t* ");
                Console.WriteLine(name);
            }

            Console.WriteLine();
            Console.WriteLine("Sample Usage");

            string GetProgramName() =>
                new FileInfo(new Uri(typeof(Program).Assembly.GetName().CodeBase).LocalPath).Name;

            Console.WriteLine($"\tdotnet ./{GetProgramName()} {names[0]}");

            return 1;
        }
    }
}
