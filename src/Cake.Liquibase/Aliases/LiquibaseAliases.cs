﻿using System;
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
        public static int UpdateDatabase(this ICakeContext context, LiquibaseSettings liquibaseSettings)
        {
            var result =  new LiquibaseRunner(context.ProcessRunner, context.Log, context.Tools, context.Globber, context.Environment.Platform)
                .Start(Commands.Update, liquibaseSettings);

            if (result != 0)
            {
                throw new InvalidOperationException("Error running liquibase");
            }
            return result;
        }

        /// <summary>
        /// Executes liquibase using the 'update' cli parameter and default liquibase settings that can be overwritten by using the action.
        /// </summary>
        /// <param name="liquibaseSettingsAction">An action that lets you modify the default liquibase settings.</param>
        /// <returns>Liquibase return code</returns>
        [CakeMethodAlias]
        public static int UpdateDatabase(this ICakeContext context, Action<LiquibaseSettings> liquibaseSettingsAction)
        {
            var liquibaseSettings = new LiquibaseSettings();

            if (liquibaseSettingsAction != null) 
            {
                liquibaseSettingsAction(liquibaseSettings);
            }

            return UpdateDatabase(context, liquibaseSettings);
        }
    }
}
