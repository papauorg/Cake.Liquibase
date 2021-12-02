#reference "../../src/Cake.Liquibase/bin/Release/net6.0/Cake.Liquibase.dll"

var target = Argument("target", "Default");

Task("Default")
    .Does(() => {
        // Install Liquibase.Cli package for the liquibase executables (or include it in the packages.config) 
        NuGetInstall("Liquibase.Cli", new NuGetInstallSettings {
            Version  = "3.3.5",
            OutputDirectory = "./tools"
        });

        UpdateDatabase(s => {
            s.ChangeLogFile = "TestChangeLog.xml";
            s.Url = "jdbc:sqlite:exampledb.sqlite";
            s.JavaSettings.Classpaths.Add("./sqlite-jdbc-3.20.0.jar");
        });
    });

RunTarget(target);
