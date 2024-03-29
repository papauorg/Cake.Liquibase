# Cake.Liquibase
Addin for running liquibase database migrations with the Cake build system.

[![NuGet](https://img.shields.io/nuget/v/Cake.Liquibase.svg)](https://www.nuget.org/packages/Cake.Liquibase)

## Dependencies
Please note that neither the required Java Runtime Environment (JRE) nor the liquibase executables are provided with this
addin and need to be provided separately.

## NuGet package
As of now there is only a preview NuGet package available. An official one will be created as soon as I'm satisfied with the code to release a first version.

## Usage
```csharp
#addin "Cake.Liquibase"

var target = Argument("target", "Default");

Task("Default")
    .Does(() => {
        // Install Liquibase.Cli package for the liquibase executables (or include it in the packages.config) 
        NuGetInstall("Liquibase.Cli", new NuGetInstallSettings {
            Version  = "3.3.5",
            OutputDirectory = "./tools"
        });

        UpdateDatabase(s => {
            s.ChangeLogFile = "YourChangeLog.xml";
            s.Url = "jdbc:sqlite:exampledb.sqlite";
            s.JavaSettings.Classpaths.Add("./sqlite-jdbc-3.20.0.jar"); // additional drivers / jar files
        });
    });

RunTarget(target);
```
When not specifying a java executable the PATH is searched for "java" and it is used when found. 

## Limitations
This is currently an early stage of the addin. As of now only the liquibase "update" command is supported. 
The most important settings can be used via the LiquibaseSettings class. If you are missing settings, you can 
use the LiquibaseSettings.ArgumentCustomization property to define them as string. Details on how to use them can
be found on the Cake website (ToolSettings).

If you are missing a command or parameter feel free to send a pull request.
