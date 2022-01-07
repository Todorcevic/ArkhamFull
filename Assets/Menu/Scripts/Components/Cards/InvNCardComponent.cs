using System.Collections.Generic;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine;
using DG.Tweening;
using ArkhamShared;
using System;
using Michsky.UI.Dark;

namespace ArkhamMenu
{
    public class InvNCardComponent : CardBaseComponent
    {
        [SerializeField] RetireInvestigatorModal retireInvestigatorModal;
        [SerializeField] TextMeshProUGUI numberCardsSelected;
        public bool IsActive { get; set; }
        public bool IsPowering { get; set; }
        public bool IsFull => ListCardsID.Count >= Data.DeckSize;
        public InvestigatorData Data => InvestigatorData.AllInvestigatorsData.Find(d => d.Id == ID);
        public List<string> FullListCards => Data.FullListCards;
        public List<string> ListCardsID => Data.ListCardsID;

        public void SetInvCard(string idCard)
        {
            ID = idCard;
            ImageComponent.enabled = true;
            gameObject.SetActive(true);
            IsActive = true;
            SetTokens();
        }

        public void CleanInvCard()
        {
            ListCardsID.Clear();
            ImageComponent.enabled = false;
            gameObject.SetActive(false);
            IsActive = false;
            UpdateTextNumberCards();
            SetTokens(isRemove: true);
        }

        public void UpdateListAdd_Remove(CardBaseComponent card, bool isAdd)
        {
            if (isAdd)
            {
                if (Data.IsPlaying)
                {
                    Data.Xp -= (card.Info.Xp ?? 0) == 0 ? 1 : card.Info.Xp ?? 1;
                    IsPowering = false;
                    SetTokens();
                }
                ListCardsID.Add(card.ID);
            }
            else
            {
                if (Data.IsPlaying) IsPowering = true;
                ListCardsID.Remove(card.ID);
            }
            Zones.dictionaryZones["DeckCards"].zoneCards.Find(c => c.ID == card.ID).PrintQuantity();
            UpdateTextNumberCards();
        }

        public void UpdateTextNumberCards() =>
            numberCardsSelected.SetText((ListCardsID.Count > 0) ? $"({ListCardsID.Count}/{Data.DeckSize})" : "");

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (!IsDisable && DoubleClickCheck.DoubleClicked(eventData.clickTime, eventData.pointerPress))
                DoubleClickCard(this);
            else DeckBuildManager.InvestigatorSelect(this);
        }

        public override void DoubleClickCard(CardBaseComponent cardBase)
        {
            DeckBuildManager.DeChooseInvestigatorCard(this);
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            transform.DOScale(1.1f, 0.25f);
            DeckBuildManager.ShowingPreviewCard(this);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            transform.DOScale(1f, 0.25f);
            DeckBuildManager.HidePreviewCard(this);
        }

        public void CallRetireModal()
        {
            retireInvestigatorModal.SetInvestigator(this);
        }
    }
}
