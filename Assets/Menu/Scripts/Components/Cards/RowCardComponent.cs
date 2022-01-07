using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.Linq;
using Zenject;
using ArkhamShared;

namespace ArkhamMenu
{
    public class RowCardComponent : CardBaseComponent
    {
        public TextMeshProUGUI cardNameComponent;
        public override string ID
        {
            get => base.ID;
            set
            {
                base.ID = value;
                cardNameComponent.text = Card.DataCardDictionary[value].Name;
            }
        }

        /********************************************************************************/
        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            auraComponent.DOFillAmount(1f, 0.25f);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            DeckBuildManager.HidePreviewCard(this);
            auraComponent.DOFillAmount(0f, 0.25f).OnComplete(DeSelect);
        }

        void DeSelect() => IsSelected = false;

        public override void DoubleClickCard(CardBaseComponent cardBase)
        {
            if (!IsDisable) DeckBuildManager.DechooseCard(this);
        }

        public void PrintQuantity(InvestigatorSelectorComponent investigator)
        {
            int quantity = investigator.CurrentInvestigator.FullListCards.FindAll(c => c.Contains(ID)).Count;
            quantityText.text = quantity == 1 ? "" : "x" + quantity;
        }
    }
}
