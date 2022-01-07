using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamMenu
{
    public static class DoubleClickCheck
    {
        static GameObject lastObjectClicked;
        static float doubleClickTimeLimit = 0.5f;
        static float lastClickTime;

        public static bool DoubleClicked(float clickTime, GameObject objectClicked)
        {
            if (clickTime - lastClickTime < doubleClickTimeLimit && objectClicked == lastObjectClicked)
                return true;
            lastClickTime = clickTime;
            lastObjectClicked = objectClicked;
            return false;
        }
    }
}
