namespace Cake.Liquibase.Runner
{
    public class LiquibaseSettings
    {
        public LiquibaseSettings()
        {
            LiquibaseJar = "liquibase.jar";
            JavaSettings = new JavaSettings();
        }

        public string LiquibaseJar { get; set; }
        public JavaSettings JavaSettings {get; private set; }
    }
}