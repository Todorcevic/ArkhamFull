using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class EventPoint<T> : GameAction
    {
        T gameAction;
        public T GameAction { get => gameAction; set => gameAction = value; }

        public EventPoint(T gameAction) => this.gameAction = gameAction;

        protected override IEnumerator ActionLogic()
        {
            yield return null;
        }
    }
}