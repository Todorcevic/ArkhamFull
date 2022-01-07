using System.Collections;
using UnityEngine;
using DG.Tweening;

namespace ArkhamGamePlay
{
    public class SelectInvestigatorAction : GameAction
    {
        readonly InvestigatorComponent investigator;
        public override GameActionType GameActionType => GameActionType.Basic;

        /*****************************************************************************************/
        public SelectInvestigatorAction(InvestigatorComponent investigator) => this.investigator = investigator;

        /*****************************************************************************************/
        protected override IEnumerator ActionLogic()
        {
            if (investigator != null && investigator != GameControl.CurrentInvestigator && !investigator.IsDefeat)
            {
                yield return new WaitWhile(() => DOTween.IsTweening("MoveCard"));
                yield return AllComponents.InvestigatorManagerComponent.SelectInvestigator(investigator);
            }
        }
    }
}