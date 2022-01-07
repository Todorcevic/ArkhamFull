using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Zenject;
using ArkhamShared;

namespace ArkhamMenu
{
    public class CardFactory : MonoBehaviour
    {
        DeckBuildManager deckBuildManager;
        [SerializeField] GameObject cardPrefab;
        [SerializeField] GameObject rowCardPrefab;
        [SerializeField] InvestigatorSelectorComponent investigatorSelector;

        [Inject]
        public void Constructor(DeckBuildManager deckBuildManager)
        {
            this.deckBuildManager = deckBuildManager;
        }

        public void BuildCards()
        {
            CreatingInvestigatorCards();
            CreatingDeckCards();
            CreatingRowCards();
        }

        void CreatingInvestigatorCards()
        {
            List<string> InvestigatorsCards = Card.DataCard.Where(c => c.Type_code == "investigator")
                .OrderBy(c => c.Faction_code)
                .Select(c => c.Code).ToList();
            foreach (string card in InvestigatorsCards)
            {
                CardBaseComponent investigatorCard = InstantiateCard(card, Zones.dictionaryZones["InvestigatorCards"], cardPrefab);
                investigatorCard.IsInvestigator = true;
            }
        }

        void CreatingDeckCards()
        {
            List<string> DeckCards = Card.DataCard.Where(c => (c.Type_code == "asset"
            || c.Type_code == "event"
            || c.Type_code == "skill"))
                .OrderBy(c => c.Faction_code)
                .Select(c => c.Code).ToList();
            foreach (string card in DeckCards)
                InstantiateCard(card, Zones.dictionaryZones["DeckCards"], cardPrefab);
        }

        void CreatingRowCards()
        {
            List<string> DeckCards = Card.DataCard.Where(c => (c.Type_code == "asset"
            || c.Type_code == "event"
            || c.Type_code == "skill"
            || c.Subtype_code == "weakness"
            || c.Subtype_code == "basicweakness"))
                .Select(c => c.Code).ToList();
            foreach (string card in DeckCards)
                InstantiateCard(card, Zones.dictionaryZones["CardsSelected"], rowCardPrefab);
        }

        CardBaseComponent InstantiateCard(string idCard, Zones zone, GameObject prefab)
        {
            CardBaseComponent cardMain = Instantiate(prefab, zone.zoneTransform).GetComponent<CardBaseComponent>();
            cardMain.ID = idCard;
            cardMain.DeckBuildManager = deckBuildManager;
            cardMain.InvestigatorSelector = investigatorSelector;
            zone.zoneCards.Add(cardMain);
            return cardMain;
        }
    }
}
