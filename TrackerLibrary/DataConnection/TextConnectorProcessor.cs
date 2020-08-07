using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;

// *Load the text File
// *Convert the text to List<PrizeModel>
// Find the max ID
// Add the new record with the new ID (max+1)
// Convert the prizes to list<string>
// Save list<string> to the textfile
namespace TrackerLibrary.DataConnection.TextHelpers
{
    public static class TextConnectorProcessor
    {
        //EXTENSTION METHOD (THIS IN PARAM)
        public static string FullFilePath(this string filename) //PrizeModels.csv
        {
            //C:\Users\user\Data\TournamentTracker\filename
            return $"{ ConfigurationManager.AppSettings["filePath"] }\\{ filename }";
        }

        /// <summary>
        /// Load all text from file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static List<string> LoadFile(this string file)
        {
            if(!File.Exists(file))
            {
                return new List<string>();
            }

            return File.ReadAllLines(file).ToList();
        }

        public static List<PrizeModel> ConvertToPrizeModel(this List<string> lines)
        {
            List<PrizeModel> output = new List<PrizeModel>();

            foreach(string line in lines)
            {
                string[] cols = line.Split(',');
                PrizeModel p = new PrizeModel();

                p.Id = int.Parse(cols[0]);
                p.PlaceNumber = int.Parse(cols[1]);
                p.PlaceName = cols[2];
                p.PrizeAmount = decimal.Parse(cols[3]);
                p.PrizePercentage = double.Parse(cols[4]);

                output.Add(p);
            }

            return output;
        }

        public static List<TeamModel> ConvertToTeamModel(this List<string> lines, string peopleFileName)
        {
            //id,team name, list of ids separated by the pipe
            //3,Tim's Team,1|3|5
            List<TeamModel> output = new List<TeamModel>();
            List<PersonModel> people = peopleFileName.FullFilePath().LoadFile().ConvertToPersonModel();

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');

                TeamModel team = new TeamModel();

                team.Id = int.Parse(cols[0]);
                team.TeamName = cols[1];
                
                //Get all people ids
                string[] peopleIds = cols[2].Split('|');

                //Add team members with known id's
                foreach (string id in peopleIds)
                {
                    team.TeamMembers.Add(people.Where(x => x.Id == int.Parse(id)).First());
                }

                output.Add(team);
            }
            return output;
        }

        public static List<PersonModel> ConvertToPersonModel(this List<string> lines)
        {
            List<PersonModel> output = new List<PersonModel>();

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');
                PersonModel p = new PersonModel();

                p.Id = int.Parse(cols[0]);
                p.FirstName = cols[1];
                p.LastName = cols[2];
                p.EmailAddress = cols[3];
                p.CellphoneNumber = cols[4];
                output.Add(p);
            }

            return output;
        }

        public static List<TournamentModel> ConvertToTournamentModel(this List<string> lines,string teamsFileName, string peopleFileName,string prizesFile)
        {
            // id = 0
            // TournamentName = 1
            // EntryFee = 2
            // Prizes = 3
            // Rounds = 4
            // id,TournamentName,EntryFee,(id|id|id - EnteredTeams),(id|id|id - Prizes),(Rounds - id^id^id|id^id^id|id^id^id)

            List<TournamentModel> output = new List<TournamentModel>();
            List<TeamModel> teams = teamsFileName.FullFilePath().LoadFile().ConvertToTeamModel(peopleFileName);
            List<PrizeModel> prizes = prizesFile.FullFilePath().LoadFile().ConvertToPrizeModel();

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');
                TournamentModel p = new TournamentModel();

                p.Id = int.Parse(cols[0]);
                p.TournamentName = cols[1];
                p.EntryFee = decimal.Parse(cols[2]);

                //Add entered teams to tournament model
                string[] teamIds = cols[3].Split('|');
                foreach (string id in teamIds)
                {
                    p.EnteredTeams.Add(teams.Where(x => x.Id == int.Parse(id)).First());
                }

                //Add prizes to tournament model
                string[] prizeIds = cols[4].Split('|');
                foreach (var item in prizes)
                {
                    p.Prizes.Add(prizes.Where(x => x.Id == item.Id).First());
                }

                //Add Rounds to tournament model
                //TODO Capture Rounds information

                output.Add(p);
            }

            return output;
        }


        public static void SaveToPrizeFile(this List<PrizeModel> models,string filename)
        {
            List<string> lines = new List<string>();

            foreach (PrizeModel p in models)
            {
                lines.Add($"{ p.Id },{ p.PlaceNumber },{ p.PlaceName },{ p.PrizeAmount },{ p.PrizePercentage }");
            }
            File.WriteAllLines(filename.FullFilePath(), lines);
        }

        public static void SaveToPeopleFile(this List<PersonModel> models, string filename)
        {
            List<string> lines = new List<string>();

            foreach (PersonModel p in models)
            {
                lines.Add($"{ p.Id },{ p.FirstName },{ p.LastName },{ p.EmailAddress },{ p.CellphoneNumber }");
            }
            File.WriteAllLines(filename.FullFilePath(), lines);
        }

        public static void SaveToTeamFile(this List<TeamModel> models, string filename)
        {
            List<string> lines = new List<string>();

            foreach (TeamModel t in models)
            {
                lines.Add($"{ t.Id },{ t.TeamName },{ ConvertPeopleListToString(t.TeamMembers) }");
            }
            File.WriteAllLines(filename.FullFilePath(), lines);
        }

       public static void  SaveToTournamentFile(this List<TournamentModel> models, string fileName)
       {
            List<string> lines = new List<string>();

            // id = 0
            // TournamentName = 1
            // EntryFee = 2
            // Prizes = 3
            // Rounds = 4
            // id,TournamentName,EntryFee,(id|id|id - EnteredTeams),(id|id|id - Prizes),(Rounds - id^id^id|id^id^id|id^id^id)


            foreach (TournamentModel tm in models)
            {
                lines.Add($"" +
                    $"{ tm.Id }," +
                    $"{ tm.TournamentName }," +
                    $"{ tm.EntryFee }," +
                    $"{ ConvertTeamListToString(tm.EnteredTeams) }," +
                    $"{ ConvertPrizeListToString(tm.Prizes) }," +
                    $"{ ConvertRoundsListToString(tm.Rounds) }");
            }
            File.WriteAllLines(fileName.FullFilePath(), lines);
        }

        private static string ConvertTeamListToString(List<TeamModel> teams)
        {
            string output = "";
            //2|5

            if (teams.Count == 0)
            {
                return "";
            }

            foreach (TeamModel t in teams)
            {
                output += $"{ t.Id }|";
            }

            output = output.Substring(0, output.Length - 1);

            return output;
        }

        private static string ConvertPrizeListToString(List<PrizeModel> prizes)
        {
            string output = "";
            //2|5

            if (prizes.Count == 0)
            {
                return "";
            }

            foreach (PrizeModel p in prizes)
            {
                output += $"{ p.Id }|";
            }

            output = output.Substring(0, output.Length - 1);

            return output;
        }

        private static string ConvertPeopleListToString(List<PersonModel> people)
        {
            string output = "";
            //2|5

            if(people.Count == 0)
            {
                return "";
            }

            foreach (PersonModel person in people)
            {
                output += $"{ person.Id }|";
            }
            
            output = output.Substring(0, output.Length - 1);
            

            return output;
        }

        private static string ConvertRoundsListToString(List<List<MatchupModel>> rounds)
        {
            string output = "";
            //2|5

            if (rounds.Count == 0)
            {
                return "";
            }

            foreach (List<MatchupModel> r in rounds)
            {
                output += $"{ ConvertMatchupListToString(r) }|";
                

            }

            output = output.Substring(0, output.Length - 1);

            return output;
        }

        private static string ConvertMatchupListToString(List<MatchupModel> matchups)
        {
            string output = "";

            if (matchups.Count == 0)
            {
                return "";
            }

            foreach(MatchupModel m in matchups)
            {
                output += $"{ m.Id }^";
            }
            output = output.Substring(0, output.Length - 1);

            return output;
        }

    }
}












