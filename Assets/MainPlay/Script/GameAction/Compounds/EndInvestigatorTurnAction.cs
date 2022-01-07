using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class EndInvestigatorTurnAction : GameAction
    {
        public InvestigatorComponent Investigator { get; set; }

        /*****************************************************************************************/
        public EndInvestigatorTurnAction(InvestigatorComponent investigator) => Investigator = investigator;

        /*****************************************************************************************/
        protected override IEnumerator ActionLogic()
        {
            Investigator.ShowInfo();
            GameControl.TurnInvestigator = null;
            yield return null;
        }
    }
}