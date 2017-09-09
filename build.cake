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

Task("Default")
    .IsDependentOn("Test");

RunTarget(target);