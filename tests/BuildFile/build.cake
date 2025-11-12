#reference "../../src/Cake.Liquibase/bin/Release/net10.0/Cake.Liquibase.dll"
#tool "nuget:?package=Liquibase.Cli&version=3.3.5&include=**/liquibase.jar"

var target = Argument("target", "Default");

Task("Default")
    .Does(() =>
    {
        ValidateChangeLog(s =>
        {
            s.ChangeLogFile = "TestChangeLog.xml";
            s.Url = "jdbc:sqlite:exampledb.sqlite";
            s.JavaSettings.Classpaths.Add("./sqlite-jdbc-3.20.0.jar");
            s.LiquibaseArgumentCustomization = args => args.Append("--logLevel=debug").Append("--logFile=RunDatabaseActions.log");
        });

        UpdateDatabase(s =>
        {
            s.ChangeLogFile = "TestChangeLog.xml";
            s.Url = "jdbc:sqlite:exampledb.sqlite";
            s.JavaSettings.Classpaths.Add("./sqlite-jdbc-3.20.0.jar");
            s.LiquibaseArgumentCustomization = args => args.Append("--logLevel=debug").Append("--logFile=RunDatabaseActions.log");
        });
    });

RunTarget(target);
