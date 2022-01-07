using UnityEngine;
using System.Linq;
using Zenject;
using ArkhamShared;
using UnityEngine.UI;

namespace ArkhamMenu
{
    public class ViewCardManager : MonoBehaviour
    {
        InvestigatorSelectorComponent investigatorSelector;
        [SerializeField] Scrollbar scrollBar;
        public Scrollbar ScrollBar { get => scrollBar; set => scrollBar = value; }

        [Inject]
        public void Constructor(InvestigatorSelectorComponent investigatorSelector)
        {
            this.investigatorSelector = investigatorSelector;
        }

        bool isShowAll;
        public bool IsShowAll
        {
            get => isShowAll;
            set
            {
                isShowAll = value;
                if (PanelManager.IsInInvestigatorPanel)
                    ShowInvestigatorCards();
                else
                    ShowDeckCards(investigatorSelector.CurrentInvestigator);
            }
        }

        string textSearch;
        public string TextSearch
        {
            get => textSearch;
            set
            {
                textSearch = value;
                if (PanelManager.IsInInvestigatorPanel)
                    ShowInvestigatorCards();
                else
                    ShowDeckCards(investigatorSelector.CurrentInvestigator);
            }
        }

        bool CheckToDisableCard(CardBaseComponent card)
        {
            if (card.Quantity == 0) return true;
            if (card.Info.Pack_code != "core") return true;
            if (investigatorSelector.CurrentInvestigator.IsFull) return true;
            if (investigatorSelector.CurrentInvestigator.Data.Xp < card.Info.Xp) return true;
            if (investigatorSelector.CurrentInvestigator.FullListCards.FindAll(c => c == card.ID).Count >= GameData.MAX_UNIQUE_CARDS_SELECTED) return true;
            if (!investigatorSelector.CurrentInvestigator.Data.DeckBuilding.Contains(card.ID)) return true;
            return false;
        }

        bool CheckToVisibleCard(CardBaseComponent card)
        {
            if (card.ImageComponent.sprite == null) return false;
            if (!card.NameCard.Contains(textSearch ?? "")) return false;
            if (!IsShowAll && card.Info.Pack_code != "core") return false;
            return (isShowAll || investigatorSelector.CurrentInvestigator.Data.DeckBuilding.Contains(card.ID));
        }

        public void ShowDeckCards(InvNCardComponent invCard)
        {
            ShowListSelectedCards(invCard);
            invCard.SetTokens();
            foreach (CardBaseComponent card in Zones.dictionaryZones["DeckCards"].zoneCards)
            {
                card.IsDisable = CheckToDisableCard(card);
                card.IsVisible = CheckToVisibleCard(card);
                card.PrintQuantity();
            }
        }

        public void ShowListSelectedCards(InvNCardComponent invCard)
        {
            CleanSelectedCards();
            if (invCard == null) return;
            foreach (string cardID in invCard.FullListCards)
            {
                RowCardComponent card = (RowCardComponent)Zones.dictionaryZones["CardsSelected"].zoneCards.Find(c => c.ID == cardID);
                card.IsVisible = true;
                card.IsDisable = invCard.Data.CardRequeriments.Contains(card.ID) || invCard.IsPowering || (invCard.Data.IsPlaying && invCard.Data.Xp <= 0);
                card.PrintQuantity(investigatorSelector);
                SortingList(card);
            }
        }

        public void CleanSelectedCards()
        {
            foreach (CardBaseComponent card in Zones.dictionaryZones["CardsSelected"].zoneCards)
                card.IsVisible = false;
        }

        void SortingList(CardBaseComponent card)
        {
            card.transform.SetParent(Zones.dictionaryZones["OverlayerZone"].zoneTransform);
            card.transform.SetParent(Zones.dictionaryZones["CardsSelected"].zoneTransform);
        }

        public void ShowInvestigatorCards()
        {
            foreach (CardBaseComponent card in Zones.dictionaryZones["InvestigatorCards"].zoneCards)
            {
                card.IsVisible = ChecKInvestigatorCardVisivility(card);
                card.IsDisable = card.Quantity < 1 || card.Info.Pack_code != "core" || !card.InvestigatorData.CanPlay;
                card.SetTokens();
                card.CheckCanPlay();
            }
        }

        bool ChecKInvestigatorCardVisivility(CardBaseComponent card)
        {
            if (card.ImageComponent.sprite == null) return false;
            if (!card.NameCard.Contains(textSearch ?? "")) return false;
            if ((card.Info.Pack_code != "core" && !IsShowAll)) return false;
            return true;
        }
    }
}
