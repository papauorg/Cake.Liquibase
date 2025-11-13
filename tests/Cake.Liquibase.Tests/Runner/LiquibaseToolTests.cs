using System;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Tooling;
using Cake.Liquibase.Runner.LiquibaseCommands;
using Cake.Liquibase.Tests.Helpers;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Cake.Liquibase.Tests
{
    public class LiquibaseToolTests
    {
        protected LiquibaseTool _runner;
        protected LiquibaseSettings _settings;

        protected IProcessRunner _processRunner;
        protected ICakeLog _cakeLog;
        protected IToolLocator _cakeTools;
        protected readonly IGlobber _globber;
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;

        public LiquibaseToolTests()
        {
            _fileSystem = Substitute.For<IFileSystem>();
            _environment = Substitute.For<ICakeEnvironment>();
            _processRunner = Substitute.For<IProcessRunner>();
            _cakeLog = Substitute.For<ICakeLog>();
            _cakeTools = Substitute.For<IToolLocator>();
            _globber = new PassThroughGlobber();

            _settings = new LiquibaseSettings();
            _runner = new LiquibaseTool(_fileSystem, _environment, _processRunner, _cakeTools, _globber, _cakeLog);
        }

        public class TheStartMethod : LiquibaseToolTests
        {
            [Fact]
            public void Throws_If_LiquibaseSettings_Is_Null()
            {
                Action startCall = () => _runner.Start(Commands.Update, null);
                startCall.ShouldThrow<ArgumentNullException>();
            }
        }
    }
}