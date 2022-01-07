using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace ArkhamGamePlay
{
    public class Card01006 : Weapon
    {
        protected override int ResourceStarting => 4;
        protected override int Damage => ThisCard.VisualOwner.AttackDamage + 1;
        protected override int Bonus => ThisCard.VisualOwner.CurrentLocation.CluesToken.Amount > 0 ? 3 : 1;
        protected override int ResourceCost => 1;
        protected override int ActionsCost => 1;
    }
}