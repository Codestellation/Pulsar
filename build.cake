//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var configurationName = "Release";
var target = Argument("target", "Default");
var configuration = Argument("configuration", configurationName);
//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
var buildDir = Directory("./src/Example/bin") + Directory(configuration);

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    CleanDirectory(buildDir);
});

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
{
    NuGetRestore("./src/Pulsar.sln");
});

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
    MSBuild("./src/Pulsar.sln", settings =>
        settings.SetConfiguration(configuration));
});

Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
{
     var directory = new DirectoryInfo(@".\");
     var testsAssembly = directory
         .EnumerateFiles("*.Tests.dll", SearchOption.AllDirectories)
         .Select(x => x.FullName)
         .First(x => x.Contains(configurationName));
     Console.WriteLine(testsAssembly);
     NUnit(testsAssembly);
});


//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Test");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
