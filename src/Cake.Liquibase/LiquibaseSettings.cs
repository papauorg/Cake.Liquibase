using Cake.Core.Tooling;
using System.Collections.Generic;

namespace Cake.Liquibase
{
	public class LiquibaseSettings : ToolSettings
	{
		public LiquibaseSettings() : base()
		{
			LiquibaseJar = "./tools/**/liquibase*.jar";
			JavaSettings = new JavaSettings();
			Contexts = new List<string>();
		}

		/// <summary>
		/// Defines where to look for the liquibase jar file.
		/// </summary>
		/// <returns></returns>
		public string LiquibaseJar { get; set; }

		/// <summary>
		/// Defines additional JavaSettings for the jvm that are included in the command line
		/// </summary>
		/// <returns></returns>
		public JavaSettings JavaSettings { get; private set; }

		/// <summary>
		/// Changelog file that should be used
		/// </summary>
		/// <returns></returns>
		public string ChangeLogFile { get; set; }

		/// <summary>
		/// Username for authenticating against the database
		/// </summary>
		/// <returns></returns>
		public string Username { get; set; }

		/// <summary>
		/// Password to use for authenticating against the database
		/// </summary>
		/// <returns></returns>
		public string Password { get; set; }

		/// <summary>
		/// JDBC Url
		/// </summary>
		/// <returns></returns>
		public string Url { get; set; }

		/// <summary>
		/// Defines the driver that liquibase should use
		/// </summary>
		/// <returns></returns>
		public string DriverClassName { get; set; }

		/// <summary>
		/// Defines the defaults properties file liquibase should use
		/// </summary>
		/// <returns></returns>
		public string DefaultsFile { get; set; }

		/// <summary>
		/// Defines the default schema liquibase should use
		/// </summary>
		/// <returns></returns>
		public string DefaultSchemaName { get; set; }

		/// <summary>
		/// Liquibase contexts
		/// </summary>
		/// <returns></returns>
		public List<string> Contexts { get; private set; }
	}
}