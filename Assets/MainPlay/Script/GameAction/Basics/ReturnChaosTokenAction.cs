using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class ReturnChaosTokenAction : GameAction
    {
        readonly ChaosTokenComponent token;
        public ChaosTokenComponent Token => token;

        /*****************************************************************************************/
        public ReturnChaosTokenAction(ChaosTokenComponent token) => this.token = token;

        /*****************************************************************************************/
        protected override IEnumerator ActionLogic()
        {
            yield return AllComponents.ChaosBag.ReturnToken(token);
        }
    }
}