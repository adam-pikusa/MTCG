namespace MTCG.Models
{
    public record UserStats
    {
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Elo { get; set; }
    }
}
