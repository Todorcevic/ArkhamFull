using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

namespace ArkhamGamePlay
{
    public class CardEvent : CardLogic
    {
        protected CardEffect playFromHand;
        public event Action<CardEvent> OnBeginCanBePlayedFromHandEvent;
        public event Action<CardEvent> OnEndCanBePlayedFromHandEvent;
        protected virtual bool IsFast => false;
        protected virtual bool NeedPlayZone => true;
        protected virtual string nameCardEffect => "Jugar " + ThisCard.Info.Name;
        protected virtual CardEffect PlayFromHand => new CardEffect(
                    card: ThisCard,
                    effect: PlayCardFromHand,
                    animationEffect: PlayCardAnimation,
                    payEffect: PayCostEffect,
                    cancelEffect: ()=>CancelEffect(ThisCard.VisualOwner),
                    checkFilterToCancel: CheckFilterToCancel,
                    type: IsFast ? EffectType.Fast : EffectType.Play,
                    name: nameCardEffect,
                    actionCost: IsFast ? 0 : 1,
                    investigatorImageCardInfoOwner: ThisCard.VisualOwner,
                    resourceCost: ThisCard.Info.Cost ?? 0
                    );
        protected virtual Zone ZoneToPlayFromHand => AllComponents.Table.PlayCardZone;

        /*****************************************************************************************/
        protected override void BeginGameAction(GameAction gameAction)
        {
            if (IsFast && gameAction is InteractableAction interactableAction && PlayingCard(interactableAction))
                interactableAction.CardEffects.Add(playFromHand = PlayFromHand);
            if (!IsFast && gameAction is InvestigatorTurn investigatorturn && PlayingCard(investigatorturn))
                investigatorturn.CardEffects.Add(playFromHand = PlayFromHand);
        }

        protected virtual bool PlayingCard(InteractableAction interactableAction)
        {
            if (GameControl.ActiveInvestigator != ThisCard.VisualOwner) return false;
            if (!interactableAction.CanPlayFastAction) return false;
            if (!CheckPlayedFromHand()) return false;
            return true;
        }

        protected virtual bool PlayingCard(InvestigatorTurn investigatorturn)
        {
            if (GameControl.ActiveInvestigator != ThisCard.VisualOwner) return false;
            if (!CheckPlayedFromHand()) return false;
            return true;
        }

        protected virtual bool CheckPlayedFromHand()
        {
            OnBeginCanBePlayedFromHandEvent?.Invoke(this); //Necessary for Card01014
            if (ThisCard.CurrentZone != ThisCard.VisualOwner?.Hand) return false;
            OnEndCanBePlayedFromHandEvent?.Invoke(this);
            return true;
        }

        protected virtual IEnumerator PlayCardFromHand()
        {
            yield return new MoveCardAction(ThisCard, ZoneToPlayFromHand, withPreview: false).RunNow();
            yield return new EffectAction(LogicEffect).RunNow();
            if (!playFromHand?.IsCancel ?? false)
                yield return new DiscardAction(ThisCard, withPreview: false, withTopUp: true).RunNow();
        }

        protected virtual IEnumerator PlayCardAnimation() => new AnimationCardAction(ThisCard, withReturn: false, audioClip: ThisCard.Effect1).RunNow();

        protected virtual IEnumerator PayCostEffect() => null;

        protected virtual IEnumerator LogicEffect() => null;

        protected virtual IEnumerator CancelEffect(InvestigatorComponent investigator) =>
            new MoveCardAction(ThisCard, investigator.Hand, withPreview: false, isFast: true).RunNow();

        protected virtual bool CheckFilterToCancel() => false;
    }
}