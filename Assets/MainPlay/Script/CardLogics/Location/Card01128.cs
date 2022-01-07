using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01128 : CardLocation
    {
        List<InvestigatorComponent> investigatorsUsed = new List<InvestigatorComponent>();
        public override LocationSymbol MySymbol => LocationSymbol.Plus;
        public override LocationSymbol MovePosibilities => LocationSymbol.Diamond | LocationSymbol.Square;

        /*****************************************************************************************/
        protected override void BeginGameAction(GameAction gameAction)
        {
            base.BeginGameAction(gameAction);
            if (gameAction is InvestigatorTurn investigatorTurn && CheckCanUse(investigatorTurn))
                investigatorTurn.CardEffects.Add(new CardEffect(
                    card: ThisCard,
                    effect: () => Heal(investigatorTurn),
                    animationEffect: HealAnimation,
                    type: EffectType.Activate,
                    name: "Curar daño",
                    actionCost: 1,
                    investigatorImageCardInfoOwner: investigatorTurn.ActiveInvestigator));
        }

        bool CheckCanUse(InvestigatorTurn investigatorTurn)
        {
            if (investigatorTurn.ActiveInvestigator.CurrentLocation != ThisCard) return false;
            if (investigatorsUsed.Contains(investigatorTurn.ActiveInvestigator)) return false;
            return true;
        }

        IEnumerator HealAnimation() => new AnimationCardAction(ThisCard, audioClip: ThisCard.Effect1).RunNow();

        IEnumerator Heal(InvestigatorTurn investigatorTurn)
        {
            investigatorsUsed.Add(investigatorTurn.ActiveInvestigator);
            int healAmoun = investigatorTurn.ActiveInvestigator.Damage >= 3 ? 3 : investigatorTurn.ActiveInvestigator.Damage;
            if (healAmoun > 0)
                yield return new AddTokenAction(investigatorTurn.ActiveInvestigator.InvestigatorCardComponent.HealthToken, -healAmoun).RunNow();
        }
    }
}
