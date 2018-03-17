# Rlx.NET
[![Travis](https://img.shields.io/travis/joncloud/rlx-net.svg)](https://travis-ci.org/joncloud/rlx-net/)
[![NuGet](https://img.shields.io/nuget/v/Rlx.svg)](https://www.nuget.org/packages/Rlx/)

## Description
Rlx.NET provides core helper functions for abstracting null-references and errors.

## Licensing
Released under the MIT License.  See the [LICENSE][] file for further details.

[license]: LICENSE.md

## Installation
In the Package Manager Console execute

```powershell
Install-Package Rlx
```

Or update `*.csproj` to include a dependency on

```xml
<ItemGroup>
  <PackageReference Include="Rlx" Version="0.1.0-*" />
</ItemGroup>
```

## Usage
Sample echo program:
```csharp
using Rlx;

static void Main(string[] args) {
  string message = args.ElementAtOrDefault(0)
    .ToOption()
    .MapOrElse(() => "Missing Argument", msg => $"Echo: {msg}");
  
  Console.WriteLine(message);
}
```

Need to generically handle exceptions? Wrap up logic with `TryFunctions`:
```csharp
using System.IO;
using static Rlx.TryFunctions;

public class MyClass
{
  public static void Unsafe() =>
    throw new NotImplementedException();

  public static Result<Unit, Exception> Safe() =>
    Try(Unsafe);

  public static Result<Unit, IOException> IOSafe() =>
    Try(Unsafe).Catch<IOException>();
}
```

And also MVC helpers:
```csharp
using Rlx;
using Rlx.MvcCore;
using static Rlx.Functions;

interface IDataService {
  OptionTask<Data> LoadDataAsync(Guid id);
  ResultTask<Data, string> UpdateDataAsync(Data data);
}

class DataController : Controller {
  readonly IDataService _dataService;
  public DataController(IDataService dataService) =>
    _dataService = dataService;

  public Task<IActionResult> Get(Guid id) =>
    WithErrors().AndThen(_ => _dataService.LoadDataAsync(id))
      .ToActionResult();

  public Task<IActionResult> Post([FromModel]Data data) =>
    WithErrors().AndThen(_ => _dataService.UpdateDataAsync(data))
      .ToActionResult(_ => 200, _ => 400, x => Some(x));

  Result<Unit, string> WithErrors() =>
    ModelState.ToResult().Map(x => "Bad Request");
}
```

For additional usage see [Tests][] and [MVC Tests][].

[Tests]: tests/Rlx.Tests
[MVC Tests]: tests/Rlx.MvcCore/Tests
