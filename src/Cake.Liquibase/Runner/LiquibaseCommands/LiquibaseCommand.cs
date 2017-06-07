namespace Cake.Liquibase.Runner.LiquibaseCommands
{
    public abstract class LiquibaseCommand
    {
        public static LiquibaseCommand Update => new LiquibaseUpdateCommand();

        public override abstract string ToString();
    }
}