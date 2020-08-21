﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TrackerLibrary.Models
{
    /// <summary>
    /// Represents one tournament
    /// </summary>
    public class TournamentModel
    {

        public event EventHandler<DateTime> OnTournamentComplete;

        /// <summary>
        /// The unique identifier for the tournament.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Represents name of tournament.
        /// </summary>
        public string TournamentName { get; set; }

        /// <summary>
        /// Represents tournament entry fee amount
        /// </summary>
        public decimal EntryFee { get; set; }

        /// <summary>
        /// Represents teams that entered tournament
        /// </summary>
        public List<TeamModel> EnteredTeams { get; set; } = new List<TeamModel>();

        /// <summary>
        /// Represents prizez that might be won my teams
        /// </summary>
        public List<PrizeModel> Prizes { get; set; } = new List<PrizeModel>();

        /// <summary>
        /// Represents list of rounds already played in tournament
        /// </summary>
        public List<List<MatchupModel>> Rounds { get; set; } = new List<List<MatchupModel>>();

        public void CompleteTournament()
        {
            OnTournamentComplete?.Invoke(this, DateTime.Now);
        }

    }
}
