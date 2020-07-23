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
        /// Represents list of team competitors
        /// </summary>
        public List<PersonModel> TeamMembers { get; set; } = new List<PersonModel>();

        /// <summary>
        /// Represents name of team
        /// </summary>
        public string TeamName { get; set; }





    }
}
