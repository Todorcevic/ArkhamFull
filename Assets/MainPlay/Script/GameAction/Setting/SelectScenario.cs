using System.Collections;
using System;
using System.Runtime.Remoting;
using UnityEngine;
using System.Reflection;
using ArkhamShared;

namespace ArkhamGamePlay
{
    public class SelectScenario : GameAction
    {
        public override GameActionType GameActionType => GameActionType.Setting;

        /*****************************************************************************************/
        protected override IEnumerator ActionLogic()
        {
            ObjectHandle handle = Activator.CreateInstance(null, "ArkhamGamePlay." + GameData.Instance.FullScenarioName);
            GameControl.CurrentScenario = (GameAction)handle.Unwrap();
            yield return GameControl.CurrentScenario.RunNow();
        }
    }
}