namespace MTCG
{
    internal interface IAPIEndPoint
    {
        string Get(string username, string route);
        string Put(string username, string route, string body);
        string Post(string username, string route, string body);
        string Patch(string username, string route, string body);
        string Delete(string username, string route);
    }
}
