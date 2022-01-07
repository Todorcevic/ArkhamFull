using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01037 : CardEvent
    {
        protected override bool IsFast => true;

        /*****************************************************************************************/
        protected override bool CheckPlayedFromHand()
        {
            if (!(GameControl.CurrentPhase is InvestigationPhase)) return false;
            if (GameControl.TurnInvestigator != ThisCard.VisualOwner) return false;
            if (ThisCard.VisualOwner.CurrentLocation.CluesToken.Amount < 1) return false;
            return base.CheckPlayedFromHand();
        }

        protected override IEnumerator LogicEffect() => new DiscoverCluesAction(ThisCard.VisualOwner, 1).RunNow();
    }
}