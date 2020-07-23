namespace TrackerLibrary.Models
{
    public class PrizeModel
    {
        /// <summary>
        /// Represents number of place taken by the player.
        /// </summary>
        public int PlaceNumber { get; set; }

        /// <summary>
        /// Represents name of place pointed by PlaceNumber property.
        /// </summary>
        public string PlaceName { get; set; }

        /// <summary>
        /// Represents amount of prize.
        /// </summary>
        public decimal PrizeAmount { get; set; }

        /// <summary>
        /// Represents how much % from prize will be taken for organisator.
        /// </summary>
        public double PrizePercentage { get; set; }
    }
}