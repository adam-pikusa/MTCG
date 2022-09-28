using MTCG.Models.Components;
using MTCG.Models;
using Newtonsoft.Json;

namespace MTCG.BL
{
    public static class CardDeserializer
    {
        static Card deserializeCard(dynamic cardJsonObject)
        {
            var result = new Card(
                (string)cardJsonObject["params"][0],
                Enum.Parse<Card.CardType>((string)cardJsonObject["params"][1]),
                Enum.Parse<Card.ElementType>((string)cardJsonObject["params"][2]),
                (long)cardJsonObject["params"][3]);

            foreach (var jcomp in cardJsonObject["components"])
            {
                Component? comp = null;
                switch ((string)jcomp["component_type"])
                {
                    case nameof(WeakAgainstComponent): comp = new WeakAgainstComponent(); break;
                    case nameof(ImmuneToComponent): comp = new ImmuneToComponent(); break;
                }
                if (comp != null)
                    result.Components.Add(comp.deserializeFromJsonObject(jcomp));
            }

            Console.WriteLine($"deserialized card: {result}");
            return result;
        }

        public static List<Card> deserializeAllCards(string jsonString)
        {
            var result = new List<Card>();
            dynamic data = JsonConvert.DeserializeObject(jsonString);
            foreach (var card in data) result.Add(deserializeCard(card));
            return result;
        }
    }
}
