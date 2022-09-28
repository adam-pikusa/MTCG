using MTCG.Models;
using MTCG.Models.Components;

namespace MTCG.BL
{
    public static class Battle
    {
        public static void Fight(Deck deckA, Deck deckB)
        {
            var rand = new Random();
            var a = deckA[rand.Next(deckA.Count)];
            var b = deckB[rand.Next(deckB.Count)];

            Console.WriteLine($"Pulled Cards:\n\tA:{a}\n\tB:{b}");

            if (a == null && b == null)
            {
                Console.WriteLine("Draw!");
                return;
            }

            if (a == null || b == null)
            {
                Console.WriteLine(
                    a == null ?
                    "B won because a has no more cards in deck!" :
                    "A won because b has no more cards in deck!");
                return;
            }

            foreach (var comp in a.Components)
            {
                if (comp is WeakAgainstComponent)
                {

                }
                else if (comp is ImmuneToComponent)
                {

                }
            }

            Console.WriteLine("calculating damage...");
        }
    }
}
