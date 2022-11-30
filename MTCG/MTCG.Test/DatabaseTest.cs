using MTCG.DAL;

namespace MTCG.Test
{
    public class DatabaseTest
    {
        IDatabase db;

        [SetUp]
        public void Setup()
        {
            db = new PGDatabase();    
        }

        [Test]
        public void TestInit()
        {
            Assert.That(db.HelloWorld(), Is.EqualTo(1234));
        }
    }
}
