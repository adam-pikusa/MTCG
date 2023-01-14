using MTCG.Models;

namespace MTCG.BL
{
    public class Elo
    {
        public static void TransferElo(UserStats loser, UserStats winner)
        {
            loser.Elo -= 50;
            winner.Elo += 50;
            loser.Losses++;
            winner.Wins++;
        }
    }
}
