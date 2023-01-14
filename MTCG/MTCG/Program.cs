 using MTCG;
using MTCG.Apis;
using MTCG.BL;
using System.Net.Sockets;

var apis = new Dictionary<string, IAPIEndPoint>
{
    { "battles", new BattlesAEP() },
    { "cards", new CardsAEP() },
    { "deck", new DeckAEP() },
    { "packages", new PackagesAEP() },
    { "score", new ScoreAEP() },
    { "sessions", new SessionsAEP() },
    { "stats", new StatsAEP() },
    { "tradings", new TradingsAEP() },
    { "transactions", new TransactionsAEP() },
    { "users", new UsersAEP() },
};

void HandleClient(object? state)
{
    if (state == null) return;
    using var tcpClient = (TcpClient)state;
    var netStream = tcpClient.GetStream();

    try
    {
        using (var reader = new StreamReader(netStream, System.Text.Encoding.UTF8))
        using (var writer = new StreamWriter(netStream, System.Text.Encoding.UTF8))
        {
            string? currentLine = null;
            bool firstLine = true;
            int contentLength = -1;

            string? verb = null;
            string? route = null;
            string? auth = null;
            char[]? body = null;
            
            while ((currentLine = reader.ReadLine()) != null)
            {
                Console.WriteLine("[{0}]{1}", Thread.CurrentThread.ManagedThreadId, currentLine);

                if (firstLine)
                {
                    firstLine = false;

                    var requestParts = currentLine.Split(' ');

                    if (requestParts.Length != 3 || requestParts[2] != "HTTP/1.1")
                    {
                        Console.Error.WriteLine("[{0}]Received malformed http request (requestParts.Length != 3 || requestParts[2] != \"HTTP/1.1\")", Thread.CurrentThread.ManagedThreadId);
                        writer.Write(HTTPHelper.Response400);
                        return;
                    }

                    verb = requestParts[0];
                    route = requestParts[1];
                }
                else if (currentLine.StartsWith("Authorization: "))
                {
                    auth = currentLine.Split(' ')[2];
                    Console.WriteLine("[{0}]Parsed auth info: {1}", Thread.CurrentThread.ManagedThreadId, auth);
                }
                else if (currentLine.StartsWith("Content-Length: "))
                {
                    contentLength = int.Parse(currentLine.Split(' ')[1]);
                }
                else if (currentLine == "")
                {
                    Console.WriteLine("[{0}]Done reading header!", Thread.CurrentThread.ManagedThreadId);
                    
                    if (contentLength > 1)
                    {
                        body = new char[contentLength];
                        if (reader.Read(body, 0, contentLength) != contentLength)
                        {
                            Console.Error.WriteLine("[{0}]Failed to read entire request body", Thread.CurrentThread.ManagedThreadId);
                            writer.Write(HTTPHelper.Response500);
                            return;
                        }
                    }

                    break;
                }
            }

            Console.WriteLine("[{0}]Checking auth info [{1}]...", Thread.CurrentThread.ManagedThreadId, auth);

            auth = SessionHandler.GetUsername(auth);

            if (false)
            {
                Console.Error.WriteLine("[{0}]Authentication failed for {1}!", Thread.CurrentThread.ManagedThreadId, tcpClient.Client.RemoteEndPoint);
                writer.Write(HTTPHelper.Response401);
                return;
            }

            Console.WriteLine("[{0}]User authenticated.", Thread.CurrentThread.ManagedThreadId);

            var routeParts = route.Substring(1).Split('/');

            if (routeParts.Length < 1)
            {
                Console.Error.WriteLine("[{0}]Received malformed http request (routeParts.Length < 1)", Thread.CurrentThread.ManagedThreadId);
                writer.Write(HTTPHelper.Response400);
                return;
            }

            if (routeParts[0].Contains('?'))
            {
                Console.WriteLine("Detected URL parameter: {0}", routeParts[0]);
                routeParts[0] = routeParts[0].Split('?')[0]; // Remove URL Parameters
            }

            if (!apis.TryGetValue(routeParts[0], out var api))
            {
                Console.Error.WriteLine("[{0}]Received http request for undefined route: {1}", Thread.CurrentThread.ManagedThreadId, routeParts[0]);
                writer.Write(HTTPHelper.Response404);
                return;
            }

            string response;

            switch (verb)
            {
                case "DELETE": response = api.Delete(auth, route); break;
                case "GET": response = api.Get(auth, route); break;
                case "PUT": response = api.Put(auth, route, new string(body)); break;
                case "PATCH": response = api.Patch(auth, route, new string(body)); break;
                case "POST": response = api.Post(auth, route, new string(body)); break;
                default: 
                    Console.Error.WriteLine("[{0}]Received unknown HTTP verb: {1}", Thread.CurrentThread.ManagedThreadId, verb);
                    writer.Write(HTTPHelper.Response400);
                    return;
            }

            writer.Write(response);
        }
    }
    catch (SocketException exc)
    {
        Console.Error.WriteLine("[{0}]Caught exception: {1}", Thread.CurrentThread.ManagedThreadId, exc.Message);
    }
}


var listener = new TcpListener(System.Net.IPAddress.Loopback, 10001);
listener.Start();

while (true)
{
    var newReq = listener.AcceptTcpClient();
    Console.WriteLine("Queueing new request handler for {0}!", newReq.Client.RemoteEndPoint);
    ThreadPool.QueueUserWorkItem(HandleClient, newReq);
}