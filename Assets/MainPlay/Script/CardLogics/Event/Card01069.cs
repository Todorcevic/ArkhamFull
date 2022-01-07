using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01069 : Card01066
    {
        protected override int DamageToEnemy => 2;

        protected override IEnumerator ChaosTokenReveled()
        {
            yield return base.ChaosTokenReveled();
            yield return new AssignDamageHorror(ThisCard.VisualOwner, horrorAmount: 1).RunNow();
        }
    }
}