using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.DataAccess;
using TrackerLibrary.Models;

namespace TrackerLibrary
{
    public static class TournamentLogic
    {
        // Order our list randomly
        // Check if it is big enough - if not, add in byes (team competing with bye doesn't have to participate in current round
        // and will be moved to next one)
        // Create our first round of matchups
        // Create every round after that - 8 matchups - 4 matchups - 2 matchups - 1 matchup

        public static void CreateRounds(TournamentModel model)
        {
            List<TeamModel> randomizedTeams = RandomizeTeamOrder(model.EnteredTeams);
            int rounds = FindNumberOfRounds(model.EnteredTeams.Count);
            int byes = NumberOfByes(rounds, randomizedTeams.Count);

            model.Rounds.Add(CreateFirstRound(byes, randomizedTeams));

            CreateOtherRounds(model, rounds);


        } 

        public static void UpdateTournamentResults(TournamentModel model)
        {
            List<MatchupModel> toScore = new List<MatchupModel>();

            foreach (List<MatchupModel> round in model.Rounds)
            {
                foreach (MatchupModel matchup in round)
                {
                    if (matchup.Winner == null && ( matchup.Entries.Any(x => x.Score != 0) || matchup.Entries.Count == 1))
                    {
                        toScore.Add(matchup);
                    }

                }
            }

            MarkWinnersInMatchup(toScore);

            AdvanceWinners(toScore,model);

            toScore.ForEach(x => GlobalConfig.Connection.UpdateMatchup(x));
        }

        private static void AdvanceWinners(List<MatchupModel> models, TournamentModel tournament)
        {
            foreach (MatchupModel matchup in models)
            {
                foreach (List<MatchupModel> round in tournament.Rounds)
                {
                    foreach (MatchupModel rm in round)
                    {
                        foreach (MatchupEntryModel matchupEntry in rm.Entries)
                        {
                            if (matchupEntry.ParentMatchup != null)
                            {
                                if (matchupEntry.ParentMatchup.Id == matchup.Id)
                                {
                                    matchupEntry.TeamCompeting = matchup.Winner;
                                    GlobalConfig.Connection.UpdateMatchup(rm);
                                }
                            }
                        }
                    }
                }
            }

        }

        private static void MarkWinnersInMatchup(List<MatchupModel> models)
        {
            //greater or lesser
            string greaterWins = ConfigurationManager.AppSettings["greaterWins"];
            


            foreach (MatchupModel matchup in models)
            {
                // Checks for bye week entry
                if (matchup.Entries.Count == 1)
                {
                    matchup.Winner = matchup.Entries[0].TeamCompeting;
                    continue;
                }

                //0 means false, or lower score wins
                if (greaterWins == "0")
                {
                    if(matchup.Entries[0].Score < matchup.Entries[1].Score)
                    {
                        matchup.Winner = matchup.Entries[0].TeamCompeting;
                    }
                    else if(matchup.Entries[1].Score < matchup.Entries[0].Score)
                    {
                        matchup.Winner = matchup.Entries[1].TeamCompeting;
                    }
                    else
                    {
                        throw new InvalidOperationException("We do not allow ties in this application.");
                    }
                }

                // 1 mean true, or high score wins
                else
                {
                    if (matchup.Entries[0].Score > matchup.Entries[1].Score)
                    {
                        matchup.Winner = matchup.Entries[0].TeamCompeting;
                    }
                    else if (matchup.Entries[1].Score > matchup.Entries[0].Score)
                    {
                        matchup.Winner = matchup.Entries[1].TeamCompeting;
                    }
                    else
                    {
                        throw new InvalidOperationException("We do not allow ties in this application.");
                    }
                }
                
            }


            //if (teamOneScore > teamTwoScore)
            //{
            //    //Team one wins
            //    m.Winner = m.Entries[0].TeamCompeting;
            //}
            //else if (teamTwoScore > teamOneScore)
            //{
            //    //Team two wins
            //    m.Winner = m.Entries[1].TeamCompeting;
            //}
            //else
            //{
            //    MessageBox.Show("I do not handle tie games.");
            //}

            //
            //{

        }

        /// <summary>
        /// Function creates rounds of the tournament that have to be played
        /// </summary>
        private static void CreateOtherRounds(TournamentModel model, int rounds)
        {

            //Number of current round
            int round = 2;

            List<MatchupModel> previousRound = model.Rounds[0];
            List<MatchupModel> currRound = new List<MatchupModel>();
            MatchupModel currMatchup = new MatchupModel();

            while (round <= rounds)
            {
                foreach (MatchupModel matchup in previousRound)
                {

                    currMatchup.Entries.Add(new MatchupEntryModel { ParentMatchup = matchup });
                    //If there are 2 teams in MatchupEntry, reset Add it to current Round list, and reset current Matchup
                    if(currMatchup.Entries.Count > 1)
                    {
                        currMatchup.MatchupRound = round;
                        currRound.Add(currMatchup);
                        currMatchup = new MatchupModel();
                    }
                }
                // When adding entries for current round is done, add this round to list of rounds
                // and head to creating next one, after reseting current Round variable
                model.Rounds.Add(currRound);
                previousRound = currRound;

                round++;
                currRound = new List<MatchupModel>();
            }
        }

        /// <summary>
        /// Function creates first round of tournament
        /// </summary>
        private static List<MatchupModel> CreateFirstRound(int byes, List<TeamModel> teams)
        {
            List<MatchupModel> output = new List<MatchupModel>();

            MatchupModel curr = new MatchupModel();

            foreach (TeamModel team in teams)
            {
                curr.Entries.Add(new MatchupEntryModel{ TeamCompeting = team });
                if(byes > 0 || curr.Entries.Count > 1)
                {
                    //set round number
                    curr.MatchupRound = 1;

                    // if byes ended this matchup, decrease it's value
                    if(byes > 0)
                    {
                        byes--;
                    }
                    //add new Matchup Entry to output 
                    output.Add(curr);
                    //Reset Matchup
                    curr = new MatchupModel();
                }
            }


            return output;
        }

        /// <summary>
        /// Function finds number of 'byes' which will cover first round up to n^2 matchups
        /// </summary>
        private static int NumberOfByes(int rounds, int numberOfTeams)
        {
            int output = 0;

            int totalTeams = 1;

            for (int i = 1; i <= rounds ; i++)
            {
                totalTeams *= 2;
            }

            output = totalTeams - numberOfTeams;


            return output;
        }

        /// <summary>
        /// Function finds needed number of rounds to create valid tournament
        /// </summary>
        private static int FindNumberOfRounds(int teamCount)
        {
            int output = 1;
            int val = 2;

            while (val < teamCount)
            {
                output++;
                val *= 2;
            }
            return output;
        }

        /// <summary>
        /// Randomizes teams order passed in parameter
        /// </summary>
        /// <returns></returns>
        private static List<TeamModel> RandomizeTeamOrder(List<TeamModel> teams)
        {
            teams.OrderBy(x => Guid.NewGuid()).ToList();
            return teams;
        }


    }
}
