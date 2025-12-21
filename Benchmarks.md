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

## .NET 10 Benchmark Results

BenchmarkDotNet v0.14.0, Fedora Linux 43 (Server Edition)
AMD Ryzen 7 7840U w/ Radeon 780M Graphics, 4 CPU, 4 logical and 4 physical cores
.NET SDK 10.0.101
[Host]     : .NET 10.0.1 (10.0.125.57005), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
DefaultJob : .NET 10.0.1 (10.0.125.57005), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

| Method                                      | Mean          | Error        | StdDev       | Median        | Min           | Max           | Gen0       | Gen1      | Gen2      | Allocated    |
|-------------------------------------------- |--------------:|-------------:|-------------:|--------------:|--------------:|--------------:|-----------:|----------:|----------:|-------------:|
| RFC4180_Validate_Failed_10Cols_by_100Rows   |      68.01 us |     1.355 us |     1.987 us |      67.59 us |      63.94 us |      71.71 us |     7.9346 |    0.6104 |         - |     64.93 KB |
| RFC4180_Validate_Failed_10Cols_by_1kRows    |     588.79 us |    11.657 us |    19.476 us |     588.48 us |     558.58 us |     640.43 us |    65.4297 |   24.4141 |         - |    536.76 KB |
| RFC4180_Validate_Failed_10Cols_by_10kRows   |   6,351.28 us |   124.199 us |   152.528 us |   6,309.80 us |   6,138.85 us |   6,644.49 us |   632.8125 |  445.3125 |         - |   5197.82 KB |
| RFC4180_Validate_Failed_10Cols_by_100kRows  |  80,935.80 us | 1,400.149 us | 1,169.188 us |  81,108.78 us |  78,908.84 us |  82,591.33 us |  6857.1429 | 2857.1429 | 1142.8571 |  53406.62 KB |
| RFC4180_Validate_Success_10Cols_by_100Rows  |      30.32 us |     0.602 us |     0.956 us |      30.24 us |      28.56 us |      32.08 us |     5.5847 |    0.1831 |         - |     45.78 KB |
| RFC4180_Validate_Success_10Cols_by_1kRows   |     207.69 us |     4.131 us |     8.622 us |     203.55 us |     197.27 us |     224.96 us |    43.4570 |    1.4648 |         - |    355.17 KB |
| RFC4180_Validate_Success_10Cols_by_10kRows  |   1,965.12 us |    20.993 us |    18.609 us |   1,967.34 us |   1,923.03 us |   1,995.36 us |   421.8750 |   11.7188 |         - |   3449.01 KB |
| RFC4180_Validate_Success_10Cols_by_100kRows |  19,832.76 us |   208.659 us |   195.180 us |  19,839.12 us |  19,485.10 us |  20,134.68 us |  4187.5000 |   31.2500 |         - |  34386.66 KB |
| RFC4180_Validate_Success_20Cols_by_100Rows  |      65.82 us |     0.754 us |     0.705 us |      65.76 us |      64.58 us |      67.40 us |     9.2773 |    0.3662 |         - |     76.53 KB |
| RFC4180_Validate_Success_20Cols_by_1kRows   |     576.61 us |    11.173 us |    14.528 us |     571.02 us |     562.35 us |     608.57 us |    78.1250 |    2.9297 |         - |    639.76 KB |
| RFC4180_Validate_Success_20Cols_by_10kRows  |   5,882.52 us |   116.035 us |   231.734 us |   5,893.18 us |   5,555.09 us |   6,288.86 us |   765.6250 |   23.4375 |         - |   6271.95 KB |
| RFC4180_Validate_Success_20Cols_by_100kRows |  57,354.07 us |   520.927 us |   406.705 us |  57,327.27 us |  56,643.64 us |  57,910.21 us |  7555.5556 |         - |         - |  62592.38 KB |
| RFC4180_Validate_Success_40Cols_by_100Rows  |     118.68 us |     1.021 us |     0.955 us |     118.58 us |     116.94 us |     120.62 us |    14.8926 |    0.6104 |         - |    122.39 KB |
| RFC4180_Validate_Success_40Cols_by_1kRows   |   1,115.89 us |     7.513 us |     6.660 us |   1,115.82 us |   1,101.31 us |   1,125.36 us |   126.9531 |    5.8594 |         - |   1051.26 KB |
| RFC4180_Validate_Success_40Cols_by_10kRows  |  10,739.27 us |    65.260 us |    61.045 us |  10,727.43 us |  10,639.74 us |  10,841.84 us |  1265.6250 |   46.8750 |         - |  10339.74 KB |
| RFC4180_Validate_Success_40Cols_by_100kRows | 108,549.95 us |   887.478 us |   830.147 us | 108,715.76 us | 107,112.43 us | 109,922.70 us | 12600.0000 |         - |         - | 103222.68 KB |
| Content_Validate_Success_10Cols_by_100Rows  |     111.21 us |     1.298 us |     1.150 us |     111.26 us |     108.24 us |     112.87 us |    26.3672 |    4.8828 |         - |    216.69 KB |
| Content_Validate_Success_10Cols_by_1kRows   |     624.24 us |    12.343 us |    13.207 us |     626.72 us |     588.64 us |     637.52 us |   177.7344 |   35.1563 |         - |   1454.24 KB |

## .NET 8 Benchmark Results

BenchmarkDotNet v0.14.0, Fedora Linux 43 (Server Edition)
AMD Ryzen 7 7840U w/ Radeon 780M Graphics, 4 CPU, 4 logical and 4 physical cores
.NET SDK 8.0.122
[Host]     : .NET 8.0.22 (8.0.2225.52707), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
DefaultJob : .NET 8.0.22 (8.0.2225.52707), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI


| Method                                      | Mean          | Error        | StdDev     | Median        | Min           | Max           | Gen0       | Gen1      | Gen2      | Allocated    |
|-------------------------------------------- |--------------:|-------------:|-----------:|--------------:|--------------:|--------------:|-----------:|----------:|----------:|-------------:|
| RFC4180_Validate_Failed_10Cols_by_100Rows   |      66.75 us |     0.519 us |   0.485 us |      66.60 us |      66.22 us |      67.78 us |     7.9346 |    0.6104 |         - |     65.03 KB |
| RFC4180_Validate_Failed_10Cols_by_1kRows    |     583.83 us |     2.799 us |   2.337 us |     584.36 us |     579.65 us |     588.08 us |    65.4297 |   24.4141 |         - |    536.86 KB |
| RFC4180_Validate_Failed_10Cols_by_10kRows   |   6,148.50 us |    86.318 us |  80.742 us |   6,141.20 us |   6,068.38 us |   6,306.78 us |   632.8125 |  453.1250 |         - |   5197.91 KB |
| RFC4180_Validate_Failed_10Cols_by_100kRows  |  81,665.03 us |   845.205 us | 705.784 us |  81,806.97 us |  80,524.30 us |  83,090.58 us |  6857.1429 | 2857.1429 | 1142.8571 |  53407.61 KB |
| RFC4180_Validate_Success_10Cols_by_100Rows  |      30.00 us |     0.592 us |   1.141 us |      29.82 us |      28.24 us |      32.97 us |     5.5847 |    0.1831 |         - |     45.81 KB |
| RFC4180_Validate_Success_10Cols_by_1kRows   |     208.12 us |     4.132 us |   7.761 us |     207.63 us |     196.96 us |     226.09 us |    43.4570 |    1.4648 |         - |    355.19 KB |
| RFC4180_Validate_Success_10Cols_by_10kRows  |   2,022.32 us |    40.308 us |  63.933 us |   2,030.39 us |   1,908.48 us |   2,131.42 us |   421.8750 |   11.7188 |         - |   3449.02 KB |
| RFC4180_Validate_Success_10Cols_by_100kRows |  20,532.65 us |   406.511 us | 399.248 us |  20,501.80 us |  20,043.12 us |  21,490.32 us |  4187.5000 |   31.2500 |         - |  34386.66 KB |
| RFC4180_Validate_Success_20Cols_by_100Rows  |      66.39 us |     0.627 us |   0.586 us |      66.26 us |      65.53 us |      67.51 us |     9.2773 |    0.3662 |         - |     76.56 KB |
| RFC4180_Validate_Success_20Cols_by_1kRows   |     559.16 us |    10.825 us |   9.596 us |     561.12 us |     544.46 us |     574.39 us |    78.1250 |    2.9297 |         - |    639.77 KB |
| RFC4180_Validate_Success_20Cols_by_10kRows  |   5,427.94 us |    37.346 us |  34.934 us |   5,425.56 us |   5,380.85 us |   5,494.67 us |   765.6250 |   23.4375 |         - |   6271.95 KB |
| RFC4180_Validate_Success_20Cols_by_100kRows |  51,974.01 us |   434.503 us | 362.830 us |  52,017.37 us |  51,275.85 us |  52,527.22 us |  7600.0000 |         - |         - |  62592.38 KB |
| RFC4180_Validate_Success_40Cols_by_100Rows  |     123.31 us |     2.462 us |   5.085 us |     120.64 us |     117.72 us |     135.61 us |    14.8926 |    0.4883 |         - |    122.42 KB |
| RFC4180_Validate_Success_40Cols_by_1kRows   |   1,061.11 us |     6.919 us |   6.134 us |   1,058.34 us |   1,053.66 us |   1,074.52 us |   126.9531 |    5.8594 |         - |   1051.27 KB |
| RFC4180_Validate_Success_40Cols_by_10kRows  |  10,274.56 us |    84.561 us |  79.099 us |  10,233.40 us |  10,181.93 us |  10,433.64 us |  1265.6250 |   46.8750 |         - |  10339.74 KB |
| RFC4180_Validate_Success_40Cols_by_100kRows | 101,943.07 us | 1,013.905 us | 948.408 us | 102,050.47 us | 100,609.45 us | 103,355.65 us | 12600.0000 |         - |         - | 103222.69 KB |
| Content_Validate_Success_10Cols_by_100Rows  |     130.53 us |     2.120 us |   2.441 us |     129.67 us |     128.02 us |     137.44 us |    25.3906 |    4.8828 |         - |    215.23 KB |
| Content_Validate_Success_10Cols_by_1kRows   |     737.48 us |    14.333 us |  14.077 us |     738.06 us |     710.37 us |     761.54 us |   177.7344 |   35.1563 |         - |   1452.76 KB |


