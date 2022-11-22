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

        public string Get(string[] route)
        {
            var index = int.Parse(route[1]);

            if (index >= cards.Count)
                return HTTPHelper.Response404;

            return HTTPHelper.ResponseJson(JObject.FromObject(cards[index]).ToString());
        }

        public string Delete(string[] route) => throw new NotImplementedException();

        public string Post(string[] route, string body)
        {
            Console.WriteLine("[{0}]Received route {1} and body({2}):{3}", Thread.CurrentThread.ManagedThreadId, route, body.Length, body);

            return HTTPHelper.Response404;
        }

        public string Patch(string[] route, string body)
        {
            throw new NotImplementedException();
        }
    }
}
