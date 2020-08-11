﻿using System;
using System.Collections.Generic;
using System.Text;
using TrackerLibrary.Models;

namespace TrackerLibrary.Interfaces
{
    public interface IDataConnection
    {
        /// <summary>
        /// Creates prize and adds it into actal data storage
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        PrizeModel CreatePrize(PrizeModel model);
        /// <summary>
        /// Creates person and adds it into actual data storage
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        PersonModel CreatePerson(PersonModel p);

        void CreateTournament(TournamentModel tm);

        void UpdateMatchup(MatchupModel m);

        TeamModel CreateTeam(TeamModel team);

        List<PersonModel> GetPerson_All();

        List<TeamModel> GetTeam_All();

        List<TournamentModel> GetTournament_All();


    }
}
