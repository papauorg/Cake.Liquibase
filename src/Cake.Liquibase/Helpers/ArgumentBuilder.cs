using System;
using System.Linq;
using Cake.Core.IO;
using Cake.Liquibase.Runner;
using Cake.Liquibase.Runner.LiquibaseCommands;

namespace Cake.Liquibase.Helpers
{
    public class ArgumentBuilder
    {
        public static readonly string LIQUIBASE_ENTRY_POINT = "liquibase.integration.commandline.Main";
        public FilePath LiquibaseJar {get; private set;}
        public BaseCommand Command { get; private set; }
        public LiquibaseSettings Settings { get; private set; }

        public ArgumentBuilder(BaseCommand command, LiquibaseSettings settings, FilePath liquibaseJar)
        {
            if (settings == null) 
                throw new ArgumentNullException("settings");

            if (liquibaseJar == null)
                throw new ArgumentNullException("liquibaseJar");
                
            Command = command;
            Settings = settings;
            LiquibaseJar = liquibaseJar;
        }

        public string Build()
        {
            var commandLineArgs = BuildCommandLineArgs();
            var javaOptions = BuildJavaOptions();
            
            return $"{javaOptions} {LIQUIBASE_ENTRY_POINT} {commandLineArgs} {Command}".Trim();
        }

        private string BuildJavaOptions()
        {
            var javaOptions = "";

            var classPath = string.Join(";", Settings.JavaSettings.Classpaths.Where(cp => !string.IsNullOrWhiteSpace(cp)).Select(cp => cp.Trim()));
            if (!string.IsNullOrWhiteSpace(classPath))
                javaOptions += $"-cp \"{classPath}\"";

            if (!string.IsNullOrWhiteSpace(Settings.JavaSettings.Options))
                javaOptions += " " + Settings.JavaSettings.Options.Trim();

            return javaOptions;
        }

        private string BuildCommandLineArgs()
        {
            return "";
        }
    }
}