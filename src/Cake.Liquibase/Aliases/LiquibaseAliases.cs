using System;
using Cake.Common.IO;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.Diagnostics;
using Cake.Liquibase;
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
        /// <param name="liquibaseSettings">The liquibase settings and arguments for running liquibase.</param>
        /// <returns>Liquibase return code</returns>
        [CakeMethodAlias]
        public static void UpdateDatabase(this ICakeContext context, LiquibaseSettings liquibaseSettings)
        {
            new LiquibaseTool(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools, context.Globber, context.Log)
                .Start(Commands.Update, liquibaseSettings);
        }

        /// <summary>
        /// Executes liquibase using the 'update' cli parameter and default liquibase settings that can be overwritten by using the action.
        /// </summary>
        /// <param name="liquibaseSettingsAction">An action that lets you modify the default liquibase settings.</param>
        /// <returns>Liquibase return code</returns>
        [CakeMethodAlias]
        public static void UpdateDatabase(this ICakeContext context, Action<LiquibaseSettings> liquibaseSettingsAction)
        {
            var liquibaseSettings = new LiquibaseSettings();
            liquibaseSettingsAction?.Invoke(liquibaseSettings);

            UpdateDatabase(context, liquibaseSettings);
        }


        /// <summary>
        /// Executes liquibase using the 'validate' cli parameter and the information in the liquibase settings.
        /// </summary>
        /// <param name="liquibaseSettings">The liquibase settings and arguments for running liquibase.</param>
        /// <returns>Liquibase return code</returns>
        [CakeMethodAlias]
        public static void ValidateChangeLog(this ICakeContext context, LiquibaseSettings liquibaseSettings)
        {
            new LiquibaseTool(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools, context.Globber, context.Log)
                .Start(Commands.Validate, liquibaseSettings);
        }

        /// <summary>
        /// Executes liquibase using the 'validate' cli parameter and default liquibase settings that can be overwritten by using the action.
        /// </summary>
        /// <param name="liquibaseSettingsAction">An action that lets you modify the default liquibase settings.</param>
        /// <returns>Liquibase return code</returns>
        [CakeMethodAlias]
        public static void ValidateChangeLog(this ICakeContext context, Action<LiquibaseSettings> liquibaseSettingsAction)
        {
            var liquibaseSettings = new LiquibaseSettings();
            liquibaseSettingsAction?.Invoke(liquibaseSettings);

            ValidateChangeLog(context, liquibaseSettings);
        }
    }
}
