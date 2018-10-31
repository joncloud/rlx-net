using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Xunit;

namespace Rlx.Tests
{
    public class ReadmeTests
    {
        [Fact]
        public void VersionShouldMatchProject()
        {
            var projectVersion = XDocument.Load("../../../../../src/Rlx/Rlx.csproj")
                .Root
                .Elements("PropertyGroup")
                .Elements("VersionPrefix")
                .First()
                .Value + "-*";

            var readmeText = File.ReadAllText("../../../../../README.md");
            var left = readmeText.IndexOf("<ItemGroup>");
            var right = readmeText.IndexOf("</ItemGroup>") + 12;

            var readmeVersion = XElement.Parse(readmeText.Substring(left, right - left))
                .Elements("PackageReference")
                .Attributes("Version")
                .First()
                .Value;

            Assert.Equal(projectVersion, readmeVersion);
        }
    }
}
