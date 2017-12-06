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
static void Main(string[] args) {
  string message = args.ElementAtOrDefault(0)
    .ToOption()
    .MapOrElse(() => "Missing Argument", msg => $"Echo: {msg}");
  
  Console.WriteLine(message);
}
```

And also MVC helpers:
```csharp
interface IDataService {
  OptionTask<Data> LoadDataAsync(Guid id);
  ResultTaks<Data, string> UpdateDataAsync(Data data);
}

class DataController : Controller {
  readonly IDataService _dataService;
  public DataController(IDataService dataService) =>
    _dataService = dataService;

  public Task<IActionResult> Get(Guid id) =>
    _dataService.LoadDataAsync(id).ToActionResult();

  public Task<IActionResult> Post([FromModel]Data data) =>
    _dataService.UpdateDataAsync(data).ToActionResult(_ => 200, _ => 400, x => Some(x));
}
```

For additional usage see [Tests][] and [MVC Tests][].

[Tests]: tests/Rlx.Tests
[MVC Tests]: tests/Rlx.MvcCore/Tests
