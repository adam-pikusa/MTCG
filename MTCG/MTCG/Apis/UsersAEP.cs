using MTCG.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MTCG.Apis
{
    internal class UsersAEP : IAPIEndPoint
    {
        public string Delete(string username, string route) => HTTPHelper.Response400;
        public string Patch(string username, string route, string body) => HTTPHelper.Response400;

        public string Get(string username, string route)
        {
            var parts = route.Substring(1).Split('/');
            if (parts.Length < 2)
                return HTTPHelper.Response400;

            var requestedUserData = parts[1];

            if (requestedUserData != username)
            {
                Console.WriteLine("user tried to get data of other user");
                return HTTPHelper.Response400;
            }

            if (!Database.Instance.GetUserData(username, out var data))
                return HTTPHelper.Response400;

            return HTTPHelper.ResponseJson(JObject.FromObject(data).ToString());
        }

        public string Put(string username, string route, string body)
        {
            var parts = route.Substring(1).Split('/');
            if (parts.Length < 2)
                return HTTPHelper.Response400;

            var requestedUserData = parts[1];

            if (requestedUserData != username)
            {
                Console.WriteLine("user tried to get data of other user");
                return HTTPHelper.Response400;
            }

            try
            {
                if (!Database.Instance.SetUserData(username, JsonConvert.DeserializeObject<UserData>(body)))
                    return HTTPHelper.Response400;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return HTTPHelper.Response400;
            }

            return HTTPHelper.Response200;
        }

        public string Post(string username, string route, string body)
        {
            Console.WriteLine($"Received users post: [{body}]");

            try
            {
                dynamic data = JsonConvert.DeserializeObject(body);
                string login = data["Username"];
                string password = data["Password"];
                if (!Database.Instance.CreateUser(login, password))
                    return HTTPHelper.Response400;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return HTTPHelper.Response500;
            }

            return HTTPHelper.Response200;
        }
    }
}
