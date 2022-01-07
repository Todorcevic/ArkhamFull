using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

namespace ArkhamGamePlay
{
    public class MulliganEnd : GameAction
    {
        readonly InvestigatorComponent activeInvestigator;
        public override GameActionType GameActionType => GameActionType.Compound;

        /*****************************************************************************************/
        public MulliganEnd(InvestigatorComponent investigator) => activeInvestigator = investigator;

        /*****************************************************************************************/
        protected override IEnumerator ActionLogic()
        {

            yield return new MoveDeck(activeInvestigator.InvestigatorDiscard.ListCards.AsEnumerable().Reverse().ToList(), activeInvestigator.InvestigatorDeck, isBack: true, withMoveUp: true).RunNow();
            yield return new ShuffleAction(activeInvestigator.InvestigatorDeck).RunNow();
        }
    }
}