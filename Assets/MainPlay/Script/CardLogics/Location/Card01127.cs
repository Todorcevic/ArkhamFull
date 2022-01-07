using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace ArkhamGamePlay
{
    public class Card01127 : CardLocation
    {
        List<InvestigatorComponent> investigatorsUsed = new List<InvestigatorComponent>();
        public override LocationSymbol MySymbol => LocationSymbol.Square;
        public override LocationSymbol MovePosibilities => LocationSymbol.Diamond | LocationSymbol.Plus | LocationSymbol.Circle;

        /*****************************************************************************************/
        protected override void BeginGameAction(GameAction gameAction)
        {
            base.BeginGameAction(gameAction);

            if (gameAction is InvestigatorTurn investigatorTurn && CheckCanUse(investigatorTurn))
                investigatorTurn.CardEffects.Add(new CardEffect(
                    card: ThisCard,
                    effect: () => SearchAlly(investigatorTurn),
                    animationEffect: SearchAllyAnimation,
                    type: EffectType.Activate,
                    name: "Buscar aliado",
                    actionCost: 1,
                    investigatorImageCardInfoOwner: investigatorTurn.ActiveInvestigator));
        }

        bool CheckCanUse(InvestigatorTurn investigatorTurn)
        {
            if (investigatorTurn.ActiveInvestigator.CurrentLocation != ThisCard) return false;
            if (investigatorsUsed.Contains(investigatorTurn.ActiveInvestigator)) return false;
            return true;
        }

        IEnumerator SearchAllyAnimation() => new AnimationCardAction(ThisCard, audioClip: ThisCard.Effect1).RunNow();

        IEnumerator SearchAlly(InvestigatorTurn investigatorTurn)
        {
            investigatorsUsed.Add(investigatorTurn.ActiveInvestigator);
            List<CardEffect> allysToChoose = new List<CardEffect>();
            List<CardComponent> allys = investigatorTurn.ActiveInvestigator.InvestigatorDeck.ListCards.FindAll(c => c.KeyWords.Contains("Ally"));
            foreach (CardComponent ally in allys)
            {
                yield return new TurnCardAction(ally, isBack: false).RunNow();
                allysToChoose.Add(new CardEffect(
                    card: ally,
                    effect: () => AddAlly(ally),
                    animationEffect: () => null,
                    type: EffectType.Choose,
                    name: "Obtener " + ally.Info.Name
                    )); ;
            }
            yield return new ChooseCardAction(allysToChoose, isOptionalChoice: false).RunNow();
            yield return new MoveDeck(allys, investigatorTurn.ActiveInvestigator.InvestigatorDeck, isBack: true, withMoveUp: true).RunNow();
            yield return new ShuffleAction(investigatorTurn.ActiveInvestigator.InvestigatorDeck).RunNow();

            IEnumerator AddAlly(CardComponent card)
            {
                yield return new MoveCardAction(card, investigatorTurn.ActiveInvestigator.Hand, withPreview: false).RunNow(); //Probably add to hand not is draw
                allys.Remove(card);
            }
        }
    }
}
