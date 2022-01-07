using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01112 : CardLocation
    {
        public override LocationSymbol MySymbol => LocationSymbol.Square;
        public override LocationSymbol MovePosibilities
            => (LocationSymbol.Triangle | LocationSymbol.Plus | LocationSymbol.Diamond);
    }
}