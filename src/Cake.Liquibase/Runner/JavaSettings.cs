using System.Collections.Generic;

namespace Cake.Liquibase.Runner
{
    public class JavaSettings
    {
        private string _options;
        private string _executable;

        public JavaSettings()
        {
            Classpaths = new List<string>();
            Classpaths.Add("."); // default class path as first item
        }

        /// <summary>
        /// Defines where the java executable can be found. Defaults to java.exe (should be found via PATH variable).
        /// </summary>
        public string Executable
        {
            get{
                if (string.IsNullOrWhiteSpace(_executable))
                {
                    _executable = "java.exe";
                }
                return _executable;
            }
            set
            {
                _executable = value;
            }
        }

        /// <summary>
        /// Class path that should be added to the liquibase call. Defaults to ".".
        /// </summary>
        /// <returns></returns>
        public IList<string> Classpaths {get; private set;}
        

        /// <summary>
        /// Defines what additional java options should be set (e.g. classpath or similar)
        /// </summary>
        public string Options
        {
            get {
                if (string.IsNullOrWhiteSpace(_options))
                {
                    _options = "";
                }
                return _options;
            }
            set {
                _options = value;
            }
        }

    }
}