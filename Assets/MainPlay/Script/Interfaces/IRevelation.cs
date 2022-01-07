using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public interface IRevelation
    {
        bool IsDiscarted { get; }
        IEnumerator Revelation();
    }
}