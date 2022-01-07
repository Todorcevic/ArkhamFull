using System.Collections;
using System;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class MythosPhase : PhaseAction
    {
        public override string GameActionInfo => "Fase de Mitos.";

        /*****************************************************************************************/
        public MythosPhase() => AllComponents.PhasesUI.SelectPhase(this);

        /*****************************************************************************************/
        protected override IEnumerator PhaseLogic()
        {
            yield return new AddTokenAction(GameControl.CurrentAgenda.ThisCard.DoomToken, 1).RunNow();
            yield return new CheckDoomThreshold(GameControl.CurrentAgenda).RunNow();
            foreach (InvestigatorComponent investigator in GameControl.AllInvestigatorsInGame.ToArray())
            {
                yield return new ActiveInvestigatorAction(investigator).RunNow();
                yield return new DrawEncounter(investigator).RunNow();
                yield return new ActiveInvestigatorAction(null).RunNow();
            }
            yield return new WindowInvestigatorAction().RunNow();
        }
    }
}