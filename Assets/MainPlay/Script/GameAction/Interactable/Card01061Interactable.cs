using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

namespace ArkhamGamePlay
{
    public class Card01061Interactable : InteractableCenterShow
    {
        readonly Zone deckZone;

        /*****************************************************************************************/
        public Card01061Interactable(List<CardEffect> cardEffects, Zone deckZone)
        {
            ChosenCardEffects = cardEffects;
            this.deckZone = deckZone;
            CardsInPreview = false;
        }

        /*****************************************************************************************/
        protected override IEnumerator ActionLogic()
        {
            if (!CardsInPreview) yield return ShowPreviewCards();
            yield return base.ActionLogic();
        }

        protected override void MoveToCenter(CardComponent card)
        {
            card.MoveFast(AllComponents.Table.CenterPreview);
            AllComponents.Table.CenterPreview.ListCards.Add(card);
        }

        protected override void MoveToOriginalzone(CardComponent card)
        {
            card.MoveFast(deckZone);
        }

        public override void CardPlay(CardComponent card)
        {
            PreparingCard(card, false);
            if (ChosenCardEffects.Count < 2)
                ContinueNextAction = true;
            base.CardPlay(card);
            ChosenCardEffects.Remove(ChosenCardEffects.Find(c => c.Card == card));
        }

        public override void ReadyClicked(ReadyButton button)
        {
            if (cardSelected != null) CardPlay(cardSelected);
            AnyIsClicked = true;
        }

        public override void SetButton() =>
            AllComponents.ReadyButton.SetReadyButton(this, state: ButtonState.Off);

        protected override void PlayableCards()
        {
            CardEffects.AddRange(ChosenCardEffects);
            if (CardEffects.Count < 1) AnyIsClicked = true;
        }
    }
}