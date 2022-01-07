using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;
using ArkhamShared;

namespace ArkhamGamePlay
{
    public class Card01129 : CardLocation
    {
        public override LocationSymbol MySymbol => LocationSymbol.Diamond;
        public override LocationSymbol MovePosibilities => LocationSymbol.T | LocationSymbol.Plus | LocationSymbol.Circle | LocationSymbol.Square;

        /*****************************************************************************************/
        protected override void BeginGameAction(GameAction gameAction)
        {
            base.BeginGameAction(gameAction);

            if (gameAction is InvestigatorTurn investigatorTurn && CheckCanUse(investigatorTurn))
                investigatorTurn.CardEffects.Add(new CardEffect(
                    card: ThisCard,
                    effect: () => SearchTomeOrSpell(investigatorTurn),
                    animationEffect: SearchTomeOrSpellAnimation,
                    type: EffectType.Activate,
                    name: "Buscar Tomo o Hechizo",
                    actionCost: 1,
                    investigatorImageCardInfoOwner: investigatorTurn.ActiveInvestigator));
        }

        bool CheckCanUse(InvestigatorTurn investigatorTurn)
        {
            if (investigatorTurn.ActiveInvestigator.CurrentLocation != ThisCard) return false;
            return true;
        }

        IEnumerator SearchTomeOrSpellAnimation() => new AnimationCardAction(ThisCard, audioClip: ThisCard.Effect1).RunNow();

        IEnumerator SearchTomeOrSpell(InvestigatorTurn investigatorTurn)
        {
            List<CardEffect> tomesToChoose = new List<CardEffect>();
            int deckAmount = investigatorTurn.ActiveInvestigator.InvestigatorDeck.ListCards.Count;
            List<CardComponent> sixCards = Enumerable.Reverse(investigatorTurn.ActiveInvestigator.InvestigatorDeck.ListCards).ToList().GetRange(0, deckAmount < 6 ? deckAmount : 6);

            foreach (CardComponent card in sixCards)
            {
                card.TurnDown(false);
                card.MoveTo(AllComponents.Table.CenterPreview);
            }
            yield return new WaitWhile(() => DOTween.IsTweening("MoveTo"));
            yield return new WaitForSeconds(GameData.ANIMATION_TIME_DEFAULT * 4);
            yield return new MoveDeck(sixCards, investigatorTurn.ActiveInvestigator.InvestigatorDeck).RunNow();

            foreach (CardComponent tomeOrSpell in sixCards.FindAll(c => c.KeyWords.Contains("Tome") || c.KeyWords.Contains("Spell")))
            {
                tomesToChoose.Add(new CardEffect(
                    card: tomeOrSpell,
                    effect: () => AddTomeOrSpell(tomeOrSpell),
                    animationEffect: () => null,
                    type: EffectType.Choose,
                    name: "Obtener " + tomeOrSpell.Info.Name
                    )); ;
            }
            yield return new ChooseCardAction(tomesToChoose, isOptionalChoice: false).RunNow();
            yield return new MoveDeck(sixCards, investigatorTurn.ActiveInvestigator.InvestigatorDeck, isBack: true, withMoveUp: true).RunNow();
            yield return new ShuffleAction(investigatorTurn.ActiveInvestigator.InvestigatorDeck).RunNow();

            IEnumerator AddTomeOrSpell(CardComponent card)
            {
                yield return new MoveCardAction(card, investigatorTurn.ActiveInvestigator.Hand, withPreview: false).RunNow();
                sixCards.Remove(card);
            }
        }
    }
}
