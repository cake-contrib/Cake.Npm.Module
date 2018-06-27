#tool "nuget:?package=GitVersion.CommandLine"
#load "helpers.cake"
#tool nuget:?package=docfx.console&version=2.33.2
#addin nuget:?package=Cake.DocFx&version=0.7.0

///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

///////////////////////////////////////////////////////////////////////////////
// GLOBAL VARIABLES
///////////////////////////////////////////////////////////////////////////////

var solutionPath = File("./src/Cake.Npm.Module.sln");
var projects = GetProjects(solutionPath);
var artifacts = "./dist/";
var testResultsPath = MakeAbsolute(Directory(artifacts + "./test-results"));
GitVersion versionInfo = null;
var frameworks = new List<string> { "netstandard2.0" };

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

Setup(ctx =>
{
	// Executed BEFORE the first task.
	Information("Running tasks...");
	versionInfo = GitVersion();
	Information("Building for version {0}", versionInfo.FullSemVer);
	Verbose("Building for " + string.Join(", ", frameworks));
});

Teardown(ctx =>
{
	// Executed AFTER the last task.
	Information("Finished running tasks.");
});

///////////////////////////////////////////////////////////////////////////////
// TASK DEFINITIONS
///////////////////////////////////////////////////////////////////////////////

Task("Clean")
	.Does(() =>
{
	// Clean solution directories.
	foreach(var path in projects.AllProjectPaths)
	{
		Information("Cleaning {0}", path);
		CleanDirectories(path + "/**/bin/" + configuration);
		CleanDirectories(path + "/**/obj/" + configuration);
	}
	Information("Cleaning common files...");
	CleanDirectory(artifacts);
});

Task("Restore")
	.Does(() =>
{
	// Restore all NuGet packages.
	Information("Restoring solution...");
	//NuGetRestore(solutionPath);
	foreach (var project in projects.AllProjectPaths) {
		DotNetCoreRestore(project.FullPath);
	}
});

Task("Build")
	.IsDependentOn("Clean")
	.IsDependentOn("Restore")
	.Does(() =>
{
	Information("Building solution...");
	foreach(var framework in frameworks) {
		foreach (var project in projects.SourceProjectPaths) {
			var settings = new DotNetCoreBuildSettings {
				Framework = framework,
				Configuration = configuration,
				NoIncremental = true,
			};
			DotNetCoreBuild(project.FullPath, settings);
		}
	}
	
});

Task("Run-Unit-Tests")
	.IsDependentOn("Build")
	.Does(() =>
{
	if (projects.TestProjects.Any()) {
		CreateDirectory(testResultsPath);


		var settings = new DotNetCoreTestSettings {
			Configuration = configuration
		};

		foreach(var project in projects.TestProjects) {
			DotNetCoreTest(project.Path.FullPath, settings);
		}
	}
});

Task("Generate-Docs").Does(() => {
	DocFxMetadata("./docfx/docfx.json");
	DocFxBuild("./docfx/docfx.json");
	Zip("./docfx/_site/", artifacts + "/docfx.zip");
});

Task("Post-Build")
	.IsDependentOn("Build")
	.IsDependentOn("Run-Unit-Tests")
	.IsDependentOn("Generate-Docs")
	.Does(() =>
{
	CreateDirectory(artifacts + "build");
	CreateDirectory(artifacts + "modules");
	foreach (var project in projects.SourceProjects) {
		CreateDirectory(artifacts + "build/" + project.Name);
		foreach (var framework in frameworks) {
			var frameworkDir = artifacts + "build/" + project.Name + "/" + framework;
			CreateDirectory(frameworkDir);
			var files = GetFiles(project.Path.GetDirectory() + "/bin/" + configuration + "/" + framework + "/" + project.Name +".*");
			CopyFiles(files, frameworkDir);
		}
	}
});

Task("NuGet")
	.IsDependentOn("Post-Build")
	.Does(() => 
{
	CreateDirectory(artifacts + "package");
	Information("Building NuGet package");
	var versionNotes = ParseAllReleaseNotes("./ReleaseNotes.md").FirstOrDefault(v => v.Version.ToString() == versionInfo.MajorMinorPatch);
	var content = GetContent(frameworks, projects);
	var settings = new NuGetPackSettings {
		Id				= "Cake.Npm.Module",
		Version			= versionInfo.NuGetVersionV2,
		Title			= "Cake npm Module",
		Authors		 	= new[] { "Alistair Chapman" },
		Owners			= new[] { "achapman", "cake-contrib" },
		Description		= "This Cake module adds support for the npm package manager when installing tools in your Cake build scripts.",
		ReleaseNotes	= versionNotes != null ? versionNotes.Notes.ToList() : new List<string>(),
		Summary			= "Adds npm support to Cake builds.",
		ProjectUrl		= new Uri("https://github.com/cake-contrib/Cake.Npm.Module"),
		LicenseUrl		= new Uri("https://raw.githubusercontent.com/cake-contrib/Cake.Npm.Module/master/LICENSE"),
		Copyright		= "Alistair Chapman 2017",
		Tags			= new[] { "cake", "build", "ci", "build", "npm", "node" },
		OutputDirectory = artifacts + "/package",
		Files			= content,
		//KeepTemporaryNuSpecFile = true
	};

	NuGetPack(settings);
});

Task("Default")
	.IsDependentOn("NuGet");

RunTarget(target);
