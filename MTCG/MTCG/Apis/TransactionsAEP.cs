namespace MTCG.Apis
{
    internal class TransactionsAEP : IAPIEndPoint
    {
        public string Delete(string username, string route) => HTTPHelper.Response400;
        public string Get(string username, string route) => HTTPHelper.Response400;
        public string Put(string username, string route, string body) => HTTPHelper.Response400;
        public string Patch(string username, string route, string body) => HTTPHelper.Response400;

        public string Post(string username, string route, string body)
        {
            var parts = route.Split('/');

            if (parts.Length < 2) return HTTPHelper.Response400;

            switch (parts[2])
            {
                case "packages":
                    {
                        if (!Database.Instance.GetUserId(username, out var id)) return HTTPHelper.Response400;
                        if (!Database.Instance.GetPacks(out var packs)) return HTTPHelper.Response500;
                        if (packs.Length < 1) return HTTPHelper.Response400;
                        if (!Database.Instance.BuyPack(id, packs[0])) return HTTPHelper.Response400;
                        return HTTPHelper.Response200;
                    }
            }

            return HTTPHelper.Response400;
        }
    }
}
