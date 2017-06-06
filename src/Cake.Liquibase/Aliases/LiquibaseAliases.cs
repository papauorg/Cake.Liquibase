using System;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Liquibase.Runner;

namespace Cake.Liquibase
{
    
    /// <summary>
    /// Contains functionality for executing liquibase.
    /// </summary>
    [CakeAliasCategory("Liquibase")]
    public static class LiquibaseAliases
    {
        /// <summary>
        /// Executes liquibase using the 'update' cli parameter and the information in the liquibase options.
        /// </summary>
        /// <returns>Liquibase return code</returns>
        [CakeMethodAlias]
        public static int UpdateDatabase(this ICakeContext context, LiquibaseSettings liquibaseOptions)
        {
            return new LiquibaseRunner(context.Environment, context.Log).Update(liquibaseOptions);
        }
    }
}
