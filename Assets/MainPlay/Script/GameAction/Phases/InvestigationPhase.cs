using System.Collections;
using System;
using ArkhamGamePlay;

namespace ArkhamGamePlay
{
    public class InvestigationPhase : PhaseAction
    {
        public override string GameActionInfo => "Fase de Investigación.";

        /*****************************************************************************************/
        public InvestigationPhase() => AllComponents.PhasesUI.SelectPhase(this);

        /*****************************************************************************************/

        protected override IEnumerator PhaseLogic()
        {
            while (GameControl.AllInvestigatorsInGame.Exists(c => !c.IsExausted))
            {
                yield return new ActiveInvestigatorAction(GameControl.AllInvestigatorsInGame.Find(c => !c.IsExausted)).RunNow();
                yield return new ChooseInvestigator().RunNow();
                GameControl.TurnInvestigator = GameControl.ActiveInvestigator;
                yield return new InvestigatorTurn().RunNow();
                yield return new EndInvestigatorTurnAction(GameControl.TurnInvestigator).RunNow();
                yield return new ActiveInvestigatorAction(null).RunNow();
            }
        }
    }
}