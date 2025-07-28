using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Tooling;
using Cake.Liquibase.Runner.LiquibaseCommands;

namespace Cake.Liquibase;

public class LiquibaseTool : Tool<LiquibaseSettings>
{
    public ICakeEnvironment Environment { get; }
    public IGlobber Globber { get; }
    public ICakeLog Log { get; }
    public IToolLocator Tools { get; }

    public LiquibaseTool(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner, IToolLocator tools, IGlobber globber, ICakeLog log)
        : base(fileSystem, environment, processRunner, tools)
    {
        Environment = environment ?? throw new ArgumentNullException(nameof(environment));
        Globber = globber ?? throw new ArgumentNullException(nameof(globber));
        Log = log ?? throw new ArgumentNullException(nameof(log));
        Tools = tools ?? throw new ArgumentNullException(nameof(tools));
    }

    protected override IEnumerable<string> GetToolExecutableNames()
    {
        yield return Environment.Platform.IsUnix() ? "java" : "java.exe";
    }

    protected override string GetToolName()
    {
        return "Liquibase";
    }


    /// <summary>
    /// Runs liquibase against a database using the update parameter.
    /// </summary>
    /// <returns></returns>
    public void Start(BaseCommand command, LiquibaseSettings settings)
    {
        if (command == null)
            throw new ArgumentNullException("command");

        if (settings == null)
            throw new ArgumentNullException("settings");

        var liquibaseJar = ResolveLiquibaseJarFile(settings.LiquibaseJar);
        if (liquibaseJar == null)
            throw new ArgumentException($"Liquibase jar file not found under '{settings.LiquibaseJar}'");

        var arguments = new Helpers.ArgumentBuilder(command, settings, liquibaseJar, Globber).Prepare();
        var processSettings = GetProcessSettings(arguments, settings);

        Run(settings, arguments, processSettings, p => RedirectErrorOutput(p.GetStandardError(), p.GetExitCode()));
    }

    private void RedirectErrorOutput(IEnumerable<string> messages, int exitCode)
    {
        LogLevel level = LogLevel.Information;
        if (exitCode != 0)
        {
            level = LogLevel.Error;
        }

        foreach (string message in messages)
        {
            Log.Write(Verbosity.Minimal, level, message, null);
        }
    }

    private ProcessSettings GetProcessSettings(ProcessArgumentBuilder arguments, LiquibaseSettings settings)
    {
        var processSettings = new ProcessSettings
        {
            Arguments = arguments,
            RedirectStandardError = true
        };

        if (settings.WorkingDirectory != null)
        {
            processSettings.UseWorkingDirectory(settings.WorkingDirectory);
        }

        return processSettings;
    }

    private string ResolveLiquibaseJarFile(string liquibaseJarPattern)
    {
        Path jarFile = null;

        // shortcut if jar file was already resolved
        if (System.IO.File.Exists(liquibaseJarPattern))
            return liquibaseJarPattern;

        try
        {
            jarFile = Tools.Resolve(liquibaseJarPattern);
        }
        catch (ArgumentException)
        {
            // illegal characters in path (when using *)
            // fall back to globber.
        }

        if (jarFile == null)
        {
            // try file globber
            jarFile = Globber.GetFiles(liquibaseJarPattern).FirstOrDefault();
        }

        return jarFile?.FullPath;
    }
}