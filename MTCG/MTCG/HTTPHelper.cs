namespace MTCG
{
    internal static class HTTPHelper
    {
        static string ResponseError(string errorCode)
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

        public static readonly string Response400 = ResponseError("400 Bad Request");
        public static readonly string Response401 = ResponseError("401 Unauthorized");
        public static readonly string Response404 = ResponseError("404 Not Found");
        public static readonly string Response500 = ResponseError("500 Internal Server Error");

        public static string ResponseJson(string jsonPayload)
        {
            string response =
        @$"HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8
Content-Length: {jsonPayload.Length}
Connection: Close

{jsonPayload}";

            return response;
        }
    }
}
