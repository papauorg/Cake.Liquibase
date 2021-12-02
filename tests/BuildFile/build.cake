#reference "../../src/Cake.Liquibase/bin/Release/net6.0/Cake.Liquibase.dll"
#tool "nuget:?package=Liquibase.Cli&version=3.3.5"

var target = Argument("target", "Default");

Task("Default")
    .Does(() => {       
        UpdateDatabase(s => {
            s.ChangeLogFile = "TestChangeLog.xml";
            s.Url = "jdbc:sqlite:exampledb.sqlite";
            s.JavaSettings.Classpaths.Add("./sqlite-jdbc-3.20.0.jar");
        });
    });

RunTarget(target);
