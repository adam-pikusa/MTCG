namespace MTCG.Models
{
    public class Package
    {
        public long Cost { get; }
        public List<Card> Content { get; } = new ();

        public Package(long cost, List<Card> content)
        {
            Cost = cost;
            Content = content;
        }
    }
}
