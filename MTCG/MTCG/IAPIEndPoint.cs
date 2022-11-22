namespace MTCG
{
    internal interface IAPIEndPoint
    {
        string Route();
        string Get(string[] route);
        string Post(string[] route, string body);
        string Patch(string[] route, string body);
        string Delete(string[] route);
    }
}
