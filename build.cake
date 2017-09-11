var target = Argument("target", "Default");
var solution = "./Cake.Liquibase.sln";

Task("Clean")
    .Does(() => {
        DotNetCoreClean(solution);
    });

Task("Restore")
    .Does(() => {
        DotNetCoreRestore(solution);
    });

Task("Build")
    .IsDependentOn("Restore")
    .Does(() => {
        var settings = new DotNetCoreBuildSettings
        {
            Configuration = "Release",
            OutputDirectory = "./output/"
        };

        DotNetCoreBuild(solution, settings);
    });

Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
    {
        var projectFiles = GetFiles("./tests/**/*.csproj");
        foreach(var file in projectFiles)
        {
            DotNetCoreTest(file.FullPath);
        }
    });

Task("Pack")
    .IsDependentOn("Test")
    .Does(() => {
        DotNetCorePack("./src/Cake.Liquibase/Cake.Liquibase.csproj", new DotNetCorePackSettings {
            OutputDirectory = "./nuget/",
            NoBuild = true
        });
    });

Task("Default")
    .IsDependentOn("Test");

RunTarget(target);