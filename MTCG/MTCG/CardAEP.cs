using MTCG.BL;
using MTCG.Models;
using Newtonsoft.Json.Linq;

namespace MTCG
{
    internal class CardAEP : IAPIEndPoint
    {
        const string assetFile = @"Assets/cards.json";

        static readonly List<Card> cards = CardDeserializer.deserializeAllCards(File.ReadAllText(assetFile));

        public string Route() => "cards";

        public string Get(string route)
        {
            var parts = route.Substring(1).Split('/');
            var index = int.Parse(parts[1]);

            if (index >= cards.Count)
                return HTTPHelper.Response404;

            return HTTPHelper.ResponseJson(JObject.FromObject(cards[index]).ToString());
        }

        public string Delete(string route) => throw new NotImplementedException();
        public string Patch(string route) => throw new NotImplementedException();
        public string Post(string route) => throw new NotImplementedException();
    }
}
