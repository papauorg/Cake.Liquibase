using System;
using System.Linq;
using Cake.Core.IO;
using Cake.Liquibase.Runner;
using Cake.Liquibase.Runner.LiquibaseCommands;

namespace Cake.Liquibase.Helpers
{
    public class ArgumentBuilder
    {
        public FilePath LiquibaseJar {get; private set;}
        public LiquibaseCommand Command { get; private set; }
        public LiquibaseSettings Settings { get; private set; }

        public ArgumentBuilder(LiquibaseCommand command, LiquibaseSettings settings, FilePath liquibaseJar)
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
            var arguments = $"liquibase.integration.commandline.Main {commandLineArgs} {Command.ToString()}";
            if (!string.IsNullOrWhiteSpace(javaOptions)) 
                arguments = $"{javaOptions} {arguments}";
            
            return arguments;
        }

        private string BuildJavaOptions()
        {
            return "";
        }

        private string BuildCommandLineArgs()
        {
            return "";
        }
    }
}