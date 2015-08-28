//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var configurationName = "Release";
var target = Argument("target", "Default");
var configuration = Argument("configuration", configurationName);

var packageVersion = string.Empty;
var product = "Codestellation.Pulsar";
var copyright = string.Format("Copyright (c) Codestellation Team 2015 - {0}", DateTime.Now.Year);

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
var buildDirInfo = new DirectoryInfo("./build");
var buildDir = Directory(buildDirInfo.FullName);
var nugetDir = Directory("./nuget");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    CleanDirectory(buildDir);
    CleanDirectory(nugetDir);
});

//////////////////////////////////////////////////////////////////////

Task("Generate-Solution-Version")
    .Does(() =>
{
    var command = new ProcessSettings
    {
        Arguments = "describe --abbrev=7 --first-parent --long --dirty --always",
        RedirectStandardOutput = true
    };
    IEnumerable<string> output;
    var exitCode = StartProcess("git", command, out output);

    var describe = output.Single();

    Information("Git describe is '{0}'", describe);
    var annotatedTagPattern = @"(?<major>[0-9]+).(?<minor>[0-9]+)-(?<revision>[0-9]+)-g(?<hash>[\w]+)-?(?<dirty>[\w]+)*";
    var parts = System.Text.RegularExpressions.Regex.Match(describe, annotatedTagPattern);

    string major = "0";
    string minor = "0";
    string revision = "0";
    string build = "0"; // get it from appveyor
    string hash = string.Empty;
    string dirty = string.Empty;

    if(parts.Success)
    {
      major = parts.Groups["major"].Value;
      minor = parts.Groups["minor"].Value;
      revision = parts.Groups["revision"].Value;
      hash = parts.Groups["hash"].Value;
      dirty = parts.Groups["dirty"].Value;
    }
    else
    {
        var tokens = describe.Split('-');
        hash = tokens[0];
        if(tokens.Length > 1)
        {
          dirty = tokens[1];
        }
    }

    if(AppVeyor.IsRunningOnAppVeyor)
    {
        build = AppVeyor.Environment.Build.Number.ToString();
    }

    var assemblyVersion = string.Format("{0}.{1}", major, minor);
    var fullVersion = string.Format("{0}.{1}.{2}.{3}", major, minor, revision, build);

    packageVersion = fullVersion;
    if(!string.IsNullOrWhiteSpace(dirty))
    {
        packageVersion += ("-" + dirty);
    }

    var infoVersion = string.Format("{0} {1}", packageVersion, hash);

    var asmInfo = new AssemblyInfoSettings
    {
        Product = product,
        Version = assemblyVersion,
        FileVersion = fullVersion,
        InformationalVersion = infoVersion,
        Copyright = copyright
    };

    var file = "./src/SolutionVersion.cs";
    CreateAssemblyInfo(file, asmInfo);

    Information("AssemblyVersion is '{0}'", infoVersion);
});

//////////////////////////////////////////////////////////////////////

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
{
    NuGetRestore("./src/Pulsar.sln");
});

//////////////////////////////////////////////////////////////////////

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .IsDependentOn("Generate-Solution-Version")
    .Does(() =>
{
    MSBuild("./src/Pulsar.sln", settings => {
      settings.SetConfiguration(configuration);
      settings.Properties["OutDir"] = new List<string>{ buildDirInfo.FullName };
      settings.Verbosity = Verbosity.Minimal;
      });
});

//////////////////////////////////////////////////////////////////////

Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
{
     var testsAssembly = buildDirInfo
         .EnumerateFiles("*.Tests.dll", SearchOption.AllDirectories)
         .Select(x => x.FullName)
         .First();

     var nunitPath = new DirectoryInfo("./")
        .EnumerateFiles("nunit-console.exe", SearchOption.AllDirectories)
        .First();

     var settings = new NUnitSettings { ToolPath = nunitPath.FullName };
     NUnit(testsAssembly, settings);
});


//////////////////////////////////////////////////////////////////////

Task("Pack")
    .IsDependentOn("Test")
    .Does(() =>
{

var nuGetPackSettings   = new NuGetPackSettings {
    Id                      = product,
    Version                 = packageVersion,
    Title                   = "Codestellation Pulsar",
    Authors                 = new[] {"Jury Soldatenkov"},
    Owners                  = new[] {"Codestellation Team"},
    Description             = "Lean scheduler abstraction over timers for using task. Supports cron expressions.",
    //Summary                 = "Excellent summare of what the package does",
    ProjectUrl              = new Uri("https://github.com/Codestellation/Pulsar"),
    //IconUrl                 = new Uri("http://cdn.rawgit.com/SomeUser/TestNuget/master/icons/testnuget.png"),
    LicenseUrl              = new Uri("https://github.com/Codestellation/Pulsar/blob/master/license.txt"),
    Copyright               = copyright,
    //ReleaseNotes            = new [] {"Bug fixes", "Issue fixes", "Typos"},
    Tags                    = new [] {"Cron", "Task", "Scheduler", "Timer"},
    RequireLicenseAcceptance= false,
    Symbols                 = false,
    //NoPackageAnalysis       = true,
    Files                   = new [] { new NuSpecContent {Source = "Codestellation.Pulsar.???", Target = "lib/portable-net451+win81+wpa81"}, },
    BasePath                = "./build",
    OutputDirectory         = "./nuget"
};

NuGetPack("./src/Pulsar/pulsar.nuspec", nuGetPackSettings);
});

Task("Push")
    .IsDependentOn("Pack")
    .Does(() =>
{

  // Get the path to the package.
  var package = string.Format("./nuget/{0}.{1}.nupkg", product, packageVersion);

  // Push the package.
  NuGetPush(package, new NuGetPushSettings {
      Source = "https://www.myget.org/F/codestellation/api/v2/package",
      ApiKey = EnvironmentVariable("myget_key")
  });
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
