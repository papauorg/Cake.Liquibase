using System;
using System.Linq;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Tooling;
using Cake.Liquibase.Runner.LiquibaseCommands;

namespace Cake.Liquibase.Runner
{
    /// <summary>
    /// Builds up the command line and executes the liquibase jar file.
    /// </summary>
    public class LiquibaseRunner
    {
        public ICakeLog Log { get; private set; }
        public IProcessRunner ProcessRunner { get; private set; }
        public IToolLocator Tools { get; private set; }
        public IGlobber Globber {get; private set; }

        public LiquibaseRunner(IProcessRunner processRunner, ICakeLog log, IToolLocator tools, IGlobber globber)
        {
            ProcessRunner = processRunner ?? throw new ArgumentNullException(nameof(processRunner));
            Log = log ?? throw new ArgumentNullException(nameof(log));
            Tools = tools ?? throw new ArgumentNullException(nameof(tools));
            Globber = globber ?? throw new ArgumentNullException(nameof(Globber));
        }
        
        /// <summary>
        /// Runs liquibase against a database using the update parameter.
        /// </summary>
        /// <returns></returns>
        public int Start(BaseCommand command, LiquibaseSettings settings)
        {
            if (command == null)
                throw new ArgumentNullException("command");

            if (settings == null)
                throw new ArgumentNullException("settings");

            var liquibaseJar = ResolveLiquibaseJarFile(settings.LiquibaseJar);
            if (liquibaseJar == null)
                throw new ArgumentException($"Liquibase jar file not found under '{settings.LiquibaseJar}'");
            
            var javaExecutable = Tools.Resolve(settings.JavaSettings.Executable);
            if (javaExecutable == null)
                throw new ArgumentException($"The java executable could not be found under '{settings.JavaSettings.Executable}'.");
            
            var arguments = new Helpers.ArgumentBuilder(command, settings, liquibaseJar, Globber).Build();
            var processSettings = new ProcessSettings {
                Arguments = arguments
            };

            using (var process = this.ProcessRunner.Start(javaExecutable, processSettings))
            {
                process.WaitForExit();
                return process.GetExitCode();
            }
        }

        private string ResolveLiquibaseJarFile(string liquibaseJarPattern)
        {
            Path jarFile = null;

            try
            {
                jarFile = Tools.Resolve(liquibaseJarPattern);
            } 
            catch (ArgumentException)
            {
                // illegal characters in path (when using *)
                // fall back to globber.
            }

            if (jarFile == null) {
                // try file globber
                jarFile = Globber.GetFiles(liquibaseJarPattern).FirstOrDefault();
            }

            return jarFile?.FullPath;
        }
    }
}