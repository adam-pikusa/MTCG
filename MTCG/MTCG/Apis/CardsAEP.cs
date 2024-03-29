﻿using Microsoft.Extensions.Logging;
using MTCG.BL;
using Newtonsoft.Json.Linq;

namespace MTCG.Apis
{
    internal class CardsAEP : IAPIEndPoint
    {
        ILogger log = Logging.Get<CardsAEP>();

        public string Get(string username, string route)
        {
            if (username == null) return HTTPHelper.Response400;

            var parts = route.Substring(1).Split('/');

            log.LogTrace("Got GET {0}", route);

            if (parts.Length < 3)
            {
                if (!Database.Instance.GetUserId(username, out var id)) return HTTPHelper.Response400;
                if (!Database.Instance.GetUserStackCards(id, out var cards)) return HTTPHelper.Response400;
                log.LogDebug("Returning {0} cards as json", cards.Length);
                return HTTPHelper.ResponseJson(CardDeserializer.SerializeCardArray(cards));
            }

            return HTTPHelper.Response400;
        }

        public string Put(string username, string route, string body) => HTTPHelper.Response400;

        public string Delete(string username, string route) => HTTPHelper.Response400;

        public string Post(string username, string route, string body)
        {
            log.LogTrace("[{0}]Received route {1} and body({2}):{3}", Thread.CurrentThread.ManagedThreadId, route, body.Length, body);

            return HTTPHelper.Response404;
        }

        public string Patch(string username, string route, string body) => HTTPHelper.Response400;
    }
}
