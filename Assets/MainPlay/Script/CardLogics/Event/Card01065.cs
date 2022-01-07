using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace ArkhamGamePlay
{
    public class Card01065 : CardEvent
    {
        RevelationAction revelationAction;
        protected override bool IsFast => true;
        protected override string nameCardEffect => "Cancelar efecto de revelación";

        /*****************************************************************************************/
        protected override void BeginGameAction(GameAction gameAction)
        {
            if (gameAction is RevelationAction revelationaction && CancelEffect(revelationaction))
                revelationaction.ChooseCardOptionalAction.ChosenCardEffects.Add(playFromHand = PlayFromHand);
        }

        bool CancelEffect(RevelationAction revelationAction)
        {
            if (!CheckPlayedFromHand()) return false;
            if (GameControl.ActiveInvestigator != ThisCard.VisualOwner) return false;
            if (revelationAction.RevelationCard.CardType != CardType.Treachery) return false;
            if (revelationAction.RevelationCard.IsWeakness) return false;
            revelationAction.RevelationCard.MoveTo(revelationAction.RevelationCard.CurrentZone);
            this.revelationAction = revelationAction;
            return true;
        }

        protected override IEnumerator LogicEffect()
        {
            revelationAction.IsActionCanceled = true;
            yield return new AssignDamageHorror(ThisCard.VisualOwner, horrorAmount: 1).RunNow();
            yield return new DiscardAction(revelationAction.RevelationCard, withPreview: false).RunNow();
        }

        protected override bool CheckFilterToCancel() => revelationAction.IsActionCanceled;
    }
}