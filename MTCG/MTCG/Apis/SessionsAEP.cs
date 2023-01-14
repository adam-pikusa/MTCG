using Microsoft.Extensions.Logging;
using MTCG.BL;
using Newtonsoft.Json;

namespace MTCG.Apis
{
    internal class SessionsAEP : IAPIEndPoint
    {
        ILogger log = Logging.Get<SessionsAEP>();

        public string Delete(string username, string route) => HTTPHelper.Response400;
        public string Get(string username, string route) => HTTPHelper.Response400;
        public string Put(string username, string route, string body) => HTTPHelper.Response400;
        public string Patch(string username, string route, string body) => HTTPHelper.Response400;

        public string Post(string username, string route, string body)
        {
            string login;

            try
            {
                dynamic data = JsonConvert.DeserializeObject(body);
                login = data["Username"];
                string password = data["Password"];
                if (!Database.Instance.CheckUserLogin(login, password))
                    return HTTPHelper.Response401;
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
                return HTTPHelper.Response500;
            }

            return HTTPHelper.ResponsePlain(SessionHandler.GetToken(login));
        }
    }
}
