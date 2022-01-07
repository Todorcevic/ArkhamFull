using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class ShuffleAction : GameAction
    {
        readonly Zone deck;
        public override GameActionType GameActionType => GameActionType.Basic;

        /*****************************************************************************************/
        public ShuffleAction(Zone deck) => this.deck = deck;

        /*****************************************************************************************/
        protected override IEnumerator ActionLogic()
        {
            yield return new SelectInvestigatorAction(deck.InvestigatorOwer).RunNow();
            yield return deck.Shuffle();
        }
    }
}