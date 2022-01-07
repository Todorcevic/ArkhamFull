using System.Collections;
using ArkhamShared;

namespace ArkhamGamePlay
{
    public class SettingGame : PhaseAction
    {
        //public override string GameActionInfo => "Preparación de la partida.";

        /*****************************************************************************************/
        protected override IEnumerator PhaseLogic()
        {
            AllComponents.PhasesUI.SettingGamePhase();
            if (!GameData.Instance.IsNotFirstScenario)
                yield return new PanelHistoryAction(GameData.Instance.FullScenarioName).RunNow();
            yield return new SetChaosBagAction(testMode: false).RunNow();
            foreach (InvestigatorComponent investigator in GameControl.AllInvestigatorsInGame)
            {
                yield return new ActiveInvestigatorAction(investigator).RunNow();
                yield return new SetInvestigator(investigator).RunNow();
                yield return new MoveDeck(investigator.ListCardComponent, investigator.InvestigatorDeck, isBack: true).RunNow();
                yield return new ShuffleAction(investigator.InvestigatorDeck).RunNow();
                yield return new DrawInitialHand(investigator).RunNow();
                yield return new Mulligan().RunNow();
                yield return new ActiveInvestigatorAction(null).RunNow();
            }
            yield return new SelectScenario().RunNow();
            yield return new AddPhases().RunNow();
        }
    }
}