using MTCG.BL;
using MTCG.Models;

const string assetFile = @"Assets/cards.json";

var allCards = CardDeserializer.deserializeAllCards(File.ReadAllText(assetFile));


//Goblins are too afraid of Dragons to attack.
//Wizzard can control Orks so they are not able to damage them.
//The armor of Knights is so heavy that WaterSpells make them drown them instantly.
//The Kraken is immune against spells.
//The FireElves know Dragons since they were little and can evade their attacks. 

var goblin = allCards[0];
var knight = allCards[3];
var dragon = allCards[5];
var fireElf = allCards[6];
var fireSpell = allCards[7];
var waterSpell = allCards[8];


var deckA = new Deck { goblin };
var deckB = new Deck { dragon };
var deckC = new Deck { fireElf };
var deckD = new Deck { knight };
var deckE = new Deck { fireSpell };
var deckF = new Deck { waterSpell };

Battle.Fight(deckA, deckB);
Battle.Fight(deckB, deckC);
Battle.Fight(deckD, deckE);
Battle.Fight(deckD, deckF);
