using ArkhamGamePlay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class DrawEncounter : GameAction
    {
        InvestigatorComponent investigator;
        public override string GameActionInfo => investigator.Name + " robando carta de Encuentro.";

        /*****************************************************************************************/
        public DrawEncounter(InvestigatorComponent investigator) => this.investigator = investigator;
        /*****************************************************************************************/

        protected override IEnumerator ActionLogic() => new DrawAction(investigator, isEncounter: true).RunNow();
    }
}