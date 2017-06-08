using System;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Liquibase.Runner;
using Cake.Liquibase.Runner.LiquibaseCommands;

namespace Cake.Liquibase
{
    
    /// <summary>
    /// Contains functionality for executing liquibase.
    /// </summary>
    [CakeAliasCategory("Liquibase")]
    public static class LiquibaseAliases
    {
        /// <summary>
        /// Executes liquibase using the 'update' cli parameter and the information in the liquibase settings.
        /// </summary>
        /// <returns>Liquibase return code</returns>
        [CakeMethodAlias]
        public static int UpdateDatabase(this ICakeContext context, LiquibaseSettings liquibaseSettings)
        {
            return new LiquibaseRunner(context.ProcessRunner, context.Log, context.Tools).Start(Commands.Update, liquibaseSettings);
        }
    }
}
