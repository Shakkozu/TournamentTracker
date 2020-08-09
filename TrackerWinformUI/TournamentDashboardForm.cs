using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrackerLibrary.DataAccess;
using TrackerLibrary.Models;

namespace TrackerWinformUI
{
    public partial class TournamentDashboardForm : Form, ITournamentRequester
    {
        List<TournamentModel> availableTournaments = GlobalConfig.Connection.GetTournament_All();
        public TournamentDashboardForm()
        {
            InitializeComponent();
            WireUpLists();
        }
        private void WireUpLists()
        {
            loadExistingTournamentDropDown.DataSource = null;


            loadExistingTournamentDropDown.DataSource = availableTournaments;
            loadExistingTournamentDropDown.DisplayMember = "TournamentName";


        }

        private void createTournamentButton_Click(object sender, EventArgs e)
        {
            CreateTournamentForm frm = new CreateTournamentForm(this);
            frm.Show();
        }

        public void TournamentComplete(TournamentModel model)
        {
            availableTournaments.Add(model);
            WireUpLists();
        }

        private void loadTournamentButton_Click(object sender, EventArgs e)
        {
            TournamentModel tm = (TournamentModel)loadExistingTournamentDropDown.SelectedItem;
            TournamentViewer frm = new TournamentViewer(tm);
            frm.Show();
        }
    }
}
