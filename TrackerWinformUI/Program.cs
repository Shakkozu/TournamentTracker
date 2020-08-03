using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrackerWinformUI
{
    static class Program
    {
        /// <summary>
        /// Główny punkt wejścia dla aplikacji.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //Initialize the database connections
            TrackerLibrary.DataAccess.GlobalConfig.InitializeConnections(TrackerLibrary.DatabaseType.TextFile);
            Application.Run(new CreateTeamForm());

            //Application.Run(new TournamentDashboardForm());

        }
    }
}
