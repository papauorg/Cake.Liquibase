# Cake.Liquibase
Addin for running liquibase database migrations with the Cake build system.

[![Build status](https://ci.appveyor.com/api/projects/status/7e29fhkr58m8akf0?svg=true)](https://ci.appveyor.com/project/papauorg/cake-liquibase)

## Dependencies
Please note that neither the required Java Runtime Environment (JRE) nor the liquibase executables are provided with this
addin and need to be provided separately.

## NuGet package
As of now there is no NuGet package available. It will be created as soon as I'm satisfied with the code to release a first version.

## Usage
```csharp
// addin not yet available via nuget
#addin "Cake.Liquibase"

var target = Argument("target", "Default");

Task("Default")
    .Does(() => {
        UpdateDatabase(s => {
            s.ChangeLogFile = "YourChangeLog.xml";
            s.Url = "jdbc:sqlite:exampledb.sqlite";
            s.JavaSettings.Classpaths.Add("./sqlite-jdbc-3.20.0.jar"); // additional drivers / jar files
            s.LiquibaseJar = "./liquibase/liquibase.jar"; // path to your liquibase jar file
        });
    });

RunTarget(target);
```
When not specifying a java executable the PATH is searched for "java" and it is used when found. 

## Limitations
This is currently an early stage of the addin. As of now only the liquibase "update" command is supported. 
The most important settings can be used via the LiquibaseSettings class. If you are missing settings, you can 
use the LiquibaseSettings.OtherParameters property to define them as string.
If you are missing a command or parameter feel free to send a pull request.
