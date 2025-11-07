namespace Cake.Liquibase.Runner.LiquibaseCommands
{
    public static class Commands
    {
        public static BaseCommand Update => new LiquibaseUpdateCommand();
        public static BaseCommand Validate => new LiquibaseValidateCommand();
    }
}