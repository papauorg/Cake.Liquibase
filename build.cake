var target = Argument("target", "Default");


Task("Build")
  .IsDependendOn("Clean")
  .IsDependendOn("Restore")
  .Does(() => {
    var settings = new DotNetCoreBuildSettings
    {
        Framework = "netcoreapp1.0",
        Configuration = "Release",
        OutputDirectory = "./output/"
    };

    DotNetCoreBuild("./src/*", settings);
    DotNetCoreBuild("./tests/*", settings);
  });

Task("Default")
  .IsDependendOn("Build");

RunTarget(target);