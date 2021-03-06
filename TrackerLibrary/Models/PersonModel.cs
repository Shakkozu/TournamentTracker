﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TrackerLibrary.Models
{
    /// <summary>
    /// Represents one person
    /// </summary>
    public class PersonModel
    {
        /// <summary>
        /// The unique identifier for the person.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Represents Player 'sFirstName
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Represents Player's LastName
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Represents Player's EmailAddress
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Represents Player's Cellphone Number
        /// </summary>
        public string CellphoneNumber { get; set; }

        public string FullName 
        {
            get
            {
                return $"{ FirstName } { LastName }";
            }
        }
        
     
    }
}
