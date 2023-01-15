using MTCG.Models;

namespace MTCG.BL
{
    public class Elo
    {
        public static void TransferElo(UserStats loser, UserStats winner)
        {
            if (loser.Elo >= 5) loser.Elo -= 5;
            else loser.Elo = 0;

            winner.Elo += 3;
            loser.Losses++;
            winner.Wins++;
        }
    }
}
