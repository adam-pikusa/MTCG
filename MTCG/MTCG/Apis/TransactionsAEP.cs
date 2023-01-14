using Microsoft.Extensions.Logging;

namespace MTCG.Apis
{
    internal class TransactionsAEP : IAPIEndPoint
    {
        ILogger log = Logging.Get<TransactionsAEP>();

        public string Delete(string username, string route) => HTTPHelper.Response400;
        public string Get(string username, string route) => HTTPHelper.Response400;
        public string Patch(string username, string route, string body) => HTTPHelper.Response400;
        
        public string Put(string username, string route, string body)
        {
            if (username != "admin") return HTTPHelper.Response401;

            var parts = route.Substring(1).Split('/');

            if (parts.Length >= 2)
                switch (parts[1])
                {
                    case "coins":
                        {
                            if (parts.Length != 3) return HTTPHelper.Response400;
                            log.LogDebug("trying to add/remove {0} coins to/from {1}", body, parts[2]);
                            if (!Database.Instance.GetUserId(parts[2], out var id)) return HTTPHelper.Response400;
                            if (!long.TryParse(body, out var coinAmountDelta)) return HTTPHelper.Response400;
                            if (!Database.Instance.ChangeCoinAmount(id, coinAmountDelta)) return HTTPHelper.Response400;
                            log.LogDebug("added/removed {0} coins to/from {1}", coinAmountDelta, parts[2]);
                            return HTTPHelper.Response200;
                        }
                }

            return HTTPHelper.Response400;
        }

        public string Post(string username, string route, string body)
        {
            var parts = route.Split('/');

            if (parts.Length < 2) return HTTPHelper.Response400;

            switch (parts[2])
            {
                case "packages":
                    {
                        if (!Database.Instance.GetUserId(username, out var id)) return HTTPHelper.Response400;
                        if (!Database.Instance.GetPacks(out var packs)) return HTTPHelper.Response500;
                        if (packs.Length < 1) return HTTPHelper.Response400;
                        if (!Database.Instance.BuyPack(id, packs[0])) return HTTPHelper.Response400;
                        return HTTPHelper.Response200;
                    }
            }

            return HTTPHelper.Response400;
        }
    }
}
