using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.DAL
{
    public class PGDatabase : IDatabase
    {
        static PGDatabase instance;
        static PGDatabase() => instance = new PGDatabase();

        private PGDatabase() 
        {
            Console.WriteLine("Database init");
        }

        public IDatabase Instance() => instance;

        public void HelloWorld()
        {
            Console.WriteLine("Hello world from DB");
        }
    }
}
