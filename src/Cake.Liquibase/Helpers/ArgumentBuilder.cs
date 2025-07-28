using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cake.Core;
using Cake.Core.IO;
using Cake.Liquibase;
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
        public IGlobber Globber {get; private set;}

        public ArgumentBuilder(BaseCommand command, LiquibaseSettings settings, FilePath liquibaseJar, IGlobber globber)
        {
            Globber = globber ?? throw new ArgumentNullException(nameof(globber));
            Command = command ?? throw new ArgumentNullException(nameof(command));
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            LiquibaseJar = liquibaseJar ?? throw new ArgumentNullException(nameof(liquibaseJar));
        }


        public ProcessArgumentBuilder Prepare()
        {
            var argumentBuilder = new ProcessArgumentBuilder();

            AppendJavaOptions(argumentBuilder);
            argumentBuilder.Append(LIQUIBASE_ENTRY_POINT);
            AppendCommandLineArgs(argumentBuilder);
            argumentBuilder.Append(Command.ToString());

            return argumentBuilder;
        }

        private void AppendJavaOptions(ProcessArgumentBuilder builder)
        {
            var classPaths = new List<string>();
            foreach(string pathPattern in Settings?.JavaSettings?.Classpaths)
            {
                if (string.IsNullOrWhiteSpace(pathPattern))
                {
                    continue;
                }

                if (pathPattern == "."){
                    classPaths.Add(pathPattern);
                    continue;
                }

                var foundFiles = Globber.GetFiles(pathPattern)?.Select(f => f.FullPath);
                if (foundFiles != null) 
                {
                    classPaths.AddRange(foundFiles);
                }
            }
            
            // add liquibase jar file to classpath
            classPaths.Add(LiquibaseJar.FullPath);

            var classPath = string.Join(System.IO.Path.PathSeparator.ToString(), classPaths);
            
            if (!string.IsNullOrWhiteSpace(classPath))
                builder.Append($"-cp \"{classPath}\"");

            if (!string.IsNullOrWhiteSpace(Settings.JavaSettings.Options))
                builder.Append(Settings.JavaSettings.Options.Trim());
        }

        private void AppendCommandLineArgs(ProcessArgumentBuilder builder)
        {
            // Required parameters
            builder.AppendQuotedIfNotEmpty("--changeLogFile", Settings.ChangeLogFile);
            builder.AppendQuotedIfNotEmpty("--username", Settings.Username);
            builder.AppendQuotedIfNotEmpty("--password", Settings.Password);
            builder.AppendQuotedIfNotEmpty("--url", Settings.Url);
            builder.AppendQuotedIfNotEmpty("--driver", Settings.DriverClassName);

            // Optional parameters
            builder.AppendQuotedIfNotEmpty("--contexts", String.Join(",", Settings.Contexts));
            builder.AppendQuotedIfNotEmpty("--defaultSchemaName", Settings.DefaultSchemaName);
            builder.AppendQuotedIfNotEmpty("--defaultsFile", Settings.DefaultsFile);

            Settings.ArgumentCustomization?.Invoke(builder);
        }
    }

    internal static class ArgumentExtensions
    {
        internal static void AppendQuotedIfNotEmpty(this ProcessArgumentBuilder builder, string parameter, string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
                builder.AppendIfNotEmpty(parameter, $"\"{value}\"");
        }

        internal static void AppendIfNotEmpty(this ProcessArgumentBuilder builder, string parameter, string value)
        {
            if (!string.IsNullOrWhiteSpace(value) && !string.IsNullOrWhiteSpace(parameter))
                builder.Append($"{parameter}={value}");
        }
    }
}