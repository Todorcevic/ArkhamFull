using ArkhamGamePlay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01122 : CardAgenda
    {
        protected override void BeginGameAction(GameAction gameAction)
        {
            if (gameAction is InvestigatorTurn investigatorTurn)
                investigatorTurn.CardEffects.Add(new CardEffect(
                    card: ThisCard,
                    effect: () => Resign(investigatorTurn.ActiveInvestigator),
                    animationEffect: () => ResignAnimation(investigatorTurn.ActiveInvestigator),
                    type: EffectType.Activate | EffectType.Resign,
                    name: "Desistir",
                    actionCost: 1));
        }

        IEnumerator ResignAnimation(InvestigatorComponent investigator) =>
            new AnimationCardAction(investigator.InvestigatorCardComponent, audioClip: investigator.InvestigatorCardComponent.Effect8).RunNow();

        IEnumerator Resign(InvestigatorComponent investigator)
        {
            investigator.IsResign = true;
            yield return new InvestigatorEliminatedAction(investigator).RunNow();
        }

        public override IEnumerator BackFace() => new FinishGameAction(new CoreScenario2().R2).RunNow();
    }
}