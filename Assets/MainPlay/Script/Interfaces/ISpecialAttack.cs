using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public interface ISpecialAttack
    {
        IEnumerator Attack();
    }
}