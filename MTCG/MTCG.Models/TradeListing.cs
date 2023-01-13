namespace MTCG.Models
{
    public class TradeListing
    {
        public string CardId { get; set; }
        public long? CoinRequirement { get; set; }
        public Card.CardType? CardTypeRequirement { get; set; }
        public int? MinimumDamageRequirement { get; set; }
        public string? CustomRequirements { get; set; }
    }
}
