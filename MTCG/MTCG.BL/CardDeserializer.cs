using MTCG.Models.Components;
using MTCG.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MTCG.BL
{
    public static class CardDeserializer
    {
        static Component DeserializeComponentJsonObject(dynamic componentJsonObject)
        {
            Component? result = null;
            
            switch ((string)componentJsonObject["component_type"])
            {
                case nameof(WeakAgainstComponent): return new WeakAgainstComponent(); 
                case nameof(ImmuneToComponent): return new ImmuneToComponent();
            }

            if (result != null) result.DeserializeFromJsonObject(componentJsonObject);

            return result;
        }

        static Card DeserializeCardJsonObject(JObject cardJsonObject)
        {
            Card result;
            var name = (string)cardJsonObject["params"][0];
            var type = Enum.Parse<Card.CardType>((string)cardJsonObject["params"][1]);
            var element = Enum.Parse<Card.ElementType>((string)cardJsonObject["params"][2]);

            if (cardJsonObject.TryGetValue("id", out var readId))
                result = new Card((Guid)readId, name, type, element, (long)cardJsonObject["params"][3]);
            else
                result = new Card(name, type, element, (long)cardJsonObject["params"][3]);

            if (cardJsonObject.TryGetValue("components", out var components))
                foreach (var componentJsonObject in components)
                    result.Components.Add(DeserializeComponentJsonObject(componentJsonObject));

            Console.WriteLine($"deserialized card: {result}");
            return result;
        }

        public static List<Card> DeserializeCardsArray(string jsonString)
        {
            var result = new List<Card>();
            dynamic data = JsonConvert.DeserializeObject(jsonString);
            foreach (var card in data) result.Add(DeserializeCardJsonObject(card));
            return result;
        }

        public static Card DeserializeCard(string jsonString)
            => DeserializeCardJsonObject(
                JsonConvert.DeserializeObject<JObject>(jsonString));

        public static Component DeserializeComponent(string jsonString)
            => DeserializeComponentJsonObject(
                JsonConvert.DeserializeObject(jsonString));
    }
}
