using System.Collections;

namespace ArkhamGamePlay
{
    public class AddPhases : GameAction
    {
        public override GameActionType GameActionType => GameActionType.Setting;

        /*****************************************************************************************/
        protected override IEnumerator ActionLogic()
        {
            if (!GameControl.GameIsStarted)
            {
                GameControl.GameIsStarted = true;
                yield return new InvestigationPhase().RunNow();
                yield return new EnemyPhase().RunNow();
                yield return new UpkeepPhase().RunNow();
                yield return new AddPhases().RunNow();
            }
            else
            {
                yield return new MythosPhase().RunNow();
                yield return new InvestigationPhase().RunNow();
                yield return new EnemyPhase().RunNow();
                yield return new UpkeepPhase().RunNow();
                yield return new AddPhases().RunNow();
            }
        }
    }
}