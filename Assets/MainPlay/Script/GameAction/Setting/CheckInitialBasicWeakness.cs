using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace ArkhamGamePlay
{
    public class CheckInitialBasicWeakness : GameAction
    {
        readonly InvestigatorComponent investigator;
        public override GameActionType GameActionType => GameActionType.Setting;

        /*****************************************************************************************/
        public CheckInitialBasicWeakness(InvestigatorComponent investigator) => this.investigator = investigator;

        /*****************************************************************************************/
        protected override IEnumerator ActionLogic()
        {
            bool isNeedRedraw = false;
            yield return new WaitWhile(() => DOTween.IsTweening("MoveTo"));
            foreach (CardComponent card in investigator.Hand.ListCards.FindAll(c => c.Info.Subtype_code == "weakness" || c.Info.Subtype_code == "basicweakness"))
            {
                yield return card.Preview().WaitForCompletion();
                yield return card.MoveTo(investigator.InvestigatorDiscard).WaitForCompletion();
                isNeedRedraw = true;
            }
            if (isNeedRedraw) yield return new DrawInitialHand(investigator).RunNow();
        }
    }
}