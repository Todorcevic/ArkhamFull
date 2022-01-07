using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01111 : CardLocation
    {
        public override LocationSymbol MySymbol => LocationSymbol.Circle;
        public override LocationSymbol MovePosibilities => LocationSymbol.None;
    }
}