using MTCG.Models;
using MTCG.Models.Components;

namespace MTCG.BL
{
    public static class Battle
    {
        static long Attack(Card attacker, Card defender)
        {
            foreach (var comp in attacker.Components)
            {
                if (comp is WeakAgainstComponent)
                {
                    var weakness = (WeakAgainstComponent)comp;

                    if (weakness.Name != null && weakness.Name != defender.Name) continue;
                    if (weakness.Element != null && weakness.Element != defender.Element) continue;

                    Console.WriteLine($"{attacker} is weak against {defender}! Loses!");
                    return 0;
                }
            }

            foreach (var comp in defender.Components)
            {
                if (comp is ImmuneToComponent)
                {
                    var immunity = (ImmuneToComponent)comp;

                    if (immunity.Name != null && immunity.Name != attacker.Name) continue;
                    if (immunity.Element != null && immunity.Element != attacker.Element) continue;
                    
                    Console.WriteLine($"{attacker} cannot damage {defender}! Loses!");
                    return 0;
                }
            }

            long elementModifiedDamage = attacker.Damage;

            if (attacker.Type == Card.CardType.Spell || defender.Type == Card.CardType.Spell)
                switch (attacker.Element)
                {
                    case Card.ElementType.Normal:
                        if (defender.Element == Card.ElementType.Water) elementModifiedDamage *= 2;
                        else if (defender.Element == Card.ElementType.Fire) elementModifiedDamage /= 2;
                        break;
                    case Card.ElementType.Fire:
                        if (defender.Element == Card.ElementType.Normal) elementModifiedDamage *= 2;
                        else if (defender.Element == Card.ElementType.Water) elementModifiedDamage /= 2;
                        break;
                    case Card.ElementType.Water:
                        if (defender.Element == Card.ElementType.Fire) elementModifiedDamage *= 2;
                        else if (defender.Element == Card.ElementType.Normal) elementModifiedDamage /= 2;
                        break;
                }

            return elementModifiedDamage;
        }

        public static int Fight(Deck deckA, Deck deckB)
        {
            var rand = new Random();
            var a = deckA[rand.Next(deckA.Count)];
            var b = deckB[rand.Next(deckB.Count)];

            Console.WriteLine($"Pulled Cards:\n\tA:{a}\n\tB:{b}");

            if (a == null && b == null)
            {
                Console.WriteLine("Draw!");
                return 0;
            }

            if (a == null || b == null)
            {
                Console.WriteLine(
                    a == null ?
                    "B won because a has no more cards in deck!" :
                    "A won because b has no more cards in deck!");
                return a == null ? 2 : 1;
            }

            Console.WriteLine("First iteration of combat");
            var fight_result_1 = Attack(a, b);
            Console.WriteLine("Second iteration of combat");
            var fight_result_2 = Attack(b, a);

            Console.WriteLine($"result a->b: {fight_result_1}\nresult b->a: {fight_result_2}");

            if (fight_result_1 == fight_result_2) return 0;

            return fight_result_1 > fight_result_2 ? 1 : 2;
        }
    }
}
