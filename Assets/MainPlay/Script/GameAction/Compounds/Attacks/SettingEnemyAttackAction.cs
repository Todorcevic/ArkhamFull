using System.Collections;
using System;

namespace ArkhamGamePlay
{
    public class SettingEnemyAttackAction : GameAction
    {
        public override GameActionType GameActionType => GameActionType.Compound;
        public CardComponent Enemy { get; }
        public InvestigatorComponent Investigator { get; }

        /*****************************************************************************************/
        public SettingEnemyAttackAction(CardComponent enemy, InvestigatorComponent investigator)
        {
            Enemy = enemy;
            Investigator = investigator;
        }

        /*****************************************************************************************/
        protected override IEnumerator ActionLogic()
        {
            if (Investigator.IsDefeat) yield break;
            yield return new EnemyAttackAction(Investigator, Enemy).RunNow();
        }
    }
}