using MTCG.Models.Components;
using System.Text;

namespace MTCG.Models
{
    public class Card
    {
        public enum CardType
        {
            Spell,
            Monster
        }

        public enum ElementType
        {
            Normal,
            Water,
            Fire
        }

        public Guid? Guid { get; }
        public string Name { get; }
        public CardType Type { get; }
        public ElementType Element { get; }
        public long Damage { get; }
        public List<Component> Components { get; set; } = new();

        public Card(string name, CardType type, ElementType element, long damage)
        {
            Name = name;
            Type = type;
            Element = element;
            Damage = damage;
        }

        public Card(Guid guid, string name, CardType type, ElementType element, long damage)
        {
            Guid = guid;
            Name = name;
            Type = type;
            Element = element;
            Damage = damage;
        }

        public override string ToString()
        {
            var result = new StringBuilder($"[Card:{Guid},{Name},{Type},{Element},{Damage},");
            foreach (var comp in Components) result.Append($"{comp},");
            result.Append(']');
            return result.ToString();
        }
    }
}
