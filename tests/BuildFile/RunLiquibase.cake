#reference "../output/Cake.Liquibase.dll"

using Cake.Liquibase.Aliases;

var target = Argument("target", "Default");

Target("Default")
    .Does(() => {
        UpdateDatabase(s => {
            s.ChangeLogFile = TestChangeLog.xml;
            s.Url = "jdbc:sqlite:exampledb.sqlite";
            s.ClassPaths.Add("./sqlite-jdbc-3.20.0.jar");
            s.LiquibaseJar = "liquibase/liquibase*.jar";
        });
    });