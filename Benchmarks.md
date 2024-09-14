# Benchmark Results

Benchmarks are run using BenchmarkDotNet. You can run these benchmarks yourself quite easily; just navigate to `tests/RapidCsv.Benchmarks` and run `dotnet -c Release` in a terminal.

```
// * Legends *
  Mean      : Arithmetic mean of all measurements
  Error     : Half of 99.9% confidence interval
  StdDev    : Standard deviation of all measurements
  Median    : Value separating the higher half of all measurements (50th percentile)
  Min       : Minimum
  Max       : Maximum
  Gen0      : GC Generation 0 collects per 1000 operations
  Gen1      : GC Generation 1 collects per 1000 operations
  Gen2      : GC Generation 2 collects per 1000 operations
  Allocated : Allocated memory per single operation (managed only, inclusive, 1KB = 1024B)
  1 us      : 1 Microsecond (0.000001 sec)

```

- AMD Ryzen 7 7840U
- .NET 8.0.824.36612
- RapidCSV [version 0.0.1](https://github.com/erik1066/rapid-csv/releases/tag/v0.0.1)

| Method                                      | Mean         | Error        | StdDev       | Median       | Min          | Max           | Gen0       | Gen1      | Gen2      | Allocated    |
|-------------------------------------------- |-------------:|-------------:|-------------:|-------------:|-------------:|--------------:|-----------:|----------:|----------:|-------------:|
| RFC4180_Validate_Failed_10Cols_by_100Rows   |     69.65 us |     1.386 us |     2.736 us |     69.79 us |     64.11 us |      75.01 us |     7.9346 |    0.6104 |         - |     65.07 KB |
| RFC4180_Validate_Failed_10Cols_by_1kRows    |    599.13 us |    11.808 us |    24.385 us |    599.13 us |    549.21 us |     644.50 us |    65.4297 |   24.4141 |         - |     536.9 KB |
| RFC4180_Validate_Failed_10Cols_by_10kRows   |  6,403.38 us |   136.472 us |   402.390 us |  6,192.36 us |  5,915.31 us |   7,444.69 us |   632.8125 |  453.1250 |         - |   5197.95 KB |
| RFC4180_Validate_Failed_10Cols_by_100kRows  | 85,303.55 us | 1,681.323 us | 2,001.496 us | 85,750.85 us | 82,260.69 us |  89,941.03 us |  6857.1429 | 2857.1429 | 1142.8571 |  53408.15 KB |
| RFC4180_Validate_Success_10Cols_by_100Rows  |     29.53 us |     0.367 us |     0.343 us |     29.56 us |     28.64 us |      30.08 us |     5.5847 |    0.1831 |         - |     45.85 KB |
| RFC4180_Validate_Success_10Cols_by_1kRows   |    192.12 us |     1.913 us |     1.790 us |    192.17 us |    189.52 us |     196.02 us |    43.4570 |    1.4648 |         - |    355.24 KB |
| RFC4180_Validate_Success_10Cols_by_10kRows  |  1,899.56 us |    28.611 us |    26.763 us |  1,904.07 us |  1,838.69 us |   1,933.93 us |   421.8750 |   13.6719 |         - |   3449.06 KB |
| RFC4180_Validate_Success_10Cols_by_100kRows | 18,611.56 us |   263.794 us |   246.753 us | 18,710.24 us | 18,036.03 us |  18,902.17 us |  4187.5000 |   31.2500 |         - |   34386.7 KB |
| RFC4180_Validate_Success_20Cols_by_100Rows  |     61.67 us |     0.472 us |     0.418 us |     61.69 us |     60.92 us |      62.42 us |     9.2773 |    0.3662 |         - |      76.6 KB |
| RFC4180_Validate_Success_20Cols_by_1kRows   |    505.83 us |     6.517 us |     5.777 us |    507.43 us |    492.36 us |     511.61 us |    78.1250 |    2.9297 |         - |    639.82 KB |
| RFC4180_Validate_Success_20Cols_by_10kRows  |  5,300.66 us |    78.941 us |    73.841 us |  5,279.84 us |  5,164.44 us |   5,441.62 us |   765.6250 |   23.4375 |         - |      6272 KB |
| RFC4180_Validate_Success_20Cols_by_100kRows | 51,013.63 us |   742.285 us |   694.334 us | 51,016.30 us | 50,104.02 us |  52,020.86 us |  7600.0000 |         - |         - |  62592.42 KB |
| RFC4180_Validate_Success_40Cols_by_100Rows  |    118.88 us |     2.188 us |     2.247 us |    119.96 us |    112.83 us |     121.03 us |    14.8926 |    0.6104 |         - |    122.46 KB |
| RFC4180_Validate_Success_40Cols_by_1kRows   |    979.49 us |    11.293 us |    10.563 us |    979.75 us |    961.53 us |     997.01 us |   126.9531 |    5.8594 |         - |   1051.32 KB |
| RFC4180_Validate_Success_40Cols_by_10kRows  | 10,109.05 us |    69.819 us |    65.309 us | 10,108.29 us |  9,973.82 us |  10,232.21 us |  1265.6250 |   46.8750 |         - |  10339.79 KB |
| RFC4180_Validate_Success_40Cols_by_100kRows | 99,714.61 us |   986.590 us |   922.857 us | 99,867.65 us | 98,288.90 us | 101,061.86 us | 12500.0000 |         - |         - | 103222.71 KB |
| Content_Validate_Success_10Cols_by_100Rows  |    130.99 us |     2.489 us |     2.556 us |    130.55 us |    126.56 us |     135.71 us |    25.3906 |    4.8828 |         - |    215.25 KB |
| Content_Validate_Success_10Cols_by_1kRows   |    753.33 us |    11.869 us |     9.911 us |    755.53 us |    726.59 us |     765.81 us |   177.7344 |   35.1563 |         - |   1452.78 KB |
```