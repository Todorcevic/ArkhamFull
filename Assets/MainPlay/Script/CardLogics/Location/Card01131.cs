using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01131 : CardLocation
    {
        List<InvestigatorComponent> investigatorsUsed = new List<InvestigatorComponent>();
        public override LocationSymbol MySymbol => LocationSymbol.Triangle;
        public override LocationSymbol MovePosibilities => LocationSymbol.Moon | LocationSymbol.T;

        /*****************************************************************************************/

        protected override void BeginGameAction(GameAction gameAction)
        {
            base.BeginGameAction(gameAction);

            if (gameAction is InvestigatorTurn investigatorTurn && CheckCanUse(investigatorTurn))
                investigatorTurn.CardEffects.Add(new CardEffect(
                    card: ThisCard,
                    effect: () => HealThreeHorror(investigatorTurn),
                    animationEffect: HealThreeHorrorAnimation,
                    type: EffectType.Activate,
                    name: "Curar horror.",
                    actionCost: 1,
                    investigatorImageCardInfoOwner: investigatorTurn.ActiveInvestigator));
        }

        bool CheckCanUse(InvestigatorTurn investigatorTurn)
        {
            if (investigatorTurn.ActiveInvestigator.CurrentLocation != ThisCard) return false;
            if (investigatorsUsed.Contains(investigatorTurn.ActiveInvestigator)) return false;
            return true;
        }

        IEnumerator HealThreeHorrorAnimation() => new AnimationCardAction(ThisCard, audioClip: ThisCard.Effect1).RunNow();

        IEnumerator HealThreeHorror(InvestigatorTurn investigatorTurn)
        {
            investigatorsUsed.Add(investigatorTurn.ActiveInvestigator);
            int healAmoun = investigatorTurn.ActiveInvestigator.Horror >= 3 ? 3 : investigatorTurn.ActiveInvestigator.Horror;
            if (healAmoun > 0)
                yield return new AddTokenAction(investigatorTurn.ActiveInvestigator.InvestigatorCardComponent.SanityToken, -healAmoun).RunNow();
        }
    }
}
