using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01084 : Card01080
    {
        protected override IEnumerator LogicEffect()
        {
            yield return base.LogicEffect();
            yield return new DrawAction(ThisCard.VisualOwner).RunNow();
        }
    }
}