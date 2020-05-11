Libraries for .NET.

Supported [target framework versions](https://docs.microsoft.com/en-us/dotnet/standard/frameworks):
* .NET Core 2.0 and above
* .NET Framework 4.5.1 and above
* .NET Standard 2.0 and above

# Orbitfog.Core.Database.Mapper

High-performance library base on `System.Linq.Expressions` to map `System.Data.Common.DbDataReader` to:
* public properties and fields in any class, e.g.: POCO or generated by ORM
* generic list with:
    * [built-in types](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/built-in-types)
    * enums
    * DateTime
    * DateTimeOffset
    * Guid

This library give you possibility change existing code, with low performance, for an existing database framework. Only requirements is access to `DbDataReader` object.

NuGet package - [Orbitfog.Core.Database.Mapper](https://www.nuget.org/packages/Orbitfog.Core.Database.Mapper)

## Configuration

Default behavior:
```cs
DbDataReaderMapperConfiguration.Default.CaseSensitive = true;
DbDataReaderMapperConfiguration.Default.UseProperties = true;
DbDataReaderMapperConfiguration.Default.UseFields = true;
```

You can initialize the class before execute `ToList()`, but it is not necessary:
```cs
DbDataReaderMapper<SomeClass>.Initialize());
```

Or changing behavior for the class:
```cs
DbDataReaderMapper<SomeClass>.Initialize(new DbDataReaderMapperConfiguration()
{
    CaseSensitive = false
});
```


## Examples

Example 1:
```cs
DbCommand command = ...;
```
```cs
List<SomeClass> list = DbDataReaderMapper<SomeClass>.ToList(command.ExecuteReader());
```

Example 2:
```cs
DbCommand command = ...;
```
```cs
List<int?> list = DbDataReaderMapper<int?>.ToList(command.ExecuteReader());
```

Example 3:
```cs
DbCommand command = ...;
```
```cs
List<string> list = DbDataReaderMapper<string>.ToList(command.ExecuteReader());
```

Example 4:
```cs
DbCommand command = ...;
```
```cs
List<SomeEnum> list = DbDataReaderMapper<SomeEnum>.ToList(command.ExecuteReader());
```

## Performance test

* Microsoft SQL Server on local machine
* Create database `Orbitfog.Core.Database.Mapper.PerformanceTestCli` on local machine
* Generate sql file by running `Orbitfog.Core.Database.Mapper.PerformanceDataGeneratorCli` and execute file on local database, this create table `[dbo].[Test1]` with 100 000 rows.
* Build `Orbitfog.Core.Database.Mapper.PerformanceTestCli` as `Release` and run from `Visual Studio`: `Debug` > `Start Without Debugging`

### Results for `.NET Core 3.1` on Windows, average time for 5 executions:

| Name | 10 rows | 100 rows | 1000 rows | 10000 rows | 100000 rows |
|:----|----:|----:|----:|----:|----:|
| HandCoded - first time | 1,0776 ms | 1,2077 ms | 5,391 ms  | 53,6101 ms | 560,3239 ms |
| HandCoded - second time | 0,5658 ms | 1,3117 ms | 5,3656 ms  | 53,275 ms | 581,3828 ms |
| OrbitfogCoreDatabaseMapper - first time | 0,5618 ms | 1,2725 ms | 5,8555 ms  | 53,9275 ms | 566,7234 ms |
| OrbitfogCoreDatabaseMapper - second time | 0,6159 ms | 1,3356 ms | 5,8459 ms  | 52,1557 ms | 590,8066 ms |
| Dapper (Query<T>) - first time | 0,8099 ms | 1,3562 ms | 5,8828 ms  | 57,9416 ms | 664,3176 ms |
| Dapper (Query<T>) - second time | 0,6125 ms | 1,334 ms | 5,4601 ms  | 59,8265 ms | 665,3886 ms |
