using System;
using System.Collections.Generic;
using System.Text;
using TrackerLibrary.Interfaces;
using TrackerLibrary.Models;
using TrackerLibrary.DataConnection.TextHelpers;
using System.Linq;

namespace TrackerLibrary.DataAccess
{
    public class TextConnector : IDataConnection
    {

        public void CreatePerson(PersonModel model)
        {
            // Load the text File
            // Convert the text to List<PrizeModel>

            List<PersonModel> people = GlobalConfig.PeopleFile.FullFilePath().LoadFile().ConvertToPersonModels();

            // Find the max ID
            int currentId = 1;
            if (people.Count > 0)
                currentId = people.OrderByDescending(x => x.Id).First().Id + 1;

            model.Id = currentId;

            // Add the new record with the new ID (max+1)
            people.Add(model);

            // Convert the prizes to list<string>
            // Save list<string> to the textfile

            people.SaveToPeopleFile();
            //TextConnectorProcessor.SaveToPrizeFile(prizes, GlobalConfig.PrizesFile);

        }

        /// <summary>
        /// Saves a new prize to the text file.
        /// </summary>
        /// <param name="model">The prize information</param>
        /// <returns>The prize information, including the unique identifier</returns>
        public void CreatePrize(PrizeModel model)
        {
            // Load the text File
            // Convert the text to List<PrizeModel>

            List<PrizeModel> prizes = GlobalConfig.PrizesFile.FullFilePath().LoadFile().ConvertToPrizeModels();

            // Find the max ID
            int currentId = 1;
            if(prizes.Count > 0)
                currentId = prizes.OrderByDescending(x => x.Id).First().Id + 1;

            model.Id = currentId;

            // Add the new record with the new ID (max+1)
            prizes.Add(model);

            // Convert the prizes to list<string>
            // Save list<string> to the textfile

            prizes.SaveToPrizeFile();
            //TextConnectorProcessor.SaveToPrizeFile(prizes, GlobalConfig.PrizesFile);


        }

        public void CreateTeam(TeamModel model)
        {
            // Load the text File
            List<TeamModel> teams = GlobalConfig.TeamFile.FullFilePath().LoadFile().ConvertToTeamModels();

            // Find the max ID
            int currentId = 1;
            if (teams.Count > 0)
                currentId = teams.OrderByDescending(x => x.Id).First().Id + 1;

            model.Id = currentId;

            // Add the new record with the new ID (max+1)
            teams.Add(model);

            // Convert the prizes to list<string>
            // Save list<string> to the textfile

            teams.SaveToTeamFile();
            //TextConnectorProcessor.SaveToPrizeFile(prizes, GlobalConfig.PrizesFile);

        }

        public void CreateTournament(TournamentModel model)
        {
            // Load the text File
            List<TournamentModel> tournaments = GlobalConfig.TournamentFile
                .FullFilePath()
                .LoadFile()
                .ConvertToTournamentModels();

            // Find the max ID
            int currentId = 1;
            if (tournaments.Count > 0)
                currentId = tournaments.OrderByDescending(x => x.Id).First().Id + 1;
            
            model.Id = currentId;

            //Save rounds
            model.SaveRoundsToFile();


            // Add the new record with the new ID (max+1)
            tournaments.Add(model);


            tournaments.SaveToTournamentFile();
            //TextConnectorProcessor.SaveToPrizeFile(prizes, GlobalConfig.PrizesFile);

        }

        public List<PersonModel> GetPerson_All()
        {
            List<PersonModel> output = GlobalConfig.PeopleFile.FullFilePath().LoadFile().ConvertToPersonModels();
            return output;
        }

        public List<TeamModel> GetTeam_All()
        {
            List<TeamModel> teams = GlobalConfig.TeamFile.FullFilePath().LoadFile().ConvertToTeamModels();
            return teams;
        }

        public List<TournamentModel> GetTournament_All()
        {
            List<TournamentModel> tournaments = GlobalConfig.TournamentFile
                .FullFilePath()
                .LoadFile()
                .ConvertToTournamentModels();
            return tournaments;
        }

        public void UpdateMatchup(MatchupModel m)
        {
            m.UpdateMatchupToFile();
        }
    }
}
