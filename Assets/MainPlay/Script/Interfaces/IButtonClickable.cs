using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public interface IButtonClickable
    {
        void SetButton();
        void ReadyClicked(ReadyButton button);
    }
}