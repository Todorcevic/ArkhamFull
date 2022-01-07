using ArkhamGamePlay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class DrawAndResources : GameAction
    {
        InvestigatorComponent investigator;
        public override string GameActionInfo => investigator.Name + " robando cartas y recursos.";

        /*****************************************************************************************/
        public DrawAndResources(InvestigatorComponent investigator) => this.investigator = investigator;

        /*****************************************************************************************/
        protected override IEnumerator ActionLogic()
        {
            yield return new DrawAction(investigator).RunNow();
            yield return new AddTokenAction(investigator.InvestigatorCardComponent.ResourcesToken, 1).RunNow();
        }
    }
}