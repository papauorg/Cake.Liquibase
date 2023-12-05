var target = Argument("target", "Default");
var version = Argument("packageVersion", "0.0.1");

var solution = "./Cake.Liquibase.sln";

var outputDirRoot = new DirectoryPath("./BuildArtifacts/").MakeAbsolute(Context.Environment);
var outputDirPackage = outputDirRoot.Combine("Package");

Task("Clean")
    .Does(() => {
        EnsureDirectoryDoesNotExist(outputDirRoot, new DeleteDirectorySettings {
			Recursive = true,
			Force = true
		});
		CreateDirectory(outputDirRoot);	
    });

Task("Build")
    .IsDependentOn("Clean")
    .Does(() => {
        var msBuildSettings = new DotNetMSBuildSettings();
        msBuildSettings.PackageVersion = version;
        msBuildSettings.WithProperty("PackageOutputPath", outputDirPackage.FullPath);
        
        var settings = new DotNetBuildSettings
        {
            Configuration = "Release",
            MSBuildSettings = msBuildSettings
        };

        DotNetBuild(solution, settings);
    });

Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
    {
        var projectFiles = GetFiles("./tests/**/*.csproj");
        foreach(var file in projectFiles)
        {
            DotNetTest(file.FullPath);
        }
    });

Task("Default")
    .IsDependentOn("Test");

RunTarget(target);