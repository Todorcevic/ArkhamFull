using System.Collections;
using System;

namespace ArkhamGamePlay
{
    public class UpkeepPhase : PhaseAction
    {
        public override string GameActionInfo => "Fase de Mantenimiento.";

        /*****************************************************************************************/
        public UpkeepPhase() => AllComponents.PhasesUI.SelectPhase(this);
        /*****************************************************************************************/

        protected override IEnumerator PhaseLogic()
        {
            yield return new WindowInvestigatorAction().RunNow();
            yield return new ReadyExaustCards().RunNow(); //4.2 - 4.3
            foreach (InvestigatorComponent investigator in GameControl.AllInvestigatorsInGame.ToArray())
            {
                yield return new ActiveInvestigatorAction(investigator).RunNow();
                yield return new DrawAndResources(investigator).RunNow(); //4.4
                yield return new CheckSizeHand(investigator).RunNow();//4.5
                yield return new ActiveInvestigatorAction(null).RunNow();
            }
        }
    }
}