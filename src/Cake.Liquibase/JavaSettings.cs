using System.Collections.Generic;

namespace Cake.Liquibase
{
    public class JavaSettings
    {
        private string _options;

        public JavaSettings()
        {
            Classpaths = new List<string>();
            Classpaths.Add("."); // default class path as first item
        }

        /// <summary>
        /// Defines where the java executable can be found. 
        /// </summary>
        public string Executable {get; set;}

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