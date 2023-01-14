namespace MTCG.Apis
{
    internal class BattlesAEP : IAPIEndPoint
    {
        public string Delete(string username, string route) => HTTPHelper.Response400;
        public string Get(string username, string route) => HTTPHelper.Response400;
        public string Patch(string username, string route, string body) => HTTPHelper.Response400;
        public string Put(string username, string route, string body) => HTTPHelper.Response400;

        public string Post(string username, string route, string body)
        {
            return HTTPHelper.Response400;
        }
    }
}
