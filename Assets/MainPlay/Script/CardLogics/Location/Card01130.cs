using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01130 : CardLocation
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
                    effect: () => TakeThreeResources(investigatorTurn),
                    animationEffect: TakeThreeResourcesAnimation,
                    type: EffectType.Activate,
                    name: "Obtener recursos.",
                    actionCost: 1,
                    investigatorImageCardInfoOwner: investigatorTurn.ActiveInvestigator));
        }

        bool CheckCanUse(InvestigatorTurn investigatorTurn)
        {
            if (investigatorTurn.ActiveInvestigator.CurrentLocation != ThisCard) return false;
            if (investigatorsUsed.Contains(investigatorTurn.ActiveInvestigator)) return false;
            return true;
        }

        IEnumerator TakeThreeResourcesAnimation() => new AnimationCardAction(ThisCard, audioClip: ThisCard.Effect1).RunNow();

        IEnumerator TakeThreeResources(InvestigatorTurn investigatorTurn)
        {
            investigatorsUsed.Add(investigatorTurn.ActiveInvestigator);
            yield return new AddTokenAction(investigatorTurn.ActiveInvestigator.InvestigatorCardComponent.ResourcesToken, 3).RunNow();
        }
    }
}
