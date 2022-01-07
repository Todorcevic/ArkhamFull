using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01013 : CardEvent
    {
        protected override void EndGameAction(GameAction gameAction)
        {
            if (gameAction is EndInvestigatorTurnAction endInvestigatorT && Forced(endInvestigatorT))
                new EffectAction(() => TakeHorror(endInvestigatorT.Investigator), TakeHorrorAnimation).AddActionTo();
        }

        bool Forced(EndInvestigatorTurnAction endInvestigatorTurn)
        {
            if (ThisCard.CurrentZone != endInvestigatorTurn.Investigator.Hand) return false;
            return true;
        }

        protected override IEnumerator LogicEffect()
        {
            yield return new AddTokenAction(GameControl.CurrentAgenda.ThisCard.DoomToken, 1).RunNow();
            yield return new CheckDoomThreshold(GameControl.CurrentAgenda).RunNow();
        }

        IEnumerator TakeHorrorAnimation() => new AnimationCardAction(ThisCard, audioClip: ThisCard.Effect2).RunNow();

        IEnumerator TakeHorror(InvestigatorComponent investigator) => new AssignDamageHorror(investigator, horrorAmount: 2).RunNow();

    }
}