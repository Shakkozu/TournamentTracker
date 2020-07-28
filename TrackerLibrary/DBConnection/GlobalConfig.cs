using System;
using System.Collections.Generic;
using System.Text;
using TrackerLibrary.Interfaces;

namespace TrackerLibrary.DBConnection
{
    public static class GlobalConfig
    {
        public static List<IDataConnection> Connections { get; private set; } = new List<IDataConnection>();

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

    }
}
