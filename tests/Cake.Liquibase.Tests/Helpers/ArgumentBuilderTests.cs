using Cake.Liquibase.Runner;
using Xunit;
using Cake.Liquibase.Helpers;
using NSubstitute;
using FluentAssertions;
using Cake.Core.IO;
using Cake.Liquibase.Runner.LiquibaseCommands;

namespace Cake.Liquibase.Tests.Helpers
{
    public class ArgumentBuilderTests
    {
        protected IGlobber _globber;

        public ArgumentBuilderTests()
        {
            _globber = new PassThroughGlobber();
        }

        public class TheBuildMethod : ArgumentBuilderTests
        {

            [Fact]
            public void Maps_The_Command_Parameter_To_A_Liquibase_Command()
            {
                var arguments = new ArgumentBuilder(
                    Commands.Update, 
                    new LiquibaseSettings(), 
                    new FilePath("someFile.jar"),
                    _globber
                ).Prepare().Render();

                arguments.Should().EndWith("update");
            }

            [Fact]
            public void Appends_The_Classpath_Parameter_If_An_Item_Exists_In_The_Java_Settings()
            {
                var settings = new LiquibaseSettings();
                settings.JavaSettings.Classpaths.Clear();
                settings.JavaSettings.Classpaths.Add("some/path/file.jar");
                
                var arguments = new ArgumentBuilder(
                    Commands.Update, 
                    settings,    
                    new FilePath("somefile.jar"),
                    _globber
                ).Prepare().Render();

                arguments.Should().StartWith("-cp \"some/path/file.jar");
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
                    Commands.Update, 
                    settings,    
                    new FilePath("somefile.jar"),
                    _globber
                ).Prepare().Render();

                arguments.Should().Contain("-cp \"somefile.jar\""); // only contain the jar file, but nothing else in the -cp parameter
            }

            [Fact]
            public void Includes_The_LiquibaseJar_File_In_The_Classpath()
            {
                var settings = new LiquibaseSettings();
                settings.JavaSettings.Classpaths.Clear();

                var arguments = new ArgumentBuilder(
                    Commands.Update,
                    settings,
                    new FilePath("somefile.jar"),
                    _globber
                ).Prepare().Render();

                arguments.Should().Contain("-cp \"somefile.jar\"");
            }

            [Fact]
            public void Includes_Additional_Java_Options_If_Present()
            {
                var settings = new LiquibaseSettings();
                settings.JavaSettings.Classpaths.Clear();
                settings.JavaSettings.Options = "-Dsome.java.option=1";
                
                var arguments = new ArgumentBuilder(
                    Commands.Update, 
                    settings,    
                    new FilePath("somefile.jar"),
                    _globber
                ).Prepare().Render();

                arguments.Should().Contain("-Dsome.java.option=1");
            }

            [Fact]
            public void Concatinates_Classpath_And_Additional_Java_Options()
            {
                var settings = new LiquibaseSettings();
                settings.JavaSettings.Classpaths.Clear();
                settings.JavaSettings.Classpaths.Add("some/file.jar");
                settings.JavaSettings.Options = "-Dsome.java.option=1";
                
                var arguments = new ArgumentBuilder(
                    Commands.Update, 
                    settings,    
                    new FilePath("somefile.jar"),
                    _globber
                ).Prepare().Render();
                
                arguments.Should()
                    .StartWith("-cp \"some/file.jar")
                    .And.Contain("somefile.jar")
                    .And.Contain("-Dsome.java.option=1");
            }
            
            [Fact]
            public void Contains_The_Java_Entry_Class()
            {
                var arguments = new ArgumentBuilder(
                    Commands.Update, 
                    new LiquibaseSettings(), 
                    new FilePath("somefile.jar"),
                    _globber
                ).Prepare().Render();

                arguments.Should().Contain(ArgumentBuilder.LIQUIBASE_ENTRY_POINT);
            }

            [Fact]
            public void Contains_The_Quoted_Username()
            {
                var settings = new LiquibaseSettings(){
                    Username = "user"
                };

                var arguments = new ArgumentBuilder(
                    Commands.Update, 
                    settings, 
                    new FilePath("somefile.jar"),
                    _globber
                ).Prepare().Render();

                arguments.Should().Contain("--username=\"user\"");
            }

            [Fact]
            public void Contains_The_Quoted_Password()
            {
                var settings = new LiquibaseSettings(){
                    Password = "password"
                };

                var arguments = new ArgumentBuilder(
                    Commands.Update, 
                    settings, 
                    new FilePath("somefile.jar"),
                    _globber
                ).Prepare().Render();

                arguments.Should().Contain("--password=\"password\"");
            }

            [Fact]
            public void Contains_The_Quoted_ChangelogFile()
            {
                var settings = new LiquibaseSettings(){
                    ChangeLogFile = "./ChangeLog.xml"
                };

                var arguments = new ArgumentBuilder(
                    Commands.Update, 
                    settings, 
                    new FilePath("somefile.jar"),
                    _globber
                ).Prepare().Render();

                arguments.Should().Contain("--changeLogFile=\"./ChangeLog.xml\"");
            }

            [Fact]
            public void Contains_The_Quoted_Url()
            {
                var settings = new LiquibaseSettings(){
                    Url = "jdbc:sqlserver://server:1433;property=value"
                };

                var arguments = new ArgumentBuilder(
                    Commands.Update, 
                    settings, 
                    new FilePath("somefile.jar"),
                    _globber
                ).Prepare().Render();

                arguments.Should().Contain("--url=\"jdbc:sqlserver://server:1433;property=value");
            }

            [Fact]
            public void Contains_The_Quoted_Context()
            {
                
                var settings = new LiquibaseSettings();
                settings.Contexts.Add("production");

                var arguments = new ArgumentBuilder(
                    Commands.Update, 
                    settings, 
                    new FilePath("somefile.jar"),
                    _globber
                ).Prepare().Render();

                arguments.Should().Contain("--contexts=\"production\"");
            }
            
            [Fact]
            public void Contains_All_Contexts_Quoted_And_Comma_Separated()
            {
                var settings = new LiquibaseSettings();
                settings.Contexts.Add("production");
                settings.Contexts.Add("test");

                var arguments = new ArgumentBuilder(
                    Commands.Update, 
                    settings, 
                    new FilePath("somefile.jar"),
                    _globber
                ).Prepare().Render();

                arguments.Should().Contain("--contexts=\"production,test\"");
            }

            [Fact]
            public void Contains_The_Quoted_DefaultSchemaName()
            {
                var settings = new LiquibaseSettings(){
                    DefaultSchemaName = "dbo"
                };

                var arguments = new ArgumentBuilder(
                    Commands.Update, 
                    settings, 
                    new FilePath("somefile.jar"),
                    _globber
                ).Prepare().Render();

                arguments.Should().Contain("--defaultSchemaName=\"dbo\"");
            }

            [Fact]
            public void Contains_The_Quoted_DefaultsFile()
            {
                var settings = new LiquibaseSettings(){
                    DefaultsFile = "./defaults.properties"
                };

                var arguments = new ArgumentBuilder(
                    Commands.Update, 
                    settings, 
                    new FilePath("somefile.jar"),
                    _globber
                ).Prepare().Render();

                arguments.Should().Contain("--defaultsFile=\"./defaults.properties\"");
            }

            [Fact]
            public void Does_Not_Contain_Parameters_If_The_Setting_Is_Empty()
            {
                var settings = new LiquibaseSettings();

                var arguments = new ArgumentBuilder(
                    Commands.Update, 
                    settings, 
                    new FilePath("somefile.jar"),
                    _globber
                ).Prepare().Render();

                arguments.Should().NotContain("--changeLogFile");
                arguments.Should().NotContain("--username");
                arguments.Should().NotContain("--password");
                arguments.Should().NotContain("--url");
                arguments.Should().NotContain("--driver");
                arguments.Should().NotContain("--contexts");
                arguments.Should().NotContain("--defaultSchemaName");
                arguments.Should().NotContain("--defaultsFile");
            }

        }
    }
}