using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01064 : CardEvent
    {
        protected override IEnumerator LogicEffect()
        {
            yield return new DrawAction(ThisCard.VisualOwner, isEncounter: true).RunNow();
            yield return new DiscoverCluesAction(ThisCard.VisualOwner, 2).RunNow();
        }
    }
}