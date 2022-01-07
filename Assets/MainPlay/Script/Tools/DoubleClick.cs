using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class DoubleClick
    {
        float doubleClickTimeLimit = 0.5f;
        float lastClickTime;
        public bool IsDetected(float clickTime)
        {
            if (clickTime - lastClickTime < doubleClickTimeLimit) return true;
            lastClickTime = clickTime;
            return false;
        }
    }
}