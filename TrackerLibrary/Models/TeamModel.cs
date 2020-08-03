using System;
using System.Collections.Generic;
using System.Text;

namespace TrackerLibrary.Models
{
    /// <summary>
    /// Represents one team
    /// </summary>
    public class TeamModel
    {
        /// <summary>
        /// The unique identifier for the team.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Represents name of team
        /// </summary>
        public string TeamName { get; set; }

        /// <summary>
        /// Represents list of team competitors
        /// </summary>
        public List<PersonModel> TeamMembers { get; set; } = new List<PersonModel>();

       



    }
}
