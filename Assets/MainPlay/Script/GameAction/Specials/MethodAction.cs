using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ArkhamGamePlay
{
    public class MethodAction : GameAction
    {
        Action method;
        public override GameActionType GameActionType => GameActionType.Basic;

        /*****************************************************************************************/
        public MethodAction(Action method) => this.method = method;

        /*****************************************************************************************/
        protected override IEnumerator ActionLogic()
        {
            method?.Invoke();
            yield return null;
        }
    }
}