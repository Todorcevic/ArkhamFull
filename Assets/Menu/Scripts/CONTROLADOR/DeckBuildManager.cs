using UnityEngine;
using DG.Tweening;
using Zenject;
using ArkhamShared;

namespace ArkhamMenu
{
    public class DeckBuildManager
    {
        CheckReady check;
        ViewCardManager viewCardManager;
        PanelManager panelManager;
        InvestigatorSelectorComponent investigatorSelector;
        PreviewComponent previewComponent;
        AudioComponent audioComponent;

        [Inject]
        public void Constructor(
            CheckReady check,
            ViewCardManager viewCardManager,
            PanelManager panelManager,
            InvestigatorSelectorComponent investigatorSelector,
            PreviewComponent previewComponent,
            AudioComponent audioComponent)
        {
            this.check = check;
            this.viewCardManager = viewCardManager;
            this.panelManager = panelManager;
            this.investigatorSelector = investigatorSelector;
            this.previewComponent = previewComponent;
            this.audioComponent = audioComponent;
        }

        public void ShowingPreviewCard(CardBaseComponent card)
        {
            audioComponent.PlayHover();
            previewComponent.ShowingPreviewCard(card);
        }

        public void HidePreviewCard(CardBaseComponent card)
        {
            previewComponent.HidePreviewCard(card);
        }

        public void ChooseInvestigatorCard(CardBaseComponent card)
        {
            if (investigatorSelector.InvestigatorQuantity >= GameData.MAX_INVESTIGATORS) return;
            previewComponent.ChooseInvestigator(card);
            audioComponent.PlayClick();
            investigatorSelector.AddInvestigator(card.ID);
            viewCardManager.ShowInvestigatorCards();
            viewCardManager.ShowListSelectedCards(investigatorSelector.CurrentInvestigator);
            check.CheckingAllReady();
        }
        public void DeChooseInvestigatorCard(InvNCardComponent card)
        {
            if (PanelManager.IsInInvestigatorPanel)
            {
                audioComponent.PlayClick();
                if (GameData.Instance.IsNotFirstScenario) card.CallRetireModal();
                else investigatorSelector.RemoveInvestigator(card);
            }
            check.CheckingAllReady();
        }

        public void InvestigatorSelect(InvNCardComponent invCard)
        {
            audioComponent.PlayHover();
            investigatorSelector.CurrentInvestigator = invCard;
            viewCardManager.ShowDeckCards(invCard);
            viewCardManager.ScrollBar.value = 1;
        }

        public void ChooseCard(CardBaseComponent card)
        {
            previewComponent.ChooseCard(card);
            audioComponent.PlayClick();
            investigatorSelector.CurrentInvestigator.UpdateListAdd_Remove(card, isAdd: true);
            viewCardManager.ShowDeckCards(investigatorSelector.CurrentInvestigator);
            check.CheckingAllReady();
        }

        public void DechooseCard(CardBaseComponent card)
        {
            CardBaseComponent cardInDeckCardsZone = Zones.dictionaryZones["DeckCards"].zoneCards.Find(c => c.ID == card.ID);
            previewComponent.DeselectCard(cardInDeckCardsZone);
            audioComponent.PlayClick();
            investigatorSelector.CurrentInvestigator.UpdateListAdd_Remove(card, isAdd: false);
            viewCardManager.ShowDeckCards(investigatorSelector.CurrentInvestigator);
            check.CheckingAllReady();
        }
    }
}
