using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class VoidAction : GameAction
    {
        protected override IEnumerator ActionLogic() => null;
    }
}