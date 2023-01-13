using MTCG.DAL;
using System.Security.Cryptography;
using System.Text;

namespace MTCG.Test
{
    public class DatabaseTest
    {
        PGDatabase db;

        [SetUp]
        public void Setup()
        {
            db = new PGDatabase();    
            db.Init();
        }

        [Test]
        public void TestCard1()
        {
            var comps = db.ReadCardComps(1);
            
            Assert.That(comps.Count, Is.EqualTo(2));
            
            Assert.That(comps[0], Is.EqualTo(new PGDatabase.TestCompA { Data = 123 }));
            Assert.That(comps[1], Is.EqualTo(new PGDatabase.TestCompB { DataS = "data1", DataI = 11 }));
        }

        [Test]
        public void TestCard2() 
        {
            var comps = db.ReadCardComps(2);
            
            Assert.That(comps.Count, Is.EqualTo(1));

            Assert.That(comps[0], Is.EqualTo(new PGDatabase.TestCompB { DataS = "data2", DataI = 22 }));
        }

        [Test]
        public void TestPack1() 
        { 
            var packCards = db.ReadPackCards(1);
            Assert.That(packCards.Count, Is.EqualTo(1));
            Assert.That(packCards[0], Is.EqualTo(1));
        }

        [Test]
        public void TestPack2()
        {
            var packCards = db.ReadPackCards(2);
            Assert.That(packCards.Count, Is.EqualTo(2));
            Assert.That(packCards[0], Is.EqualTo(1));
            Assert.That(packCards[1], Is.EqualTo(2));
        }

        [Test]
        public void CreateUser()
        {
            db.CreateUser("testuser1", "testpass1");
            Assert.That(db.CheckUserLogin("testuser1", "wrong password"), Is.EqualTo(false));
            Assert.That(db.CheckUserLogin("testuser1", "testpass1"), Is.EqualTo(true));

            db.CreateUser("testuser2", "pass2");
            db.GetScoreBoard(out var elos);

            Console.WriteLine("Got ratings:");
            foreach (var rating in elos)
                Console.WriteLine($"{rating.Item1}:{rating.Item2}");
        }

        [Test]
        public void CreatePack()
        {
            db.CreatePack(new Models.Card[]
            {
                new Models.Card("c1", Models.Card.CardType.Monster, Models.Card.ElementType.Normal, 10),
                new Models.Card("c2", Models.Card.CardType.Monster, Models.Card.ElementType.Normal, 20),
                new Models.Card("c3", Models.Card.CardType.Monster, Models.Card.ElementType.Normal, 30),
            });

            db.CreatePack(new Models.Card[]
            {
                new Models.Card("c4", Models.Card.CardType.Monster, Models.Card.ElementType.Normal, 40),
                new Models.Card("c5", Models.Card.CardType.Monster, Models.Card.ElementType.Normal, 50),
            });

            for (int i = 0; i < 10; i++) db.BuyPack(null);
        }

        [Test]
        public void BuyPack()
        {
            var newUserName = "cardpackbuyer";
            db.CreateUser(newUserName, "1234");
            
            Assert.That(db.GetUserId(newUserName, out var newUserId), Is.EqualTo(true));

            Console.WriteLine($"new guid: {newUserId}");
            db.BuyPack(newUserId);
        }
    }
}
