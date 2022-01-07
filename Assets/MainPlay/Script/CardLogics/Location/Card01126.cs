using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01126 : CardLocation
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
                    effect: () => DrawThreeCards(investigatorTurn),
                    animationEffect: DrawThreeCardsAnimation,
                    type: EffectType.Activate,
                    name: "Robar cartas",
                    actionCost: 1,
                    investigatorImageCardInfoOwner: investigatorTurn.ActiveInvestigator));
        }

        bool CheckCanUse(InvestigatorTurn investigatorTurn)
        {
            if (investigatorTurn.ActiveInvestigator.CurrentLocation != ThisCard) return false;
            if (investigatorsUsed.Contains(investigatorTurn.ActiveInvestigator)) return false;
            return true;
        }

        IEnumerator DrawThreeCardsAnimation() => new AnimationCardAction(ThisCard, audioClip: ThisCard.Effect1).RunNow();

        IEnumerator DrawThreeCards(InvestigatorTurn investigatorTurn)
        {
            investigatorsUsed.Add(investigatorTurn.ActiveInvestigator);
            for (int i = 0; i < 3; i++)
                yield return new DrawAction(investigatorTurn.ActiveInvestigator).RunNow();
        }
    }
}
