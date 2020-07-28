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
        public static List<IDataConnection> Connections { get; private set; } = new List<IDataConnection>();

        /// <summary>
        /// Initializes connection for each of available connections
        /// </summary>
        /// <param name="database">Defines if sql database connection should be established</param>
        /// <param name="textFiles">Defines if textFile connection should be established</param>
        public static void InitializeConnections(bool database, bool textFiles)
        {
            if(database)
            {
                //TODO - Set up the SQL Connector properly
                SQLConnector sql = new SQLConnector();
                Connections.Add(sql);
            }
            if(textFiles)
            {
                TextConnector text = new TextConnector();
                Connections.Add(text);
                //TODO - Create the Text Connection
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
