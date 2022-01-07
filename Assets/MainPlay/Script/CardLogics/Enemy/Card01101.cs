using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01101 : CardEnemy
    {
        public override List<InvestigatorComponent> Prey(List<InvestigatorComponent> investigatorList) =>
            investigatorList.Contains(ThisCard.Owner) ? new List<InvestigatorComponent> { ThisCard.Owner } : investigatorList;

        public override bool IsHunter => true;

        /*****************************************************************************************/

        protected override void BeginGameAction(GameAction gameAction)
        {
            base.BeginGameAction(gameAction);
            if (gameAction is InvestigatorTurn investigatorTurn && CheckToParley(investigatorTurn))
                investigatorTurn.CardEffects.Add(new CardEffect(
                    card: ThisCard,
                    effect: Parley,
                    animationEffect: ParleyAnimation,
                    type: EffectType.Activate | EffectType.Parley,
                    name: "Pagar la deuda a " + ThisCard.Info.Name,
                    actionCost: 1,
                    resourceCost: 4));
        }

        bool CheckToParley(InvestigatorTurn investigatorTurn)
        {
            if (!ThisCard.IsInPlay) return false;
            if (((CardEnemy)ThisCard.CardLogic).CurrentLocation != investigatorTurn.ActiveInvestigator.CurrentLocation) return false;
            return true;
        }

        IEnumerator ParleyAnimation() => new AnimationCardAction(ThisCard, audioClip: ThisCard.Effect1).RunNow();

        IEnumerator Parley() => new DiscardAction(ThisCard).RunNow();
    }
}