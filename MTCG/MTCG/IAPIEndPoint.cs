namespace MTCG
{
    internal interface IAPIEndPoint
    {
        string Route();
        string Get(string route);
        string Post(string route);
        string Patch(string route);
        string Delete(string route);
    }
}
