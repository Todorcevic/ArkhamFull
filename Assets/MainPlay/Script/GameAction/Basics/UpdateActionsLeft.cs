using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class UpdateActionsLeft : GameAction
    {
        public override GameActionType GameActionType => GameActionType.Basic;
        public InvestigatorComponent Investigator { get; }
        public int Amount { get; }

        /*****************************************************************************************/
        public UpdateActionsLeft(InvestigatorComponent investigator, int amount)
        {
            Amount = amount;
            Investigator = investigator;
        }

        /*****************************************************************************************/
        protected override IEnumerator ActionLogic()
        {
            if (Investigator.IsDefeat) yield break;
            Investigator.ActionsLeft += Amount;
            if (Investigator.ActionsLeft < 0) Investigator.ActionsLeft = 0;

            Investigator.IsExausted = Investigator.ActionsLeft < 1;
            yield return new ExaustCardAction(Investigator.PlayCard, Investigator.ActionsLeft < 1, withPause: false).RunNow();
            yield return new ExaustCardAction(Investigator.InvestigatorCardComponent, Investigator.ActionsLeft < 1, withPause: false).RunNow();
            yield return new WaitWhile(() => DOTween.IsTweening("Exhausting"));
        }
    }
}