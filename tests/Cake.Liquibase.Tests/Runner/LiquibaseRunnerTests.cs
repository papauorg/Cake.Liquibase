using System;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Tooling;
using Cake.Liquibase.Runner;
using Cake.Liquibase.Runner.LiquibaseCommands;
using Cake.Liquibase.Tests.Helpers;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Cake.Liquibase.Tests
{
    public class LiquibaseRunnerTests
    {
        protected LiquibaseRunner _runner;
        protected LiquibaseSettings _settings;
        
        protected IProcessRunner _processRunner;
        protected ICakeLog _cakeLog;
        protected IToolLocator _cakeTools;
        protected readonly IGlobber _globber;
        protected readonly ICakePlatform _platform;

        public LiquibaseRunnerTests()
        {
            _processRunner = Substitute.For<IProcessRunner>();
            _cakeLog = Substitute.For<ICakeLog>();
            _cakeTools = Substitute.For<IToolLocator>();
            _globber = new PassThroughGlobber();
            _platform = Substitute.For<ICakePlatform>();
    
            _settings = new LiquibaseSettings();
            _runner = new LiquibaseRunner(_processRunner, _cakeLog, _cakeTools, _globber, _platform);
        }

        public class TheConstructor : LiquibaseRunnerTests
        {
            [Fact]
            public void Throws_If_ProcessRunner_Is_Null()
            {
                Action instantiation = () => new LiquibaseRunner(null, _cakeLog, _cakeTools, _globber, _platform);
                instantiation.Should().Throw<ArgumentNullException>();
            }

            [Fact]
            public void Throws_If_Log_Is_Null()
            {
                Action instantiation = () => new LiquibaseRunner(_processRunner, null, _cakeTools, _globber, _platform);
                instantiation.Should().Throw<ArgumentNullException>();
            }

            [Fact]
            public void Throws_If_Tools_Is_Null()
            {
                Action instantiation = () => new LiquibaseRunner(_processRunner, _cakeLog, null, _globber, _platform);
                instantiation.Should().Throw<ArgumentNullException>();
            }

            [Fact]
            public void Throws_If_Globber_Is_Null()
            {
                Action instantiation = () => new LiquibaseRunner(_processRunner, _cakeLog, _cakeTools, null, _platform);
                instantiation.Should().Throw<ArgumentNullException>();
            }

            [Fact]
            public void Throws_If_Environment_Is_Null()
            {
                Action instantiation = () => new LiquibaseRunner(_processRunner, _cakeLog, _cakeTools, _globber, null);
                instantiation.Should().Throw<ArgumentNullException>();
            }
        }

        public class TheStartMethod : LiquibaseRunnerTests
        {
            [Fact]
            public void Throws_If_LiquibaseSettings_Is_Null()
            {
                Action startCall = () => _runner.Start(Commands.Update, null);
                startCall.Should().Throw<ArgumentNullException>();
            }
            
        }
    }
}