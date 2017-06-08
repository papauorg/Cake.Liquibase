using System;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Tooling;
using Cake.Liquibase.Runner;
using Cake.Liquibase.Runner.LiquibaseCommands;
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

        public LiquibaseRunnerTests()
        {
            _processRunner = Substitute.For<IProcessRunner>();
            _cakeLog = Substitute.For<ICakeLog>();
            _cakeTools = Substitute.For<IToolLocator>();
            
            _settings = new LiquibaseSettings();
            _runner = new LiquibaseRunner(_processRunner, _cakeLog, _cakeTools);
        }

        public class TheConstructor : LiquibaseRunnerTests
        {
            [Fact]
            public void Throws_If_ProcessRunner_Is_Null()
            {
                Action instantiation = () => new LiquibaseRunner(null, _cakeLog, _cakeTools);
                instantiation.ShouldThrow<ArgumentNullException>();
            }

            [Fact]
            public void Throws_If_Log_Is_Null()
            {
                Action instantiation = () => new LiquibaseRunner(_processRunner, null, _cakeTools);
                instantiation.ShouldThrow<ArgumentNullException>();
            }

            [Fact]
            public void Throws_If_Tools_Is_Null()
            {
                Action instantiation = () => new LiquibaseRunner(_processRunner, _cakeLog, null);
                instantiation.ShouldThrow<ArgumentNullException>();
            }
        }

        public class TheStartMethod : LiquibaseRunnerTests
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