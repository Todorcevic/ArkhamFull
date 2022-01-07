using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Michsky.UI.Dark;

namespace ArkhamMenu
{
    public static class SwitchButton
    {
        public static void SwitchON(this Button button, bool state)
        {
            button.interactable = state;
            button.GetComponent<UIElementSound>().enabled = state;
        }
    }
}
