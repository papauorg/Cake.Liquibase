var target = Argument("target", "Default");
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
        var msBuildSettings = new DotNetMSBuildSettings()
            .WithProperty("PackageOutputPath", outputDirPackage.FullPath);	
        
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