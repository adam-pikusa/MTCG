using MTCG.BL;
using Newtonsoft.Json.Linq;
using System.Net.Sockets;

const string assetFile = @"Assets/cards.json";

string ResponseError(string errorCode)
{
    string htmlPayload =
        "<!DOCTYPE html>" +
        "<html>" +
        "<head>" +
        $"<title>{errorCode}</title>" +
        "</head>" +
        $"<body><h1>{errorCode}</h1></body>" +
        "</html>";

    string response =
@$"HTTP/1.1 {errorCode}
Content-Type: text/html
Content-Length: {htmlPayload.Length}
Connection: Close

{htmlPayload}";

    return response;
}

string Response400 = ResponseError("400 Bad Request");
string Response401 = ResponseError("401 Unauthorized");
string Response404 = ResponseError("404 Not Found");
string Response500 = ResponseError("500 Internal Server Error");

string ResponseJson(string jsonPayload)
{
    string response =
@$"HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8
Content-Length: {jsonPayload.Length}
Connection: Close

{jsonPayload}";

    return response;
}

var cards = CardDeserializer.deserializeAllCards(File.ReadAllText(assetFile));

string HandleGet(string route)
{
    Console.WriteLine("[{0}]Received request for route: {1}", Thread.CurrentThread.ManagedThreadId, route);

    var parts = route.Substring(1).Split('/');

    if (parts.Length != 2) return Response400;

    object responseObject;

    switch (parts[0])
    {
        case "double": 
            responseObject = new { 
                request_route = route, 
                result = int.Parse(parts[1]) * 2 }; 
            break;

        case "halve": 
            responseObject = new { 
                request_route = route, 
                result = int.Parse(parts[1]) / 2 }; 
            break;

        case "cards":
            responseObject = cards[int.Parse(parts[1])];
            break;

        default: return Response404;
    }

    return ResponseJson(JObject.FromObject(responseObject).ToString());
}

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

            bool doneReadingHeader = false;
            List<string> body = new();

            string? auth = null;
            string response = Response500;
            
            while ((currentLine = reader.ReadLine()) != null)
            {
                Console.WriteLine("[{0}]{1}", Thread.CurrentThread.ManagedThreadId, currentLine);

                if (firstLine)
                {
                    firstLine = false;

                    var requestParts = currentLine.Split(' ');

                    if (requestParts.Length != 3 || requestParts[2] != "HTTP/1.1")
                    {
                        Console.Error.WriteLine("[{0}]Received malformed http request", Thread.CurrentThread.ManagedThreadId);
                        writer.Write(Response400);
                        return;
                    }

                    switch (requestParts[0])
                    {
                        case "GET": response = HandleGet(requestParts[1]); break;

                        default:
                            Console.Error.WriteLine("[{0}]Received unknown verb: {1}", Thread.CurrentThread.ManagedThreadId, requestParts[0]);
                            writer.Write(Response400);
                            return;
                    }
                }

                if (currentLine.StartsWith("Authorization: "))
                {
                    auth = currentLine.Split(' ')[2];
                    Console.WriteLine("[{0}]Parsed auth info: {1}", Thread.CurrentThread.ManagedThreadId, auth);
                }

                if (currentLine == "")
                {
                    Console.WriteLine("[{0}]Done reading header!", Thread.CurrentThread.ManagedThreadId);
                    doneReadingHeader = true;
                    break;
                }

                if (doneReadingHeader)
                {
                    body.Add(currentLine);
                }
            }

            Console.WriteLine("[{0}]Checking auth info [{1}]...", Thread.CurrentThread.ManagedThreadId, auth);

            if (false)
            {
                Console.Error.WriteLine("[{0}]Authentication failed for {1}!", Thread.CurrentThread.ManagedThreadId, tcpClient.Client.RemoteEndPoint);
                writer.Write(Response401);
                return;
            }

            Console.WriteLine("[{0}]User authenticated.", Thread.CurrentThread.ManagedThreadId);

            writer.Write(response);
        }
    }
    catch (Exception exc)
    {
        Console.Error.WriteLine("[{0}]Caught exception: {1}", Thread.CurrentThread.ManagedThreadId, exc.Message);
    }
}


TcpListener listener = new(System.Net.IPAddress.Loopback, 80);
listener.Start();

while (true)
{
    var newReq = listener.AcceptTcpClient();
    Console.WriteLine("Queueing new request handler for {0}!", newReq.Client.RemoteEndPoint);
    ThreadPool.QueueUserWorkItem(HandleClient, newReq);
}