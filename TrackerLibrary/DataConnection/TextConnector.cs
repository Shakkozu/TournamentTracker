﻿using System;
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
            TextConnectorProcessor.SaveToPrizeFile(prizes, PrizesFile);

            return model;
        }
    }
}
