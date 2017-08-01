using System;
using System.Linq;
using System.Text;
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
            var commandLineArgs = new StringBuilder();
            
            // Required parameters
            commandLineArgs.AppendQuotedIfNotEmpty("--changeLogFile", Settings.ChangeLogFile);
            commandLineArgs.AppendQuotedIfNotEmpty("--username", Settings.Username);
            commandLineArgs.AppendQuotedIfNotEmpty("--password", Settings.Password);
            commandLineArgs.AppendQuotedIfNotEmpty("--url", Settings.Url);
            commandLineArgs.AppendQuotedIfNotEmpty("--driver", Settings.DriverClassName);

            // Optional parameters
            commandLineArgs.AppendQuotedIfNotEmpty("--contexts", String.Join(",", Settings.Contexts));
            commandLineArgs.AppendQuotedIfNotEmpty("--defaultSchemaName", Settings.DefaultSchemaName);
            commandLineArgs.AppendQuotedIfNotEmpty("--defaultsFile", Settings.DefaultsFile);

            // not yet implemented specifically ...
            if (!string.IsNullOrWhiteSpace(Settings.OtherParameters))
                commandLineArgs.Append(" ").Append(Settings.OtherParameters);

            return commandLineArgs.ToString().Trim();
        }
    }

    internal static class ArgumentExtensions
    {
        internal static void AppendQuotedIfNotEmpty(this StringBuilder builder, string parameter, string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
                builder.AppendIfNotEmpty(parameter, $"\"{value}\"");

        }

        internal static void AppendIfNotEmpty(this StringBuilder builder, string parameter, string value)
        {
            if (!string.IsNullOrWhiteSpace(value) && !string.IsNullOrWhiteSpace(parameter))
                builder.Append(" "); // keep distance to previous parameters
                builder.Append($"{parameter}={value}");
        }
    }
}