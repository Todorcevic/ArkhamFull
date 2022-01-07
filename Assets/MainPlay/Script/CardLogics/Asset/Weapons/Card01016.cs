using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01016 : Weapon
    {
        protected override int ResourceStarting => 4;
        protected override int ResourceCost => 1;
        protected override int ActionsCost => 1;
        protected override int Damage => ThisCard.VisualOwner.AttackDamage + 1;
        protected override int Bonus => 1;
    }
}