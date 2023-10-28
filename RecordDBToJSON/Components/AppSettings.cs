// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppSettings.cs" company="Software Inc.">
//   A.Robson
// </copyright>
// <summary>
//   Defines the AppSettings type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace RecordDBToJSON.Components
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// The app settings class.
    /// </summary>
    public class AppSettings
    {
        #region " Singleton of AppSettings "

        /// <summary>
        /// The App settings instance.
        /// </summary>
        private static AppSettings _Instance = null;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        public static AppSettings Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new AppSettings();
                }

                return _Instance;
            }
        }

        #endregion

        /// <summary>
        /// The get connection string.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The <see cref="string"/>connection string.</returns>
        protected string GetConnectionString(string key)
        {
            return ConfigurationManager.ConnectionStrings[key].ConnectionString;
        }

        /// <summary>
        /// Gets the connection string.
        /// </summary>
        public string ConnectString
        {
            get { return this.GetConnectionString("RecordDB"); }
        }
    }
}
