using Newtonsoft.Json.Linq;

namespace MTCG.Apis
{
    internal class StatsAEP : IAPIEndPoint
    {
        public string Delete(string username, string route) => HTTPHelper.Response400;
        public string Put(string username, string route, string body) => HTTPHelper.Response400;
        public string Patch(string username, string route, string body) => HTTPHelper.Response400;
        public string Post(string username, string route, string body) => HTTPHelper.Response400;

        public string Get(string username, string route)
        {
            if (!Database.Instance.GetUserStats(username, out var stats)) return HTTPHelper.Response400;
            return HTTPHelper.ResponseJson(JObject.FromObject(stats).ToString());
        }
    }
}
