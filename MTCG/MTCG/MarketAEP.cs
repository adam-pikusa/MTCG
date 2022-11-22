using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG
{
    internal class MarketAEP : IAPIEndPoint
    {
        public string Route() => "market";

        public string Get(string[] route)
        {
            Console.WriteLine("[{0}]received get request for the market route: {1}", Thread.CurrentThread.ManagedThreadId, route);

            if (route.Length != 4) return HTTPHelper.Response400;

            switch (route[1])
            {
                case "trade":
                    if (!int.TryParse(route[2], out var userId)) return HTTPHelper.Response400;
                    if (!int.TryParse(route[3], out var content)) return HTTPHelper.Response400;
                    Console.WriteLine("[{0}] Received trade request for {1} belonging to {2}", Thread.CurrentThread.ManagedThreadId, userId, content);
                    return HTTPHelper.Response404;
            }

            return HTTPHelper.Response500;
        }

        public string Delete(string[] route)
        {
            throw new NotImplementedException();
        }

        public string Patch(string[] route, string body)
        {
            throw new NotImplementedException();
        }

        public string Post(string[] route, string body)
        {
            throw new NotImplementedException();
        }

    }
}
