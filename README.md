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

For additional usage see [Tests][].

[Tests]: tests/Rlx.Tests
