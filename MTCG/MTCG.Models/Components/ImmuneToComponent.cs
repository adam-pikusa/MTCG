using Newtonsoft.Json;

namespace MTCG.Models.Components
{
    public class ImmuneToComponent : Component
    {
        [JsonProperty]
        const string component_type = nameof(ImmuneToComponent);
        public string? name { get; private set; }
        public Card.ElementType? element { get; private set; }

        public override Component DeserializeFromJsonObject(dynamic jsonObject)
        {
            if (jsonObject.ContainsKey("name")) name = (string)jsonObject["name"];
            if (jsonObject.ContainsKey("element")) element = Enum.Parse<Card.ElementType>(jsonObject["element"]);
            return this;
        }

        public override string ToString() => $"ImmuneTo({element},{name})";
    }
}
