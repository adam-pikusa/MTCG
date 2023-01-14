using MTCG.BL;
using MTCG.Models;

namespace MTCG.Test
{
    public class BattleTest
    {
        const string assetFile = @"Assets/cards.json";

        Dictionary<string, Card> cards;
        Dictionary<string, Deck> decks;

        [SetUp]
        public void Setup()
        {
            {
                var _cards = CardDeserializer.DeserializeCardsArray(File.ReadAllText(assetFile));

                cards = new Dictionary<string, Card>
                {
                    { "goblin", _cards[0] },
                    { "wizzard", _cards[1] },
                    { "ork", _cards[2] },
                    { "knight", _cards[3] },
                    { "kraken", _cards[4] },
                    { "dragon", _cards[5] },
                    { "fireElf", _cards[6] },
                    { "fireSpell", _cards[7] },
                    { "waterSpell", _cards[8] }
                };
            }

            decks = new Dictionary<string, Deck>
            {
                {"Gob", new Deck { cards["goblin"] } },
                {"Dra", new Deck { cards["dragon"] } },
                {"FEl", new Deck { cards["fireElf"] } },
                {"Kni", new Deck { cards["knight"] } },
                {"FSp", new Deck { cards["fireSpell"] } },
                {"WSp", new Deck { cards["waterSpell"] } },
                {"Wiz", new Deck { cards["wizzard"] } },
                {"Ork", new Deck { cards["ork"] } },
                {"Kra", new Deck { cards["kraken"] } }
            };

        }

        //Goblins are too afraid of Dragons to attack.
        //Wizzard can control Orks so they are not able to damage them.
        //The armor of Knights is so heavy that WaterSpells make them drown them instantly.
        //The Kraken is immune against spells.
        //The FireElves know Dragons since they were little and can evade their attacks. 

        [TestCase("Gob", "Dra", 2)]
        [TestCase("Dra", "Gob", 1)]
        [TestCase("Wiz", "Ork", 1)]
        [TestCase("Kni", "WSp", 2)]
        [TestCase("Kni", "FSp", 1)]
        [TestCase("Kra", "WSp", 1)]
        [TestCase("Kra", "FSp", 1)]
        [TestCase("Dra", "FEl", 2)]
        public void TestBattle(string deckA, string deckB, int result)
            => Assert.That(Battle.Fight(decks[deckA], decks[deckB]), Is.EqualTo(result));
    }
}