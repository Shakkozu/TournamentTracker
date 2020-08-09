using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrackerLibrary.Models;

namespace TrackerWinformUI
{
    public partial class TournamentViewer : Form
    {
        private TournamentModel tournament;
        BindingList<int> rounds = new BindingList<int>();
        BindingList<MatchupModel> selectedMatchups = new BindingList<MatchupModel>();



        public TournamentViewer(TournamentModel tournamentModel)
        {
            InitializeComponent();

            tournament = tournamentModel;

            WireUpLists();
            LoadFormData();
            LoadRounds();
        }

        private void LoadFormData()
        {
            tournamentNameLabel.Text = tournament.TournamentName;

        }

        private void LoadRounds()
        {
            rounds.Clear();
            rounds.Add(1);
            int currRound = 1;
            foreach (List<MatchupModel> round in tournament.Rounds)
            {
                if(round.First().MatchupRound > currRound)
                {
                    currRound = round.First().MatchupRound;
                    rounds.Add(currRound);
                }

            }
            LoadMatchups(1);

        }

        private void WireUpLists()
        {
           roundDropDown.DataSource = rounds;
            matchupListBox.DataSource = selectedMatchups;
            matchupListBox.DisplayMember = "DisplayName";

        }


        private void roundDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMatchups((int)roundDropDown.SelectedValue);
        }

        private void LoadMatchups(int round)
        {
            if (roundDropDown.SelectedValue != null)
            {

                int selectedRound = (int)roundDropDown.SelectedValue;

                foreach (List<MatchupModel> matchups in tournament.Rounds)
                {
                    if (matchups.First().MatchupRound == selectedRound)
                    {
                        selectedMatchups.Clear();
                        foreach (MatchupModel matchup in matchups)
                        {
                            selectedMatchups.Add(matchup);
                        }

                    }
                }

            }
            LoadMatchup(selectedMatchups.First());

        }

        private void LoadMatchup(MatchupModel m)
        {
            if (matchupListBox.SelectedItem != null)
            {

                for (int i = 0; i < m.Entries.Count; i++)
                {
                    if (i == 0)
                    {

                        if (m.Entries[0].TeamCompeting != null)
                        {
                            teamOneNameLabel.Text = m.Entries[0].TeamCompeting.TeamName;
                            teamOneScoreTextBox.Text = m.Entries[0].Score.ToString();

                            teamTwoNameLabel.Text = "<bye>";
                            teamTwoScoreTextBox.Text = "";
                        }
                        else
                        {
                            teamOneNameLabel.Text = "Not Yet Set";
                            teamOneScoreTextBox.Text = "";
                        }
                    }
                    else
                    {
                        if (m.Entries[1].TeamCompeting != null)
                        {
                            teamTwoNameLabel.Text = m.Entries[1].TeamCompeting.TeamName;
                            teamTwoScoreTextBox.Text = m.Entries[1].Score.ToString();
                        }
                        else
                        {
                            teamTwoNameLabel.Text = "Not Yet Set";
                            teamTwoScoreTextBox.Text = "";
                        }
                    }
                }
            }
        }
        private void matchupListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            LoadMatchup((MatchupModel)matchupListBox.SelectedItem);
        }
    }
}
