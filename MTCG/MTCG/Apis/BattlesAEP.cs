using Microsoft.Extensions.Logging;
using MTCG.BL;
using MTCG.Models;
using Newtonsoft.Json.Linq;

namespace MTCG.Apis
{
    internal class BattlesAEP : IAPIEndPoint
    {
        ILogger log = Logging.Get<BattlesAEP>();

        public string Delete(string username, string route) => HTTPHelper.Response400;
        public string Patch(string username, string route, string body) => HTTPHelper.Response400;

        public string Get(string username, string route)
        {
            var parts = route.Substring(1).Split('/');

            if (parts.Length >= 2) 
                switch (parts[1]) 
                {
                    case "friends":
                        {
                            log.LogDebug("got request for friend battles of {0}", username);
                            if (!Database.Instance.GetFriendbattles(username, out var friendBattles)) return HTTPHelper.Response400;
                            return HTTPHelper.ResponseJson(JArray.FromObject(friendBattles).ToString());
                        }
                    default: return HTTPHelper.Response404;
                }

            if (!Database.Instance.GetBattles(out var battlers)) return HTTPHelper.Response500;

            return HTTPHelper.ResponseJson(JArray.FromObject(battlers).ToString());
        }
        
        public string Put(string username, string route, string body)
        {
            if (!Database.Instance.CreateBattleChallenge(username)) return HTTPHelper.Response400;

            log.LogDebug("user {0} created battle challenge", username);

            return HTTPHelper.Response200;
        }

        public string Post(string username, string route, string body)
        {
            if (body == username)
            {
                log.LogWarning("user {0} tried to challenge self to battle", username);
                return HTTPHelper.Response400;
            }

            if (!Database.Instance.GetBattles(out var battlers)) return HTTPHelper.Response500;

            log.LogDebug("current battler count: {0}", battlers.Length);
            for (int i = 0; i < battlers.Length; i++)
                log.LogDebug("{0} [{1}]", i, battlers[i]);

            if (!battlers.Contains(body))
            {
                log.LogWarning("user {0} tried to fight {1}, who is not challenging to battle", username, body);
                return HTTPHelper.Response400;
            }

            if (!Database.Instance.GetUserId(username, out var id)) return HTTPHelper.Response400;
            if (!Database.Instance.GetUserId(body, out var opponentId)) return HTTPHelper.Response400;
            if (!Database.Instance.GetDeck(id, out Deck deckA)) return HTTPHelper.Response400;
            if (!Database.Instance.GetDeck(opponentId, out Deck deckB)) return HTTPHelper.Response400;

            log.LogInformation("initiating battle between {0} and {1}", username, body);

            var result = Battle.Fight(deckA, deckB);

            if (result != 0)
            {
                if (!Database.Instance.GetUserStats(username, out UserStats statsA)) return HTTPHelper.Response500;
                if (!Database.Instance.GetUserStats(body, out UserStats statsB)) return HTTPHelper.Response500;

                log.LogInformation(result == 1 ?
                    "transfering elo from {0} to {1}" :
                    "transfering elo from {1} to {0}", body, username);

                if (result == 1)
                    Elo.TransferElo(statsB, statsA);
                else
                    Elo.TransferElo(statsA, statsB);

                if (!Database.Instance.SetUserStats(username, statsA)) return HTTPHelper.Response500;
                if (!Database.Instance.SetUserStats(body, statsB)) return HTTPHelper.Response500;
            }

            log.LogInformation("removing battle challenge...");
            Database.Instance.EndBattle(body);

            return HTTPHelper.Response200;
        }
    }
}
