using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ArkhamShared;

namespace ArkhamGamePlay
{
    public class Card01061 : CardAsset
    {
        CardEffect cardEffect;

        /*****************************************************************************************/
        protected override void BeginGameAction(GameAction gameAction)
        {
            base.BeginGameAction(gameAction);
            if (gameAction is InvestigatorTurn investigatorTurn && CheckActiveEffect(investigatorTurn))
                investigatorTurn.CardEffects.Add(cardEffect = new CardEffect(
                    card: ThisCard,
                    effect: SearchDeck,
                    animationEffect: SearchDeckAnimation,
                    payEffect: PayCostEffect,
                    cancelEffect: CancelEffect,
                    type: EffectType.Activate,
                    name: "Activar " + ThisCard.Info.Name,
                    actionCost: 1,
                    needExhaust: true,
                    investigatorImageCardInfoOwner: ThisCard.VisualOwner)
                    );
        }

        bool CheckActiveEffect(InvestigatorTurn investigatorTurn)
        {
            if (ThisCard.CurrentZone != investigatorTurn.ActiveInvestigator.Assets) return false;
            if (ThisCard.ResourcesToken.Amount < 1) return false;
            return true;
        }

        IEnumerator SearchDeckAnimation() => new AnimationCardAction(ThisCard, audioClip: ThisCard.Effect1).RunNow();

        IEnumerator SearchDeck()
        {
            List<CardEffect> decksToSearch = new List<CardEffect>();
            foreach (CardComponent playCard in GameControl.AllInvestigatorsInGame.Select(i => i.PlayCard))
                decksToSearch.Add(new CardEffect(
                    card: playCard,
                    effect: () => Searching(playCard.Owner.InvestigatorDeck),
                    animationEffect: ((CardInvestigator)playCard.Owner.InvestigatorCardComponent.CardLogic).ThankYouAnimation,
                    type: EffectType.Choose,
                    name: "Mirar el mazo de " + playCard.Info.Name,
                    investigatorImageCardInfoOwner: ThisCard.VisualOwner));

            decksToSearch.Add(new CardEffect(
                card: AllComponents.Table.EncounterDeck.ListCards.Last(),
                effect: () => Searching(AllComponents.Table.EncounterDeck),
                type: EffectType.Choose,
                name: "Mirar el mazo de Encuentros",
                investigatorImageCardInfoOwner: ThisCard.VisualOwner));
            yield return new ChooseCardAction(decksToSearch, cancelableCardEffect: cardEffect).RunNow();

            IEnumerator Searching(Zone deckZone)
            {
                List<CardEffect> cardEffects = new List<CardEffect>();
                foreach (CardComponent card in Enumerable.Reverse(deckZone.ListCards).ToList().GetRange(0, deckZone.ListCards.Count < 3 ? deckZone.ListCards.Count : 3))
                {
                    yield return new TurnCardAction(card, false).RunNow();
                    cardEffects.Add(new CardEffect(
                        card: card,
                        effect: () => SelectCard(card),
                        type: EffectType.Choose,
                        name: "Devolver " + card.Info.Name + " al mazo",
                        investigatorImageCardInfoOwner: ThisCard.VisualOwner)
                        );
                }
                yield return new Card01061Interactable(cardEffects, deckZone).RunNow();

                IEnumerator SelectCard(CardComponent card)
                {
                    yield return card.transform.DOLocalMoveZ(0.5f, GameData.ANIMATION_TIME_DEFAULT).WaitForCompletion();
                    yield return new MoveCardAction(card, deckZone, isBack: true, withPreview: false).RunNow();
                }
            }
        }

        IEnumerator PayCostEffect() => new AddTokenAction(ThisCard.ResourcesToken, -1).RunNow();
        IEnumerator CancelEffect() => new AddTokenAction(ThisCard.ResourcesToken, 1).RunNow();

        protected override IEnumerator PlayCardFromHand()
        {
            yield return base.PlayCardFromHand();
            yield return new AddTokenAction(ThisCard.ResourcesToken, 3).RunNow();
        }
    }
}