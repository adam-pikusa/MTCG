using MTCG.DAL;

namespace MTCG
{
    internal static class Database
    {
        static PGDatabase instance = null;

        public static IDatabase Instance 
        { 
            get
            {
                if (instance == null)
                {
                    instance = new PGDatabase();
                    instance.Init();
                }

                return instance;
            }
        }
    }
}
