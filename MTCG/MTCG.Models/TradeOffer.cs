namespace MTCG.Models
{
    public class TradeOffer
    {
        public Card Offer { get; }
        public long? Coins { get; }
        public Card? WantedCard { get; }

        public TradeOffer(Card offer, long coins)
        {
            Offer = offer;
            Coins = coins;
        }

        public TradeOffer(Card offer, Card wantedCard)
        {
            Offer = offer;
            WantedCard = wantedCard;
        }
    }
}
