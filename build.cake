var target = Argument("target", "Default");
var solution = "./Cake.Liquibase.sln";

Task("Clean")
    .Does(() => {
        DotNetClean(solution);
    });

Task("Restore")
    .Does(() => {
        DotNetRestore(solution);
    });

Task("Build")
    .IsDependentOn("Restore")
    .Does(() => {
        var settings = new DotNetBuildSettings
        {
            Configuration = "Release",
            OutputDirectory = "./output/"
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

Task("Pack")
    .IsDependentOn("Test")
    .Does(() => {
        DotNetPack("./src/Cake.Liquibase/Cake.Liquibase.csproj", new DotNetPackSettings {
            OutputDirectory = "./nuget/",
            NoBuild = true
        });
    });

Task("Default")
    .IsDependentOn("Test");

RunTarget(target);