using Michsky.UI.Dark;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ArkhamMenu
{
    public class InvestigatorSelectorComponent : MonoBehaviour
    {
        InvNCardComponent currentInvestigator;
        [SerializeField] List<InvNCardComponent> allInvN;
        [SerializeField] InvestigatorImageComponent imageComponent;
        [SerializeField] ViewCardManager viewCardManager;
        public List<InvNCardComponent> ListActiveInvCard { get; } = new List<InvNCardComponent>();
        public int InvestigatorQuantity => ListActiveInvCard.Count;
        public InvNCardComponent CurrentInvestigator
        {
            get => currentInvestigator;
            set
            {
                if (currentInvestigator != null)
                    currentInvestigator.IsSelected = false;
                if (value != null)
                {
                    value.IsSelected = true;
                    imageComponent.CurrentImage = value.ImageComponent.sprite;
                    value.UpdateTextNumberCards();
                }
                else
                    imageComponent.CurrentImage = null;
                currentInvestigator = value;
            }
        }

        public void AddInvestigator(string idCard)
        {
            InvNCardComponent invCard = TakeNextInvCard(iWantActive: false);
            invCard.SetInvCard(idCard);
            CurrentInvestigator = invCard;
            ListActiveInvCard.Add(invCard);
        }

        public void RemoveInvestigator(InvNCardComponent invCard)
        {
            ListActiveInvCard.Remove(invCard);
            invCard.CleanInvCard();
            CurrentInvestigator = TakeNextInvCard(iWantActive: true);
            viewCardManager.ShowInvestigatorCards();
            viewCardManager.ShowListSelectedCards(CurrentInvestigator);
        }

        public void RetireInvestigator(InvNCardComponent invCard)
        {
            invCard.Data.IsRetired = true;
            RemoveInvestigator(invCard);
        }

        public InvNCardComponent TakeNextInvCard(bool iWantActive)
        {
            foreach (InvNCardComponent invNCard in allInvN)
                if (invNCard.isActiveAndEnabled == iWantActive) return invNCard;
            return null;
        }

        public void CleanAllInvCard()
        {
            foreach (InvNCardComponent invCard in ListActiveInvCard)
                invCard.CleanInvCard();
            ListActiveInvCard.Clear();
            CurrentInvestigator = null;
        }
    }
}
