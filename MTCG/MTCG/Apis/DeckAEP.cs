using MTCG.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MTCG.Apis
{
    internal class DeckAEP : IAPIEndPoint
    {
        public string Delete(string username, string route) => HTTPHelper.Response400;
        public string Patch(string username, string route, string body) => HTTPHelper.Response400;
        public string Post(string username, string route, string body) => HTTPHelper.Response400;

        public string Get(string username, string route)
        {
            if (!Database.Instance.GetUserId(username, out string id)) return HTTPHelper.Response400;
            if (!Database.Instance.GetDeck(id, out Card[] deck)) return HTTPHelper.Response400;
            return HTTPHelper.ResponseJson(JArray.FromObject(deck).ToString());
        }

        public string Put(string username, string route, string body)
        {
            if (!Database.Instance.GetUserId(username, out var id)) return HTTPHelper.Response400;

            string[] data = null;
            try
            {
                data = JsonConvert.DeserializeObject<string[]>(body);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return HTTPHelper.Response400;
            }

            if (data == null) return HTTPHelper.Response400;
            if (data.Length != 4) return HTTPHelper.Response400;

            if (!Database.Instance.SetDeck(id, data)) return HTTPHelper.Response400;
            return HTTPHelper.Response200;
        }
    }
}
