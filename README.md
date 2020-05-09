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

You can initialize type before execute `ToList()`, but it is not necessary:
```cs
DbDataReaderMapper<SomeClass>.Initialize());
```

## NuGet package

* [Orbitfog.Core.Database.Mapper](https://www.nuget.org/packages/Orbitfog.Core.Database.Mapper)
