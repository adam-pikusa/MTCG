using Microsoft.Extensions.Logging;
using MTCG.Models;
using MTCG.Models.Components;

namespace MTCG.BL
{
    public class Battle
    {
        static ILogger log = Logging.Get<Battle>();
        static Random rand = new();

        static long Attack(Card attacker, Card defender)
        {
            foreach (var comp in attacker.Components)
            {
                if (comp is WeakAgainstComponent)
                {
                    var weakness = (WeakAgainstComponent)comp;

                    if (weakness.name != null && weakness.name != defender.Name) continue;
                    if (weakness.element != null && weakness.element != defender.Element) continue;

                    log.LogInformation($"{attacker} is weak against {defender}! Loses!");
                    return 0;
                }
            }

            foreach (var comp in defender.Components)
            {
                if (comp is ImmuneToComponent)
                {
                    var immunity = (ImmuneToComponent)comp;

                    if (immunity.name != null && immunity.name != attacker.Name) continue;
                    if (immunity.element != null && immunity.element != attacker.Element) continue;
                    
                    log.LogInformation($"{attacker} cannot damage {defender}! Loses!");
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

            log.LogInformation("Base damage {0} modified to {1} because of elemental interaction", attacker.Damage, elementModifiedDamage);

            return elementModifiedDamage;
        }

        public static int Fight(Deck deckA, Deck deckB)
        {
            for (int fightIteration = 0; fightIteration < 100; fightIteration++)
            {
                log.LogInformation("deck sizes: {0}-{1}", deckA.Count, deckB.Count);

                if (deckA.Count == 0 || deckB.Count == 0)
                {
                    log.LogInformation(
                        deckA.Count == 0 ?
                        "B won battle because a has no more cards in deck!" :
                        "A won battle because b has no more cards in deck!");
                    return deckA.Count == 0 ? 2 : 1;
                }

                var a = deckA[rand.Next(deckA.Count)];
                var b = deckB[rand.Next(deckB.Count)];

                log.LogInformation($"Pulled Cards:\n\tA:{a}\n\tB:{b}");

                log.LogInformation("First iteration of combat");
                var fight_result_1 = Attack(a, b);
                log.LogInformation("Second iteration of combat");
                var fight_result_2 = Attack(b, a);

                log.LogInformation($"result a->b: {fight_result_1}, result b->a: {fight_result_2}");

                if (fight_result_1 == fight_result_2)
                {
                    log.LogInformation("fight result: draw!");
                    continue;
                }

                if (fight_result_1 > fight_result_2)
                {
                    deckB.Remove(b);
                    deckA.Add(b); 
                }
                else
                {
                    deckA.Remove(a);
                    deckB.Add(a);
                }

                log.LogInformation("fight result: {0} won fight, moving {1} to winner", 
                    fight_result_1 > fight_result_2 ? "A" : "B",
                    fight_result_1 > fight_result_2 ? b.ToString() : a.ToString());
            }

            log.LogInformation("draw after 100 fight iterations!");
            return 0;
        }
    }
}
