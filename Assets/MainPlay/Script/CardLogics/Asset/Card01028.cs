using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01028 : Card01018
    {
        protected override bool NeedExhaust => true;

        /*****************************************************************************************/
        protected override bool CheckPlayCardInGame(InteractableAction interactableAction)
        {
            if (ThisCard.IsExausted) return false;
            return base.CheckPlayCardInGame(interactableAction);
        }

        protected override IEnumerator PayCostEffect() =>
            new AddTokenAction(ThisCard.HealthToken, 1).RunNow();

        protected override IEnumerator CancelCardEffect()
        {
            if (!ThisCard.IsInPlay)
            {
                yield return new MoveCardAction(ThisCard, ThisCard.VisualOwner.Assets).RunNow();
                yield return new AddTokenAction(ThisCard.HealthToken, (int)ThisCard.Info.Health - 1).RunNow();
            }
            else yield return new AddTokenAction(ThisCard.HealthToken, -1).RunNow();
        }
    }
}