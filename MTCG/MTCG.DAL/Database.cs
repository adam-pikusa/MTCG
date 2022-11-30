namespace MTCG.DAL
{
    public static class Database
    {
        public static IDatabase Instance { get; } 
            = new PGDatabase();
    }
}
