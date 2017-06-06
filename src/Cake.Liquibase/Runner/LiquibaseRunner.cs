using System;
using Cake.Core;
using Cake.Core.Diagnostics;

namespace Cake.Liquibase.Runner
{
    /// <summary>
    /// Builds up the command line and executes the liquibase jar file.
    /// </summary>
    public class LiquibaseRunner
    {
        public ICakeLog Log { get; private set; }
        public ICakeEnvironment Environment { get; private set; }


        public LiquibaseRunner(ICakeEnvironment env, ICakeLog log)
        {
            if (env == null)
                throw new ArgumentNullException("env");

            if (log == null)
                throw new ArgumentNullException("log");

            this.Environment = env;
            this.Log = log;
                
        }

        
        /// <summary>
        /// Runs liquibase against a database using the update parameter.
        /// </summary>
        /// <returns></returns>
        public int Update(LiquibaseSettings settings)
        {
            

            return null;
        }

    }
}