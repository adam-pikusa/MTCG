using MTCG.Models;

namespace MTCG.BL
{
    public class Marketplace
    {
        List<TradeOffer> offers = new();

        public List<TradeOffer> GetAvailableOffers() => offers;

        public List<Package> GetAvailablePackages()
            => new List<Package>
        { 
            new Package(24, new List<Card>
            {
                new Card("Goblin", Card.CardType.Monster, Card.ElementType.Normal, 50)
            })
        };
        
    }
}
