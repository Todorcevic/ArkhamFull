using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public interface IBuffable
    {
        bool ActiveBuffCondition(GameAction gameAction);
        void BuffEffect();
        void DeBuffEffect();
    }
}