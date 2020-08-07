using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                    if(currMatchup.Entries.Count > 1)
                    {
                        currMatchup.MatchupRound = round;
                        currRound.Add(currMatchup);
                        currMatchup = new MatchupModel();
                    }
                }
                model.Rounds.Add(currRound);
                previousRound = currRound;

                round++;
                currRound = new List<MatchupModel>();
            }
        }

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

        private static List<TeamModel> RandomizeTeamOrder(List<TeamModel> teams)
        {
            teams.OrderBy(x => Guid.NewGuid()).ToList();
            return teams;
        }

    }
}
