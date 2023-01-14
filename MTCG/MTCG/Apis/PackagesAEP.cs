using MTCG.BL;

namespace MTCG.Apis
{
    internal class PackagesAEP : IAPIEndPoint
    {
        public string Delete(string username, string route) => HTTPHelper.Response400;
        public string Get(string username, string route) => HTTPHelper.Response400;
        public string Put(string username, string route, string body) => HTTPHelper.Response400;
        public string Patch(string username, string route, string body) => HTTPHelper.Response400;

        public string Post(string username, string route, string body)
        {
            try
            {
                if (!Database.Instance.CreatePack(CardDeserializer.DeserializeCardsArray(body).ToArray()))
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
