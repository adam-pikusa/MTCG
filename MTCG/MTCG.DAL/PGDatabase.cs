namespace MTCG.DAL
{
    public class PGDatabase : IDatabase
    {
        public PGDatabase() 
        {
            Console.WriteLine("Database init");
        }

        public int HelloWorld()
        {
            Console.WriteLine("Hello world from DB");
            return 1234;
        }
    }
}
