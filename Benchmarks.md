# Benchmark Results

Benchmarks are run using BenchmarkDotNet. You can run these benchmarks yourself quite easily; just navigate to `tests/RapidCsv.Benchmarks` and run `dotnet -c Release` in a terminal.

| Method                              | Mean          | Error         | StdDev        | Median        | Min           | Max           | Gen0       | Gen1      | Gen2      | Allocated    |
|------------------------------------ |--------------:|--------------:|--------------:|--------------:|--------------:|--------------:|-----------:|----------:|----------:|-------------:|
| Validate_Failed_10Cols_by_100Rows   |      87.61 us |      2.266 us |      6.538 us |      88.00 us |      77.34 us |     107.83 us |     7.9346 |    0.6104 |         - |        65 KB |
| Validate_Failed_10Cols_by_1kRows    |     918.53 us |     45.356 us |    130.136 us |     864.36 us |     710.28 us |   1,269.32 us |    64.4531 |   23.4375 |         - |    536.83 KB |
| Validate_Failed_10Cols_by_10kRows   |   9,371.06 us |    541.558 us |  1,553.833 us |   9,222.48 us |   7,360.58 us |  14,662.51 us |   632.8125 |  453.1250 |         - |   5197.88 KB |
| Validate_Failed_10Cols_by_100kRows  | 137,255.74 us | 10,101.840 us | 28,657.241 us | 132,103.95 us |  98,605.46 us | 213,613.10 us |  6800.0000 | 2800.0000 | 1000.0000 |  53408.86 KB |
| Validate_Success_10Cols_by_100Rows  |      51.92 us |      3.533 us |      9.964 us |      49.83 us |      38.82 us |      81.14 us |     5.5542 |    0.1831 |         - |     45.78 KB |
| Validate_Success_10Cols_by_1kRows   |     454.80 us |     24.300 us |     67.739 us |     450.06 us |     348.11 us |     687.20 us |    42.9688 |    0.9766 |         - |    355.17 KB |
| Validate_Success_10Cols_by_10kRows  |   4,224.42 us |    227.174 us |    659.073 us |   4,162.00 us |   3,109.66 us |   5,769.80 us |   421.8750 |   11.7188 |         - |      3449 KB |
| Validate_Success_10Cols_by_100kRows |  32,516.19 us |    270.185 us |    225.617 us |  32,519.53 us |  32,156.01 us |  32,936.94 us |  4153.8462 |         - |         - |  34386.67 KB |
| Validate_Success_20Cols_by_100Rows  |     134.93 us |      8.406 us |     24.520 us |     134.45 us |     100.74 us |     209.09 us |     9.2773 |    0.3662 |         - |     76.53 KB |
| Validate_Success_20Cols_by_1kRows   |   1,150.73 us |     66.373 us |    187.208 us |   1,068.95 us |     901.67 us |   1,759.82 us |    78.1250 |    1.9531 |         - |    639.75 KB |
| Validate_Success_20Cols_by_10kRows  |  12,448.11 us |    635.278 us |  1,863.160 us |  11,975.67 us |   9,421.97 us |  17,774.58 us |   765.6250 |   15.6250 |         - |   6271.93 KB |
| Validate_Success_20Cols_by_100kRows |  97,025.14 us |  1,817.465 us |  1,700.058 us |  96,638.03 us |  94,944.54 us | 100,262.43 us |  7600.0000 |         - |         - |  62592.42 KB |
| Validate_Success_40Cols_by_100Rows  |     255.74 us |     15.963 us |     47.066 us |     238.39 us |     204.69 us |     389.98 us |    14.8926 |    0.4883 |         - |    122.39 KB |
| Validate_Success_40Cols_by_1kRows   |   2,186.90 us |     43.609 us |     87.092 us |   2,195.55 us |   2,004.72 us |   2,361.03 us |   125.0000 |    3.9063 |         - |   1051.25 KB |
| Validate_Success_40Cols_by_10kRows  |  22,161.61 us |  1,374.638 us |  3,763.050 us |  21,557.15 us |  18,080.27 us |  35,861.67 us |  1250.0000 |   31.2500 |         - |  10339.73 KB |
| Validate_Success_40Cols_by_100kRows | 235,141.48 us | 11,723.216 us | 33,636.107 us | 237,224.67 us | 180,311.23 us | 321,101.68 us | 12000.0000 |         - |         - | 103223.23 KB |


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