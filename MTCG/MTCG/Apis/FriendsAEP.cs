using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace MTCG.Apis
{
    internal class FriendsAEP : IAPIEndPoint
    {
        ILogger log = Logging.Get<FriendsAEP>();

        public string Get(string username, string route) => HTTPHelper.Response400;
        public string Patch(string username, string route, string body) => HTTPHelper.Response400;
        public string Post(string username, string route, string body) => HTTPHelper.Response400;
        
        public string Delete(string username, string route)
        {
            var parts = route.Substring(1).Split('/');

            if (parts.Length != 2) return HTTPHelper.Response400;

            log.LogDebug("removing friend {0} from {1}", parts[1], username);
            if (!Database.Instance.RemoveFriend(username, parts[1])) return HTTPHelper.Response400;

            return HTTPHelper.Response200;
        }

        public string Put(string username, string route, string body)
        {
            if (!Database.Instance.AddFriend(username, body)) return HTTPHelper.Response400;
            log.LogDebug("user {0} added friend {1}", username, body);

            return HTTPHelper.Response200;
        }
    }
}
