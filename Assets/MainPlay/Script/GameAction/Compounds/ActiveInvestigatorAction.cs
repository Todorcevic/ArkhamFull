using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class ActiveInvestigatorAction : GameAction
    {
        readonly InvestigatorComponent investigator;
        public override GameActionType GameActionType => GameActionType.Compound;

        /*****************************************************************************************/
        public ActiveInvestigatorAction(InvestigatorComponent investigator) => this.investigator = investigator;

        /*****************************************************************************************/
        protected override IEnumerator ActionLogic()
        {
            if (investigator?.IsDefeat ?? false) yield break;
            if (investigator != null)
                yield return new SelectInvestigatorAction(investigator).RunNow();
            GameControl.ActiveInvestigator = investigator;
        }
    }
}