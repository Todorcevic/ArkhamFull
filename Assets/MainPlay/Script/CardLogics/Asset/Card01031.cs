using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

namespace ArkhamGamePlay
{
    public class Card01031 : CardAsset
    {
        CardEffect oldBookEffect;
        List<InvestigatorComponent> InvestigatorsToChoose =>
            GameControl.AllInvestigatorsInGame.FindAll(i => i.PlayCard.CurrentZone == ThisCard.VisualOwner.PlayCard.CurrentZone);

        /*****************************************************************************************/
        protected override void BeginGameAction(GameAction gameAction)
        {
            base.BeginGameAction(gameAction);
            if (gameAction is InvestigatorTurn investigatorTurn && CheckplayOldBookEffect(investigatorTurn))
                investigatorTurn.CardEffects.Add(oldBookEffect = new CardEffect(
                    card: ThisCard,
                    effect: OldBookEffect,
                    animationEffect: OldBookEffectAnimation,
                    type: EffectType.Activate,
                    name: "Usar " + ThisCard.Info.Name,
                    actionCost: 1,
                    needExhaust: true,
                    investigatorImageCardInfoOwner: ThisCard.VisualOwner));
        }

        bool CheckplayOldBookEffect(InvestigatorTurn investigatorTurn)
        {
            if (ThisCard.CurrentZone != investigatorTurn.ActiveInvestigator.Assets) return false;
            return true;
        }

        IEnumerator OldBookEffectAnimation() => new AnimationCardAction(ThisCard, audioClip: ThisCard.Effect1).RunNow();

        IEnumerator OldBookEffect()
        {
            List<CardEffect> cardEffect = new List<CardEffect>();
            foreach (CardComponent playCard in InvestigatorsToChoose.Select(i => i.PlayCard))
                cardEffect.Add(new CardEffect(
                    card: playCard,
                    effect: () => ChooseCard(playCard),
                    animationEffect: () => ChooseCardAnimation(playCard),
                    type: EffectType.Choose,
                    name: "Seleccionar a " + playCard.Info.Name,
                    investigatorImageCardInfoOwner: ThisCard.VisualOwner));
            yield return new ChooseCardAction(cardEffect, cancelableCardEffect: oldBookEffect).RunNow();

            IEnumerator ChooseCardAnimation(CardComponent playCard) => new AnimationCardAction(playCard, audioClip: playCard.Owner.InvestigatorCardComponent.Effect6).RunNow();

            IEnumerator ChooseCard(CardComponent playCard)
            {
                List<CardEffect> cardEffect2 = new List<CardEffect>();
                int deckAmount = playCard.Owner.InvestigatorDeck.ListCards.Count;
                List<CardComponent> drawCards = Enumerable.Reverse(playCard.Owner.InvestigatorDeck.ListCards).ToList()
                    .GetRange(0, deckAmount < 3 ? deckAmount : 3);

                foreach (CardComponent card in drawCards)
                {
                    yield return new TurnCardAction(card, isBack: false).RunNow();
                    cardEffect2.Add(new CardEffect(
                        card: card,
                        effect: () => Drawing(card),
                        animationEffect: () => null,
                        type: EffectType.Choose,
                        name: "Elije quedarte con " + card.Info.Name,
                        investigatorImageCardInfoOwner: playCard.Owner));
                }
                yield return new ChooseCardAction(cardEffect2, isOptionalChoice: false).RunNow();
                yield return new MoveDeck(drawCards, playCard.Owner.InvestigatorDeck, isBack: true, withMoveUp: true).RunNow();
                yield return new ShuffleAction(playCard.Owner.InvestigatorDeck).RunNow();

                IEnumerator Drawing(CardComponent card)
                {
                    yield return new DrawAction(playCard.Owner, card).RunNow();
                    drawCards.Remove(card);
                }
            }
        }
    }
}