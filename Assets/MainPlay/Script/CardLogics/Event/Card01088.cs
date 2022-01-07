using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01088 : CardEvent
    {
        protected override IEnumerator LogicEffect()
            => new AddTokenAction(GameControl.ActiveInvestigator.InvestigatorCardComponent.ResourcesToken, 3).RunNow();
    }
}