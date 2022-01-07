using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01134 : CardLocation
    {
        bool effectUsed;
        public override LocationSymbol MySymbol => LocationSymbol.T;
        public override LocationSymbol MovePosibilities => LocationSymbol.Diamond | LocationSymbol.Triangle;

        /*****************************************************************************************/
        protected override void BeginGameAction(GameAction gameAction)
        {
            base.BeginGameAction(gameAction);
            if (gameAction is InvestigatorTurn investigatorTurn && CheckCanUse(investigatorTurn))
                investigatorTurn.CardEffects.Add(new CardEffect(
                    card: ThisCard,
                    effect: () => TakeClues(investigatorTurn),
                    animationEffect: TakeCluesAnimation,
                    type: EffectType.Activate,
                    name: "Obtener pistas",
                    actionCost: 1,
                    resourceCost: 5,
                    investigatorImageCardInfoOwner: investigatorTurn.ActiveInvestigator));
        }

        bool CheckCanUse(InvestigatorTurn investigatorTurn)
        {
            if (effectUsed) return false;
            if (investigatorTurn.ActiveInvestigator.CurrentLocation != ThisCard) return false;
            return true;
        }

        IEnumerator TakeCluesAnimation() => new AnimationCardAction(ThisCard, audioClip: ThisCard.Effect1).RunNow();

        IEnumerator TakeClues(InvestigatorTurn investigatorTurn)
        {
            effectUsed = true;
            yield return new AddTokenAction(investigatorTurn.ActiveInvestigator.InvestigatorCardComponent.CluesToken, 2).RunNow();
        }
    }
}
