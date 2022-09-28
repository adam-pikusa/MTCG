namespace MTCG.Models.Components
{
    public class ImmuneToComponent : Component
    {
        public string? Name { get; private set; }
        public Card.ElementType? Element { get; private set; }

        public override Component deserializeFromJsonObject(dynamic jsonObject)
        {
            if (jsonObject.ContainsKey("name")) Name = (string)jsonObject["name"];
            if (jsonObject.ContainsKey("element")) Element = Enum.Parse<Card.ElementType>(jsonObject["element"]);
            return this;
        }

        public override string ToString() => $"ImmuneTo({Element},{Name})";
    }
}
