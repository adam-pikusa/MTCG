using Newtonsoft.Json;

namespace MTCG.Models.Components
{
    public class WeakAgainstComponent : Component
    {
        [JsonProperty]
        const string ComponentType = nameof(WeakAgainstComponent);
        public string? Name { get; private set; }
        public Card.ElementType? Element { get; private set; }

        public override Component deserializeFromJsonObject(dynamic jsonObject)
        {
            if (jsonObject.ContainsKey("name")) Name = (string)jsonObject["name"];
            if (jsonObject.ContainsKey("element")) Element = Enum.Parse<Card.ElementType>((string)jsonObject["element"]);
            return this;
        }

        public override string ToString() => $"WeakAgainst({Element},{Name})";
    }
}
