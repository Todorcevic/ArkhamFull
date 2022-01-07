using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01125 : CardLocation
    {
        public override LocationSymbol MySymbol => LocationSymbol.Circle;

        public override LocationSymbol MovePosibilities => LocationSymbol.Moon | LocationSymbol.Diamond | LocationSymbol.Square | LocationSymbol.Bar | LocationSymbol.X;
    }
}
