namespace MTCG.BL
{
    public static class SessionHandler
    {
        public static string GetToken(string username) => $"{username}-mtcgToken";
        public static string GetUsername(string token) => token?.Substring(0, token.Length - "-mtcgToken".Length);
    }
}
