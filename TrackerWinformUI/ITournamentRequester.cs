using TrackerLibrary.Models;

namespace TrackerWinformUI
{
    public interface ITournamentRequester
    {
        void TournamentComplete(TournamentModel model);
    }
}