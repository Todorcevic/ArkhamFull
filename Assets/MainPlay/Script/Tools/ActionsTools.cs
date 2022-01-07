using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class ActionsTools
    {
        public Effect JoinEffects(Effect effect1, Effect effect2)
        {
            IEnumerator EffectResult()
            {
                yield return effect1?.Invoke();
                yield return effect2?.Invoke();
            }
            return EffectResult;
        }

        public Effect JoinActions(GameAction action1, GameAction action2)
        {
            return EffectResult;
            IEnumerator EffectResult()
            {
                yield return action1.RunNow();
                yield return action2.RunNow();
            }
        }
    }
}