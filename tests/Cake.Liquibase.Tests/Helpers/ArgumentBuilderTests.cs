using Cake.Liquibase.Runner;
using Cake.Liquibase.Runner.LiquibaseCommands;
using Xunit;
using Cake.Liquibase.Helpers;
using NSubstitute;
using FluentAssertions;
using Cake.Core.IO;

namespace Cake.Liquibase.Tests.Helpers
{
    public class ArgumentBuilderTests
    {
        public class TheBuildMethod : ArgumentBuilderTests
        {
            [Fact]
            public void Maps_The_Command_Parameter_To_A_Liquibase_Command()
            {
                var arguments = new ArgumentBuilder(LiquibaseCommand.Update, new LiquibaseSettings(), new FilePath("someFile.jar")).Build();

                arguments.Should().EndWith("update");
            }
        }
    }
}