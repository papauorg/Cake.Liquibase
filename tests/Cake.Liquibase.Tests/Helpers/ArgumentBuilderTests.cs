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

            [Fact]
            public void Appends_The_Classpath_Parameter_If_An_Item_Exists_In_The_Java_Settings()
            {
                var settings = new LiquibaseSettings();
                settings.JavaSettings.Classpaths.Clear();
                settings.JavaSettings.Classpaths.Add("./some/path/file.jar");
                
                var arguments = new ArgumentBuilder(
                    LiquibaseCommand.Update, 
                    settings,    
                    new FilePath("somefile.jar")
                ).Build();

                arguments.Should().StartWith("-cp \"./some/path/file.jar\"");
            }

            [Fact]
            public void Does_Not_Append_Empty_Classpath_Parameters()
            {
                var settings = new LiquibaseSettings();
                settings.JavaSettings.Classpaths.Clear();
                settings.JavaSettings.Classpaths.Add("");
                settings.JavaSettings.Classpaths.Add(" ");
                settings.JavaSettings.Classpaths.Add("\r\n");
                
                var arguments = new ArgumentBuilder(
                    LiquibaseCommand.Update, 
                    settings,    
                    new FilePath("somefile.jar")
                ).Build();

                arguments.Should().NotContain("-cp");
            }

            [Fact]
            public void Includes_Additional_Java_Options_If_Present()
            {
                var settings = new LiquibaseSettings();
                settings.JavaSettings.Classpaths.Clear();
                settings.JavaSettings.Options = "-Dsome.java.option=1";
                
                var arguments = new ArgumentBuilder(
                    LiquibaseCommand.Update, 
                    settings,    
                    new FilePath("somefile.jar")
                ).Build();

                arguments.Should().StartWith("-Dsome.java.option=1");
            }

            [Fact]
            public void Concatinates_Classpath_And_Additional_Java_Options()
            {
                var settings = new LiquibaseSettings();
                settings.JavaSettings.Classpaths.Clear();
                settings.JavaSettings.Classpaths.Add("./some/file.jar");
                settings.JavaSettings.Options = "-Dsome.java.option=1";
                
                var arguments = new ArgumentBuilder(
                    LiquibaseCommand.Update, 
                    settings,    
                    new FilePath("somefile.jar")
                ).Build();

                arguments.Should().StartWith("-cp \"./some/file.jar\" -Dsome.java.option=1");
            }
        }
    }
}