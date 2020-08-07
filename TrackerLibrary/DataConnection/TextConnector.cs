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

        private const string PrizesFile = "PrizeModels.csv";
        private const string PeopleFile = "PeopleModels.csv";

        private const string TeamsFile = "TeamModels.csv";

        private const string TournamentFile = "TournamentModels.csv";

        public PersonModel CreatePerson(PersonModel model)
        {
            // Load the text File
            // Convert the text to List<PrizeModel>

            List<PersonModel> people = PeopleFile.FullFilePath().LoadFile().ConvertToPersonModel();

            // Find the max ID
            int currentId = 1;
            if (people.Count > 0)
                currentId = people.OrderByDescending(x => x.Id).First().Id + 1;

            model.Id = currentId;

            // Add the new record with the new ID (max+1)
            people.Add(model);

            // Convert the prizes to list<string>
            // Save list<string> to the textfile

            people.SaveToPeopleFile(PeopleFile);
            //TextConnectorProcessor.SaveToPrizeFile(prizes, PrizesFile);

            return model;
        }

        //TODO - Wire up the CreatePrize for text files
        /// <summary>
        /// Saves a new prize to the text file.
        /// </summary>
        /// <param name="model">The prize information</param>
        /// <returns>The prize information, including the unique identifier</returns>
        public PrizeModel CreatePrize(PrizeModel model)
        {
            // Load the text File
            // Convert the text to List<PrizeModel>

            List<PrizeModel> prizes = PrizesFile.FullFilePath().LoadFile().ConvertToPrizeModel();

            // Find the max ID
            int currentId = 1;
            if(prizes.Count > 0)
                currentId = prizes.OrderByDescending(x => x.Id).First().Id + 1;

            model.Id = currentId;

            // Add the new record with the new ID (max+1)
            prizes.Add(model);

            // Convert the prizes to list<string>
            // Save list<string> to the textfile

            prizes.SaveToPrizeFile(PrizesFile);
            //TextConnectorProcessor.SaveToPrizeFile(prizes, PrizesFile);

            return model;
        }

        public TeamModel CreateTeam(TeamModel model)
        {
            // Load the text File
            List<TeamModel> teams = TeamsFile.FullFilePath().LoadFile().ConvertToTeamModel(PeopleFile);

            // Find the max ID
            int currentId = 1;
            if (teams.Count > 0)
                currentId = teams.OrderByDescending(x => x.Id).First().Id + 1;

            model.Id = currentId;

            // Add the new record with the new ID (max+1)
            teams.Add(model);

            // Convert the prizes to list<string>
            // Save list<string> to the textfile

            teams.SaveToTeamFile(TeamsFile);
            //TextConnectorProcessor.SaveToPrizeFile(prizes, PrizesFile);

            return model;
        }

        public void CreateTournament(TournamentModel model)
        {
            // Load the text File
            // Convert the text to List<PrizeModel>

            List<TournamentModel> tournaments = TournamentFile
                .FullFilePath()
                .LoadFile()
                .ConvertToTournamentModel(TeamsFile,PeopleFile,PrizesFile);

            // Find the max ID
            int currentId = 1;
            if (tournaments.Count > 0)
                currentId = tournaments.OrderByDescending(x => x.Id).First().Id + 1;

            model.Id = currentId;

            // Add the new record with the new ID (max+1)
            tournaments.Add(model);

            // Convert the prizes to list<string>
            // Save list<string> to the textfile

            tournaments.SaveToTournamentFile(TournamentFile);
            //TextConnectorProcessor.SaveToPrizeFile(prizes, PrizesFile);

        }

        public List<PersonModel> GetPerson_All()
        {
            List<PersonModel> output = PeopleFile.FullFilePath().LoadFile().ConvertToPersonModel();
            return output;
        }

        public List<TeamModel> GetTeam_All()
        {
            List<TeamModel> teams = TeamsFile.FullFilePath().LoadFile().ConvertToTeamModel(PeopleFile);
            return teams;
        }
    }
}
