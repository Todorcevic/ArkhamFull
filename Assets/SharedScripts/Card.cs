using System.Collections.Generic;
using System.Linq;

namespace ArkhamShared
{
    public class Card
    {
        public string Pack_code { get; set; }
        public string Pack_name { get; set; }
        public string Type_code { get; set; }
        public string Type_name { get; set; }
        public string Subtype_code { get; set; }
        public string Subtype_name { get; set; }
        public string Faction_code { get; set; }
        public string Faction_name { get; set; }
        public int? Position { get; set; }
        public bool? Exceptional { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Real_name { get; set; }
        public string Text { get; set; }
        public string Real_text { get; set; }
        public int? Quantity { get; set; }
        public bool? Clues_fixed { get; set; }
        public bool? Health_per_investigator { get; set; }
        public int? Deck_limit { get; set; }
        public string Traits { get; set; }
        public string Real_traits { get; set; }
        public bool? Is_unique { get; set; }
        public bool? Exile { get; set; }
        public bool? Hidden { get; set; }
        public bool? Permanent { get; set; }
        public bool? Double_sided { get; set; }
        public string Url { get; set; }
        public string Subname { get; set; }
        public int? Skill_willpower { get; set; }
        public int? Skill_intellect { get; set; }
        public int? Skill_combat { get; set; }
        public int? Skill_agility { get; set; }
        public int? Health { get; set; }
        public int? Sanity { get; set; }
        public string Flavor { get; set; }
        public string Illustrator { get; set; }
        public string Back_text { get; set; }
        public string Back_flavor { get; set; }
        public string Octgn_id { get; set; }
        public string Imagesrc { get; set; }
        public string Backimagesrc { get; set; }
        public int? Cost { get; set; }
        public int? Skill_wild { get; set; }
        public string Slot { get; set; }
        public int? Xp { get; set; }
        public int? Enemy_damage { get; set; }
        public int? Enemy_horror { get; set; }
        public int? Enemy_fight { get; set; }
        public int? Enemy_evade { get; set; }
        public int? Victory { get; set; }
        public string Linked_to_code { get; set; }
        public string Linked_to_name { get; set; }
        public string Faction2_code { get; set; }
        public string Faction2_name { get; set; }
        public int? Shroud { get; set; }
        public int? Clues { get; set; }
        public string Encounter_code { get; set; }
        public string Encounter_name { get; set; }
        public int? Encounter_position { get; set; }
        public int? Spoiler { get; set; }
        public int? Doom { get; set; }
        public int? Stage { get; set; }
        public string Back_name { get; set; }

        public static Card[] DataCard { get; set; }
        public static Dictionary<string, Card> DataCardDictionary { get; set; } = new Dictionary<string, Card>();
        public static void MultiplyX2CoreSetQuantity()
        {
            var allData = DataCard.Where(c => c.Pack_code == "core"
            && (c.Type_code == "asset"
            || c.Type_code == "event"
            || c.Type_code == "skill"));

            foreach (Card card in allData)
                card.Quantity *= 2;
        }
    }
}