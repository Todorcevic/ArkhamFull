using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01057 : CardEvent
    {
        protected override IEnumerator LogicEffect() =>
            new AddTokenAction(ThisCard.VisualOwner.InvestigatorCardComponent.ResourcesToken, 10).RunNow();
    }
}