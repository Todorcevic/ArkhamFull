using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace ArkhamGamePlay
{
    public class Card01116 : CardEnemy
    {
        public override bool IsHunter => true;
        public override bool IsRetaliate => true;

        /*****************************************************************************************/
        public override List<InvestigatorComponent> Prey(List<InvestigatorComponent> investigatorList)
        {
            int? maxCombat = investigatorList.Select(i => i.InvestigatorCardComponent.Combat)?.Max();
            return investigatorList.FindAll(i => i.InvestigatorCardComponent.Combat == maxCombat);
        }
    }
}