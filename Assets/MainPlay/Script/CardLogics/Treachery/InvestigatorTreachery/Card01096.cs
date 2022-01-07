using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace ArkhamGamePlay
{
    public class Card01096 : InvestigatorTreachery
    {
        public override IEnumerator Revelation()
        {
            List<CardComponent> cardsToDiscard = Investigator.Hand.ListCards.FindAll(c => !c.IsWeakness && c.CanBeDiscard);
            List<CardEffect> cardEffects = new List<CardEffect>();
            foreach (CardComponent card in cardsToDiscard)
            {
                cardEffects.Add(new CardEffect(
                    card: card,
                    effect: () => DiscardCards(card, cardsToDiscard),
                    type: EffectType.Choose,
                    name: "Quedarte " + card.Info.Name + " en la mano"
                    ));
            }
            yield return new ChooseCardAction(cardEffects, isOptionalChoice: false).RunNow();
        }

        IEnumerator DiscardCards(CardComponent cardToHand, List<CardComponent> cardsToDiscard)
        {
            cardToHand.MoveFast(cardToHand.CurrentZone);
            cardsToDiscard.Remove(cardToHand);
            foreach (CardComponent card in cardsToDiscard)
                yield return new DiscardAction(card, withTopUp: false, isFast: true).RunNow();
            yield return new WaitWhile(() => DOTween.IsTweening("MoveCard"));
        }
    }
}