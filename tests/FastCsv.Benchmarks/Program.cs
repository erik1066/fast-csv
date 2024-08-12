using BenchmarkDotNet.Running;

namespace FastCsv.Benchmarks;

public static class Program
{
    public static void Main(string[] args)
    {
        var summary1 = BenchmarkRunner.Run<ValidatorBenchmarks>();
    }
}

