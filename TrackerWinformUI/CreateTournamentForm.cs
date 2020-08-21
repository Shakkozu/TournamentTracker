using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrackerLibrary;
using TrackerLibrary.DataAccess;
using TrackerLibrary.Models;

namespace TrackerWinformUI
{
    public partial class CreateTournamentForm : Form, IPrizeRequester, ITeamRequester
    {
        List<TeamModel> availableTeams = GlobalConfig.Connection.GetTeam_All();
        List<TeamModel> selectedTeams = new List<TeamModel>();

        List<PrizeModel> selectedPrizes = new List<PrizeModel>();

        ITournamentRequester callingForm;

        public CreateTournamentForm(ITournamentRequester caller)
        {
            InitializeComponent();
            WireUpLists();
            callingForm = caller;
        }

  

        private void WireUpLists()
        {
            selectTeamDropDown.DataSource = null;

            selectTeamDropDown.DataSource = availableTeams;
            selectTeamDropDown.DisplayMember = "TeamName";


            tournamentTeamsListBox.DataSource = null;

            tournamentTeamsListBox.DataSource = selectedTeams;
            tournamentTeamsListBox.DisplayMember = "TeamName";


            prizesListBox.DataSource = null;
            prizesListBox.DataSource = selectedPrizes;
            prizesListBox.DisplayMember = "PlaceName";



        }

        private void addTeamButton_Click(object sender, EventArgs e)
        {
            TeamModel team = (TeamModel)selectTeamDropDown.SelectedItem;
            if(team != null)
            {
                selectedTeams.Add(team);
                availableTeams.Remove(team);

                WireUpLists();
            }

        }

        private void removeSelectedTeamButton_Click(object sender, EventArgs e)
        {
            TeamModel team = (TeamModel)tournamentTeamsListBox.SelectedItem;
            if (team != null)
            {

                selectedTeams.Remove(team);
                availableTeams.Add(team);

                WireUpLists();
            }
        }

        private void removeSelectedPrizeButton_Click(object sender, EventArgs e)
        {
            PrizeModel prize = (PrizeModel)prizesListBox.SelectedItem;
            if (prize != null)
            {

                selectedPrizes.Remove(prize);

                WireUpLists();
            }
        }

        private void createPrizeButton_Click(object sender, EventArgs e)
        {
            //Call the create prize form
            CreatePrizeForm frm = new CreatePrizeForm(this);
            frm.Show();
        }            

        public void PrizeComplete(PrizeModel model)
        {
            //Get back from the form a PrizeModel
            //Take the PrizeModel and put it into our list of selected Prizes
            selectedPrizes.Add(model);
            WireUpLists();
        }

        public void TeamComplete(TeamModel model)
        {
            selectedTeams.Add(model);
            WireUpLists();
        }

        private void createNewTeamLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CreateTeamForm frm = new CreateTeamForm(this);
            frm.Show();
        }

        private void createTournamentButton_Click(object sender, EventArgs e)
        {
            //Validate data

            if (ValidateForm())
            {
                //Create our tournament model
                TournamentModel tm = new TournamentModel();
                tm.TournamentName = tournamentNameValue.Text;
                tm.EntryFee = decimal.Parse(entryFeeValue.Text);

                tm.Prizes = selectedPrizes;
                tm.EnteredTeams = selectedTeams;

                // Wire up matchups
                TournamentLogic.CreateRounds(tm);


                //Create tournament entry, using tournamentName, EntryFee,
                //Create all of the prizes entries
                //Create all of the team entries
                GlobalConfig.Connection.CreateTournament(tm);

                tm.AlertUsersToNewRound();

                TournamentLogic.UpdateTournamentResults(tm);

                TournamentViewer frm = new TournamentViewer(tm);
                frm.Show();
                callingForm.TournamentComplete(tm);
                Close();
            }
            

        }
        private bool ValidateForm()
        {
            if (tournamentNameValue.Text.Length == 0)
            {
                MessageBox.Show("You need to enter a Tournament Name.",
                  "Invalid Tournament Name",
                  MessageBoxButtons.OK,
                  MessageBoxIcon.Error);
                return false;
            }

            decimal fee = 0;
            bool feeAcceptable = decimal.TryParse(entryFeeValue.Text, out fee);
            if (!feeAcceptable)
            {
                MessageBox.Show("You need to enter a valid Entry Fee.",
                    "Invalid Fee",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return false;
            }
            if (selectedTeams.Count < 2)
            {
                MessageBox.Show("You need to select Teams participating in tournament.",
                 "Invalid Tournament Teams",
                 MessageBoxButtons.OK,
                 MessageBoxIcon.Error);
                return false;
            }
            if (selectedPrizes.Count < 1)
            {
                MessageBox.Show("You need to Add Prizes to tournament.",
                 "Invalid Tournament Prizes",
                 MessageBoxButtons.OK,
                 MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
    }
}
