using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using TrackerLibrary.Interfaces;
using TrackerLibrary.Models;

namespace TrackerLibrary.DataAccess
{
    public class SQLConnector : IDataConnection
    {
        private const string db = "Tournaments";

        public PersonModel CreatePerson(PersonModel person)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(db)))
            {
                var p = new DynamicParameters();
                p.Add("@FirstName",person.FirstName);
                p.Add("@LastName",person.LastName);
                p.Add("@EmailAddress",person.EmailAddress);
                p.Add("@CellphoneNumber",person.CellphoneNumber);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("dbo.spPeople_Insert", p, commandType: CommandType.StoredProcedure);
                person.Id = p.Get<int>("@id");

                return person;
            }
        }

        /// <summary>
        /// Saves a new prize to the database
        /// </summary>
        /// <param name="model">The prize information</param>
        /// <returns>The prize information, including the unique identifier</returns>
        public PrizeModel CreatePrize(PrizeModel model)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(db)))
            {
                var p = new DynamicParameters();
                p.Add("@PlaceNumber", model.PlaceNumber);
                p.Add("@PrizeAmount", model.PrizeAmount);
                p.Add("@PlaceName", model.PlaceName);
                p.Add("@PrizePercentage", model.PrizePercentage);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("dbo.spPrizes_Insert", p, commandType:CommandType.StoredProcedure);
                model.Id = p.Get<int>("@id");

                return model;
            }
        }

        /// <summary>
        /// Saves a new team to the database
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public TeamModel CreateTeam(TeamModel model)
        { 

            //Connect to db
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(db)))
            {
                //Add parameters to insert Team
                var p = new DynamicParameters();
                p.Add("@TeamName", model.TeamName);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                //Execute SQL Statement that will run stored procedure inserting team into Teams db
                connection.Execute("dbo.spTeams_Insert", p, commandType: CommandType.StoredProcedure);
                //Get inserted id from table
                model.Id = p.Get<int>("@id");

                //Insert new row into TeamMembers table for each TeamMember in TeamModel.TeamMembers
                //The row will contain Team id, and player id
                foreach (PersonModel person in model.TeamMembers)
                {
                    p = new DynamicParameters();
                    p.Add("@TeamId", model.Id);
                    p.Add("@PersonId", person.Id);


                    connection.Execute("dbo.spTeamMembers_Insert", p, commandType: CommandType.StoredProcedure);
                }

                return model;
            }
        }

        public void CreateTournament(TournamentModel model)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(db)))
            {
                SaveTournament(model,connection);

                SaveTournamentPrizes(model, connection);

                SaveTournamentEntries(model, connection);

                SaveTournamentRounds(model, connection);
            }
        }

        private void SaveTournamentRounds(TournamentModel model, IDbConnection connection)
        {

            // Loop through the rounds
            // Loop through the matchups
            // Save the matchup
            // Loop through the entries and save them
            int roundCounter = 0;

            foreach (List<MatchupModel> round in model.Rounds)
            {
                foreach (MatchupModel matchup in round)
                {

                    var p = new DynamicParameters();

                    // Add matchup to db
                    p.Add("@TournamentId", model.Id);
                    p.Add("@MatchupRound",matchup.MatchupRound);
                    p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                    connection.Execute("dbo.spMatchups_Insert", p, commandType: CommandType.StoredProcedure);

                    //get inserted matchup id from database
                    matchup.Id = p.Get<int>("@id");

                    // Add matchup entry to db
                    foreach (MatchupEntryModel entryModel in matchup.Entries)
                    {
                        p = new DynamicParameters();
                        p.Add("@MatchupId", matchup.Id);
                        
                        if (entryModel.ParentMatchup == null)
                        {
                            p.Add("@ParentMatchupId", null);
                        }
                        else
                        {
                            p.Add("@ParentMatchupId", entryModel.ParentMatchup.Id);
                        }
                        if(entryModel.TeamCompeting == null)
                        {
                            p.Add("@TeamCompetingId", null);
                        }
                        else
                        {
                            p.Add("@TeamCompetingId", entryModel.TeamCompeting.Id);
                        }
                       
                        p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                        connection.Execute("dbo.spMatchupEntries_Insert", p, commandType: CommandType.StoredProcedure);

                    }
                }
                roundCounter++;
            }

        }

        private void SaveTournament(TournamentModel tournamentModel, IDbConnection connection)
        {
            var p = new DynamicParameters();
            p.Add("@TournamentName", tournamentModel.TournamentName);
            p.Add("@EntryFee", tournamentModel.EntryFee);
            p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

            //Execute SQL Statement that will run stored procedure inserting team into Teams db
            connection.Execute("dbo.spTournaments_Insert", p, commandType: CommandType.StoredProcedure);
            //Get inserted id from table
            tournamentModel.Id = p.Get<int>("@id");
        }

        private void SaveTournamentPrizes(TournamentModel tournamentModel, IDbConnection connection)
        {
            foreach (PrizeModel pz in tournamentModel.Prizes)
                {
                    var p = new DynamicParameters();

                    p.Add("@TournamentId", tournamentModel.Id);
                    p.Add("@PrizeId", pz.Id);
                    p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                    connection.Execute("dbo.spTournamentPrizes_Insert", p, commandType: CommandType.StoredProcedure);
                }
        }

        private void SaveTournamentEntries(TournamentModel tournamentModel, IDbConnection connection)
        {
            foreach (TeamModel tm in tournamentModel.EnteredTeams)
            {
                var p = new DynamicParameters();

                p.Add("@TournamentId", tournamentModel.Id);
                p.Add("@TeamId", tm.Id);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("dbo.spTournamentEntries_Insert", p, commandType: CommandType.StoredProcedure);
            }
        }

        /// <summary>
        /// Gets all people from People table
        /// </summary>
        /// <returns></returns>
        public List<PersonModel> GetPerson_All()
        {
            List<PersonModel> output;
            using(IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(db)))
            {
                output = connection.Query<PersonModel>("dbo.spPeople_GetAll").ToList();
            }
            return output;
            
        }

        public List<TeamModel> GetTeam_All()
        {
            List<TeamModel> output;
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(db)))
            {
                output = connection.Query<TeamModel>("dbo.spTeam_GetAll").ToList();
                foreach(TeamModel team in output)
                {
                    var p = new DynamicParameters();
                    p.Add("@TeamId", team.Id);
                   // p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
                    team.TeamMembers = connection.Query<PersonModel>("dbo.spTeamMembers_GetByTeam",p, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            return output;

        }
    }
}
