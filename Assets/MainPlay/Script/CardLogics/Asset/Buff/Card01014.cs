using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace ArkhamGamePlay
{
    public class Card01014 : CardAsset, IBuffable
    {
        CardComponent playableCardEvent;
        CardComponent cardAffected;
        CardEffect cardToCheck;

        /*****************************************************************************************/
        protected override void BeginGameAction(GameAction gameAction)
        {
            base.BeginGameAction(gameAction);
            if (gameAction is DiscardAction discardAction && PutTopEvent(discardAction))
                new EffectAction(() => DiscardBottom(discardAction), DiscardBottonAnimation).AddActionTo();
            if (gameAction is ExecuteCardAction executeCard && CheckEvent(executeCard))
                cardToCheck = executeCard.CardEffect;
        }

        protected override void EndGameAction(GameAction gameAction)
        {
            base.EndGameAction(gameAction);
            if (ThisCard.IsInPlay) playableCardEvent = ThisCard.VisualOwner.InvestigatorDiscard.ListCards.FindAll(c => c.CardType == CardType.Event).LastOrDefault();
            if (gameAction is ExecuteCardAction executeCard && executeCard.CardEffect == cardToCheck) cardToCheck = null;
        }

        bool PutTopEvent(DiscardAction discardAction)
        {
            if (!ThisCard.IsInPlay) return false;
            if (discardAction.ThisCard != cardToCheck?.Card) return false;
            if (discardAction.ZoneToDiscard != ThisCard.VisualOwner.InvestigatorDiscard) return false;
            if (discardAction.ThisCard.Owner != ThisCard.VisualOwner) return false;
            if (discardAction.ThisCard.CardType != CardType.Event) return false;
            return true;
        }

        bool CheckEvent(ExecuteCardAction executeCard)
        {
            if (!ThisCard.IsInPlay) return false;
            if (executeCard.CardEffect.Card.CardType != CardType.Event) return false;
            if (!executeCard.CardEffect.Type.HasFlag(EffectType.Play)
                && !executeCard.CardEffect.Type.HasFlag(EffectType.Fast)) return false;
            if (GameControl.ActiveInvestigator != ThisCard.VisualOwner) return false;
            return true;
        }

        IEnumerator DiscardBottonAnimation() =>
            new AnimationCardAction(ThisCard, audioClip: ThisCard.Effect1).RunNow();

        IEnumerator DiscardBottom(DiscardAction discardAction)
        {
            discardAction.IsActionCanceled = true;
            yield return new MoveCardAction(discardAction.ThisCard, discardAction.ThisCard.VisualOwner.InvestigatorDeck, isBack: true, withPreview: true).RunNow();
            yield return discardAction.ThisCard.VisualOwner.InvestigatorDeck.MoveToPosition(discardAction.ThisCard, 0);
        }

        bool IBuffable.ActiveBuffCondition(GameAction gameAction)
        {
            if (!ThisCard.IsInPlay) return false;
            if (playableCardEvent == null) return false;
            if (cardAffected != null && cardAffected != playableCardEvent) return false;
            return true;
        }

        void IBuffable.BuffEffect()
        {
            cardAffected = playableCardEvent;
            playableCardEvent.CardTools.ShowBuff(ThisCard);
            ((CardEvent)playableCardEvent.CardLogic).OnBeginCanBePlayedFromHandEvent += ChangeDiscardToHand;
            ((CardEvent)playableCardEvent.CardLogic).OnEndCanBePlayedFromHandEvent += ChangeHandToDiscard;
        }

        void IBuffable.DeBuffEffect()
        {
            cardAffected.CardTools.HideBuff(ThisCard.UniqueId.ToString());
            ((CardEvent)cardAffected.CardLogic).OnBeginCanBePlayedFromHandEvent -= ChangeDiscardToHand;
            ((CardEvent)cardAffected.CardLogic).OnEndCanBePlayedFromHandEvent -= ChangeHandToDiscard;
            cardAffected = null;
        }

        void ChangeDiscardToHand(CardEvent cardEvent) => cardEvent.ThisCard.CurrentZone = cardEvent.ThisCard.VisualOwner.Hand;

        void ChangeHandToDiscard(CardEvent cardEvent) => cardEvent.ThisCard.CurrentZone = cardEvent.ThisCard.VisualOwner.InvestigatorDiscard;
    }
}