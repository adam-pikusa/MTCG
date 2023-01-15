using MTCG.DAL;
using MTCG.Models;

namespace MTCG.Test
{
    public class DatabaseTest
    {
        PGDatabase db;
        List<Card> testCards;

        [OneTimeSetUp]
        public void Setup()
        {
            db = new PGDatabase();    
            db.Init();
            db.Clear();

            testCards = new List<Card>()
            {
                new Card("m1", Card.CardType.Monster, Card.ElementType.Normal, 10),
                new Card("m2", Card.CardType.Monster, Card.ElementType.Normal, 20),
                new Card("m3", Card.CardType.Monster, Card.ElementType.Normal, 30),
                new Card("m4", Card.CardType.Monster, Card.ElementType.Normal, 40)
            };
        }

        [Test]
        public void TestLogin()
        {
            var name = "logintest_name";
            var password = "logintest_password";

            Assert.That(db.CreateUser(name, password), Is.EqualTo(true));
            Assert.That(db.CheckUserLogin(name, password), Is.EqualTo(true));
            Assert.That(db.CheckUserLogin(name, "wrongpassword"), Is.EqualTo(false));
        }

        [Test]
        public void TestDefaultScore()
        {
            var name = "scoretest_user";
            var password = "scoretest_password";
            
            Assert.That(db.CreateUser(name, password), Is.EqualTo(true));
            Assert.That(db.GetUserStats(name, out var stats), Is.EqualTo(true));
            Assert.That(stats.Wins, Is.EqualTo(0));
            Assert.That(stats.Losses, Is.EqualTo(0));
            Assert.That(stats.Elo, Is.EqualTo(100));
        }

        [Test]
        public void TestCreatePacks() 
        {
            Assert.True(db.GetPacks(out var packs1));

            Assert.True(db.CreatePack(testCards.ToArray()));
            Assert.True(db.GetPacks(out var packs2));
            Assert.That(packs2.Length, Is.EqualTo(packs1.Length + 1));
            
            Assert.True(db.CreatePack(testCards.ToArray()));
            Assert.True(db.GetPacks(out var packs3));
            Assert.That(packs3.Length, Is.EqualTo(packs2.Length + 1));
        }

        [Test]
        public void TestCreateDecks()
        {
            var name = "decktest_name";
            Assert.True(db.CreateUser(name, "123"));
            Assert.True(db.GetUserId(name, out var userId));
            Assert.True(db.CreatePack(testCards.ToArray()));
            Assert.True(db.GetPacks(out var packs));
            Assert.True(db.BuyPack(userId, packs[0]));
            Assert.True(db.GetUserStackCards(userId, out Card[] userCards));
            Assert.True(db.SetDeck(userId, userCards.Select(c => c.Guid.ToString()).ToArray()));
            Assert.True(db.GetDeck(userId, out Deck userDeck));
        }

        [Test]
        public void TestBattleChallenge()
        {
            var name = "battletest_name";

            Assert.True(db.CreateUser(name, "123"));
            Assert.True(db.CreateBattleChallenge(name));
            
            Assert.True(db.GetBattles(out var battles));
            Assert.True(battles.Contains(name));
            
            Assert.True(db.EndBattle(name));
            Assert.True(db.GetBattles(out var modifiedBattles));
            Assert.False(modifiedBattles.Contains(name));
        }

        [Test]
        public void TestFriendBattleChallenge()
        {
            var name = "friendbattletest_name";
            var friendName = "friendbattletest_friend_name";
            var nonFriendName = "friendbattletest_non_friend_name";

            Assert.True(db.CreateUser(name, "123"));
            Assert.True(db.CreateUser(friendName, "123"));
            Assert.True(db.CreateUser(nonFriendName, "123"));

            Assert.True(db.AddFriend(name, friendName));

            Assert.True(db.CreateBattleChallenge(friendName));
            Assert.True(db.CreateBattleChallenge(nonFriendName));

            Assert.True(db.GetBattles(out var battles));
            Assert.True(db.GetFriendBattles(name, out var friendBattles));

            Assert.That(battles.Length, Is.EqualTo(2));
            Assert.That(friendBattles.Length, Is.EqualTo(1));
        }

        [Test]
        public void TestUserData()
        {
            var name = "userdatatest_name";

            Assert.True(db.CreateUser(name, "123"));
            
            Assert.True(db.GetUserData(name, out var data));
            Assert.That(data.Bio, Is.EqualTo(null));
            Assert.That(data.Name, Is.EqualTo(null));
            Assert.That(data.Image, Is.EqualTo(null));

            Assert.True(db.SetUserData(name, new UserData 
            {
                Name = "nametest",
                Image = "imagetest"
            }));
            Assert.True(db.GetUserData(name, out var modifiedData));
            Assert.That(modifiedData.Bio, Is.EqualTo(null));
            Assert.That(modifiedData.Name, Is.EqualTo("nametest"));
            Assert.That(modifiedData.Image, Is.EqualTo("imagetest"));
        }
    }
}
