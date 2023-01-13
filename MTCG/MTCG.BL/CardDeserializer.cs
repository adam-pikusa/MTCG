using MTCG.Models.Components;
using MTCG.Models;
using Newtonsoft.Json;

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

        static Card DeserializeCardJsonObject(dynamic cardJsonObject)
        {
            var result = new Card(
                (string)cardJsonObject["params"][0],
                Enum.Parse<Card.CardType>((string)cardJsonObject["params"][1]),
                Enum.Parse<Card.ElementType>((string)cardJsonObject["params"][2]),
                (long)cardJsonObject["params"][3]);

            foreach (var componentJsonObject in cardJsonObject["components"])
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
                JsonConvert.DeserializeObject(jsonString));

        public static Component DeserializeComponent(string jsonString)
            => DeserializeComponentJsonObject(
                JsonConvert.DeserializeObject(jsonString));
    }
}
