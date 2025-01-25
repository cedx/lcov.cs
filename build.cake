using static System.IO.File;
using System.Text.RegularExpressions;

var release = HasArgument("r") || HasArgument("release");
var target = Argument<string>("t", null) ?? Argument("target", "default");
var version = "1.0.0";

Task("build")
	.Description("Builds the project.")
	.Does(() => DotNetBuild("lcov.sln", new() { Configuration = release ? "Release" : "Debug" }));

Task("clean")
	.Description("Deletes all generated files.")
	.Does(() => EnsureDirectoryDoesNotExist("bin"))
	.DoesForEach(GetDirectories("*/obj"), dir => EnsureDirectoryDoesNotExist(dir, new() { Recursive = true }))
	.Does(() => CleanDirectory("var", fileSystemInfo => fileSystemInfo.Path.Segments[^1] != ".gitkeep"));

Task("format")
	.Description("Formats the source code.")
	.Does(() => DotNetFormat("lcov.sln"));

Task("publish")
	.Description("Publishes the package.")
	.WithCriteria(release)
	.IsDependentOn("default")
	.DoesForEach(["tag", "push origin"], action => StartProcess("git", $"{action} v{version}"));

Task("test")
	.Description("Runs the test suite.")
	.Does(() => DotNetTest());

Task("version")
	.Description("Updates the version number in the sources.")
	.DoesForEach(GetFiles("*/*.csproj"), file => {
		var pattern = new Regex(@"<VersionPrefix>\d+(\.\d+){2}</VersionPrefix>");
		WriteAllText(file.FullPath, pattern.Replace(ReadAllText(file.FullPath), $"<VersionPrefix>{version}</VersionPrefix>"));
	});

Task("default")
	.Description("The default task.")
	.IsDependentOn("clean")
	.IsDependentOn("version")
	.IsDependentOn("build");

RunTarget(target);
