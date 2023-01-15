using MTCG.BL;
using MTCG.Models;

namespace MTCG.Test
{
    public class EloTest
    {
        [TestCase(100, 100, 95, 103)]
        [TestCase(0, 10, 0, 13)]
        public void EloTransferTest(int a, int b, int a2, int b2)
        {
            var statsA = new UserStats { Elo = a };
            var statsB = new UserStats { Elo = b };

            Elo.TransferElo(statsA, statsB);

            Assert.That(statsA.Elo, Is.EqualTo(a2));
            Assert.That(statsB.Elo, Is.EqualTo(b2));
        }

        [Test]
        public void ScoreTest()
        {
            var statsA = new UserStats { Wins = 10, Losses = 5 };
            var statsB = new UserStats { Wins = 3, Losses = 8 };

            Elo.TransferElo(statsA, statsB);

            Assert.That(statsA.Losses, Is.EqualTo(5 + 1));
            Assert.That(statsA.Wins, Is.EqualTo(10));

            Assert.That(statsB.Losses, Is.EqualTo(8));
            Assert.That(statsB.Wins, Is.EqualTo(3 + 1));
        }
    }
}