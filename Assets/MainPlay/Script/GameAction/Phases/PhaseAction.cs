using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ArkhamGamePlay
{
    public abstract class PhaseAction : GameAction
    {
        public sealed override GameActionType GameActionType => GameActionType.Phase;

        /*****************************************************************************************/
        protected override sealed IEnumerator ActionLogic()
        {
            GameControl.CurrentPhase = this;
            yield return PhaseLogic();
        }

        protected abstract IEnumerator PhaseLogic();
    }
}