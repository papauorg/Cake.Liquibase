#reference "../../output/Cake.Liquibase.dll"

var target = Argument("target", "Default");

Task("Default")
    .Does(() => {
        UpdateDatabase(s => {
            s.ChangeLogFile = "TestChangeLog.xml";
            s.Url = "jdbc:sqlite:exampledb.sqlite";
            s.JavaSettings.Classpaths.Add("./sqlite-jdbc-3.20.0.jar");
            s.LiquibaseJar = "./liquibase/liquibase*.jar";
        });
    });

RunTarget(target);
