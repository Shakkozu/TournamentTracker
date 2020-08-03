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

        public List<PersonModel> GetPerson_All()
        {
            List<PersonModel> output;
            using(IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(db)))
            {
                output = connection.Query<PersonModel>("dbo.spPeople_GetAll").ToList();
            }
            return output;
            
        }
    }
}
