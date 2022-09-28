namespace MTCG.Models.Components
{
    public class ImmuneToComponent : Component
    {
        public string? Name { get; private set; }

        public override Component deserializeFromJsonObject(dynamic jsonObject)
        {
            Name = (string)jsonObject["name"];
            return this;
        }

        public override string ToString() => $"ImmuneTo({Name})";
    }
}
