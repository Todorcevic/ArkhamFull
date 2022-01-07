using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ArkhamShared;

namespace ArkhamGamePlay
{
    public class Card01063 : CardAsset
    {
        protected override void BeginGameAction(GameAction gameAction)
        {
            base.BeginGameAction(gameAction);
            if (gameAction is InteractableAction interactableAction && CheckPlayCardInGame(interactableAction))
                interactableAction.CardEffects.Add(new CardEffect(
                    card: ThisCard,
                    effect: SearchSpell,
                    animationEffect: SearchSpellAnimation,
                    needExhaust: true,
                    type: EffectType.Instead,
                    name: "Activar " + ThisCard.Info.Name,
                    investigatorImageCardInfoOwner: ThisCard.VisualOwner)
                    );
        }

        bool CheckPlayCardInGame(InteractableAction interactableAction)
        {
            if (!interactableAction.CanPlayFastAction) return false;
            if (!ThisCard.IsInPlay) return false;
            return true;
        }

        IEnumerator SearchSpellAnimation() => new AnimationCardAction(ThisCard, audioClip: ThisCard.Effect1).RunNow();

        IEnumerator SearchSpell()
        {
            List<CardEffect> cardEffects = new List<CardEffect>();
            int deckAmount = ThisCard.VisualOwner.InvestigatorDeck.ListCards.Count;
            List<CardComponent> threeCards = Enumerable.Reverse(ThisCard.VisualOwner.InvestigatorDeck.ListCards).ToList().GetRange(0, deckAmount < 3 ? deckAmount : 3);

            foreach (CardComponent card in threeCards)
            {
                card.TurnDown(false);
                card.MoveTo(AllComponents.Table.CenterPreview);
            }
            yield return new WaitWhile(() => DOTween.IsTweening("MoveTo"));
            yield return new WaitForSeconds(GameData.ANIMATION_TIME_DEFAULT * 4);
            yield return new MoveDeck(threeCards, ThisCard.VisualOwner.InvestigatorDeck).RunNow();

            foreach (CardComponent spell in threeCards.FindAll(c => c.KeyWords.Contains("Spell")))
                cardEffects.Add(new CardEffect(
                    card: spell,
                    effect: () => SpellChoose(spell),
                    animationEffect: () => null,
                    type: EffectType.Choose,
                    name: "Robar " + spell.Info.Name,
                    investigatorImageCardInfoOwner: ThisCard.VisualOwner));
            yield return new ChooseCardAction(cardEffects, isOptionalChoice: false).RunNow();
            yield return new MoveDeck(threeCards, ThisCard.VisualOwner.InvestigatorDeck, isBack: true, withMoveUp: true).RunNow();
            yield return new ShuffleAction(ThisCard.VisualOwner.InvestigatorDeck).RunNow();

            IEnumerator SpellChoose(CardComponent spell)
            {
                yield return new MoveCardAction(spell, ThisCard.VisualOwner.Hand, withPreview: false).RunNow();
                threeCards.Remove(spell);
            }
        }

        protected override IEnumerator PlayCardFromHand()
        {
            yield return base.PlayCardFromHand();
            yield return new AddTokenAction(ThisCard.DoomToken, 1).RunNow();
        }
    }
}