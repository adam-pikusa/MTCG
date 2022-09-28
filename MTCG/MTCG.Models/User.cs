namespace MTCG.Models
{
    public class User
    {
        public Credentials Credentials { get; set; }

        public long Coins { get; set; }

        public List<Card> Stack { get; set; } = new();
    }
}
