using MTCG.BL;
using MTCG.Models;
using MTCG.Models.Components;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Test
{
    public class CardSerializerTest
    {
        [Test]
        public void TestDeserializeComponent()
        {
            var component = new WeakAgainstComponent();
            component.DeserializeFromJsonObject(
                JObject.FromObject(
                    new 
                    { 
                        component_type = nameof(WeakAgainstComponent),
                        name = "Goblin"
                    }));

            Assert.That(component.name, Is.EqualTo("Goblin"));
            Assert.That(component.element, Is.EqualTo(null));
        }

        [Test]
        public void TestDeserializeCard() 
        {
            var card = CardDeserializer.DeserializeCard("{\"id\":\"845f0dc7-37d0-426e-994e-43fc3ac83c08\", \"params\": [\"Goblin\", \"Monster\", \"Water\", 10], \"components\":[{\"component_type\":\"WeakAgainstComponent\",\"name\":\"Dragon\"}] }");
            Assert.That(card.Name, Is.EqualTo("Goblin"));
            Assert.That(card.Type, Is.EqualTo(Card.CardType.Monster));
            Assert.That(card.Element, Is.EqualTo(Card.ElementType.Water));
            Assert.That(card.Components.Count, Is.EqualTo(1));
            Assert.That(card.Components[0], Is.TypeOf<WeakAgainstComponent>());
            var component = (WeakAgainstComponent)card.Components[0];
            Assert.That(component.name, Is.EqualTo("Dragon"));
            Assert.That(component.element, Is.EqualTo(null));
        }
    }
}
