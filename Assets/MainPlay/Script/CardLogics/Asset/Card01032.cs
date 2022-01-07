using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01032 : CardAsset
    {
        protected override void EndGameAction(GameAction gameAction)
        {
            base.EndGameAction(gameAction);
            if (gameAction is MoveCardAction moveCardAction && ReactionEffect(moveCardAction))
                moveCardAction.ChooseCardOptionalAction.ChosenCardEffects.Add(new CardEffect(
                    card: ThisCard,
                    effect: SearchTome,
                    animationEffect: SearchTomeAnimation,
                    type: EffectType.Reaction,
                    name: "Buscar tomo",
                    investigatorImageCardInfoOwner: ThisCard.VisualOwner)
                    );
        }

        bool ReactionEffect(MoveCardAction moveCardAction)
        {
            if (moveCardAction.ThisCard != ThisCard) return false;
            if (moveCardAction.Zone != ThisCard.VisualOwner.Assets) return false;
            return true;
        }

        IEnumerator SearchTomeAnimation() =>
            new AnimationCardAction(ThisCard, audioClip: ThisCard.Effect1).RunNow();

        IEnumerator SearchTome()
        {
            List<CardEffect> cardEffects = new List<CardEffect>();
            List<CardComponent> allTomes = ThisCard.VisualOwner.InvestigatorDeck.ListCards.FindAll(c => c.CardType == CardType.Asset && c.KeyWords.Contains("Tome"));
            foreach (CardComponent tome in allTomes)
            {
                yield return new TurnCardAction(tome, isBack: false).RunNow();
                cardEffects.Add(new CardEffect(
                    card: tome,
                    effect: () => AddTome(tome),
                    animationEffect: () => null,
                    type: EffectType.Choose,
                    name: "Obtener " + tome.Info.Name,
                    investigatorImageCardInfoOwner: ThisCard.VisualOwner));
            }
            yield return new ChooseCardAction(cardEffects, isOptionalChoice: false).RunNow();
            yield return new MoveDeck(allTomes, ThisCard.VisualOwner.InvestigatorDeck, isBack: true, withMoveUp: true).RunNow();
            yield return new ShuffleAction(ThisCard.VisualOwner.InvestigatorDeck).RunNow();

            IEnumerator AddTome(CardComponent card)
            {
                yield return new MoveCardAction(card, ThisCard.VisualOwner.Hand, withPreview: false).RunNow(); //Probably add to hand not is draw
                allTomes.Remove(card);
            }
        }
    }
}