using MTCG.DAL;

namespace MTCG.Test
{
    public class DatabaseTest
    {
        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void TestInit()
        {
            Assert.That(Database.Instance.HelloWorld(), Is.EqualTo(1234));
        }
    }
}
