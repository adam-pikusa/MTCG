using Newtonsoft.Json;

namespace MTCG.Models.Components
{
    public class CritComponent : Component
    {
        [JsonProperty]
        const string component_type = nameof(CritComponent);
        public double chance { get; private set; }

        public override Component DeserializeFromJsonObject(dynamic jsonObject)
        {
            chance = (double)jsonObject["chance"];
            return this;
        }

        public override string ToString() => $"CritComponent({chance})";
    }
}
