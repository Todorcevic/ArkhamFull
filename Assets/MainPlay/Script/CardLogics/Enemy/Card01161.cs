using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace ArkhamGamePlay
{
    public class Card01161 : CardEnemy
    {
        public override List<InvestigatorComponent> Prey(List<InvestigatorComponent> investigatorList)
        {
            int? minHealthLeft = investigatorList.Select(i => i.Health - i.InvestigatorCardComponent.HealthToken.Amount)?.Min();
            return investigatorList.FindAll(i => i.Health - i.InvestigatorCardComponent.HealthToken.Amount == minHealthLeft);
        }
    }
}