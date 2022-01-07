using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;
using System;

namespace ArkhamGamePlay
{
    public class ChooseCardAction : InteractableCenterShow
    {
        readonly bool isLoop;
        protected bool isOptionalChoice;
        protected bool isFastAction;
        readonly string buttonText;
        readonly CardEffect cancelableCardEffect;
        public bool IsCancel { get; set; }
        public override bool CanPlayFastAction => isFastAction;
        public CardComponent CardPlayed { get; set; }

        /*****************************************************************************************/
        public ChooseCardAction(CardEffect cardEffects, CardEffect cancelableCardEffect)
        {
            ChosenCardEffects.Add(cardEffects);
            this.cancelableCardEffect = cancelableCardEffect;
            isOptionalChoice = cancelableCardEffect.IsCancelable;
        }

        public ChooseCardAction(List<CardEffect> cardEffects, CardEffect cancelableCardEffect)
        {
            ChosenCardEffects = cardEffects;
            this.cancelableCardEffect = cancelableCardEffect;
            isOptionalChoice = cancelableCardEffect.IsCancelable;
        }

        public ChooseCardAction(CardEffect cardEffects, bool isOptionalChoice, bool isFastAction = false)
        {
            ChosenCardEffects.Add(cardEffects);
            this.isFastAction = isFastAction;
            this.isOptionalChoice = isOptionalChoice;
        }
        public ChooseCardAction(List<CardEffect> cardEffects, bool isOptionalChoice, bool isFastAction = false, bool withPreview = true, string buttonText = null, bool isLoop = false)
        {
            ChosenCardEffects = cardEffects;
            CardsInPreview = withPreview;
            this.isFastAction = isFastAction;
            this.isOptionalChoice = isOptionalChoice;
            this.buttonText = buttonText;
            this.isLoop = isLoop;
        }

        /*****************************************************************************************/

        protected override IEnumerator ActionLogicBegin()
        {
            yield return new CardEffectsFilterAction(ChosenCardEffects).RunNow();
            yield return base.ActionLogicBegin();
            CheckAmountCard();
            if (CardsInPreview || isLoop) yield return ShowPreviewCards();
        }

        protected override IEnumerator ActionLogic()
        {
            yield return base.ActionLogic();
            if (CardsInPreview) yield return ShowTable();
            if (isLoop) ChosenCardEffects.Remove(ChosenCardEffects.Find(c => c.Card == CardPlayed));
        }

        void CheckAmountCard()
        {
            if (CardEffects.Count == 1 && !isOptionalChoice)
                CardPlay(CardEffects[0].Card);
        }

        public override void SetButton()
        {
            if (isOptionalChoice)
            {
                AllComponents.ReadyButton.SetReadyButton(this, state: ButtonState.StandBy);
                AllComponents.ReadyButton.ChangeButtonText("Cancelar");
            }
            else AllComponents.ReadyButton.SetReadyButton(this, state: ButtonState.Off);
            if (buttonText != null) AllComponents.ReadyButton.ChangeButtonText(buttonText);
        }

        protected override void PlayableCards()
        {
            ChosenCardEffects.RemoveAll(c => c.Card.IsEliminated);
            CardEffects.AddRange(ChosenCardEffects);
        }

        public override void ReadyClicked(ReadyButton button)
        {
            if (cardSelected != null) CardPlay(cardSelected);
            else
            {
                IsCancel = true;
                if (cancelableCardEffect != null) cancelableCardEffect.IsCancel = true;
            }
            AnyIsClicked = true;
        }

        public override void CardPlay(CardComponent card)
        {
            CardPlayed = card;
            if ((ChosenCardEffects.Exists(c => c.Card == card) && !isLoop) || ChosenCardEffects.Count < 2)
                ContinueNextAction = true;
            base.CardPlay(card);
        }

        protected override void MoveToOriginalzone(CardComponent card)
        {
            card.CurrentZone.ZoneBehaviour = realZone[card.CurrentZone];
            if (CardPlayed != card || !ChosenCardEffects.Find(c => c.Card == card).IsWithAnimation)
            {
                card.MoveFast(card.CurrentZone, indexPosition: card.Position);
                card.Position = null;
            }
        }
    }
}