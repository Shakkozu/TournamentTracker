using System;
using System.Collections.Generic;
using System.Text;
using TrackerLibrary.Interfaces;
using System.Configuration;

namespace TrackerLibrary.DataAccess
{
    public static class GlobalConfig
    {
        /// <summary>
        /// List of connections
        /// </summary>
        public static IDataConnection Connection { get; private set; }


        /// <summary>
        /// Initializes connection for each of available connections
        /// </summary>
        /// <param name="database">Defines if sql database connection should be established</param>
        /// <param name="textFiles">Defines if textFile connection should be established</param>
        public static void InitializeConnections(DatabaseType db)
        {
            switch (db)
            {
                case DatabaseType.Sql:
                    //TODO - Set up the SQL Connector properly
                    SQLConnector sql = new SQLConnector();
                    Connection = sql;
                    break;
                case DatabaseType.TextFile:
                    //TODO - Create the Text Connection
                    TextConnector text = new TextConnector();
                    Connection = text;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Returns ConnectionString located in App.config file, within TrackerWinformUI project
        /// </summary>
        /// <param name="name">Name of connection string that is accessed</param>
        /// <returns>Connection String with specified name in @name parameter</returns>
        public static string CnnString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

    }
}
