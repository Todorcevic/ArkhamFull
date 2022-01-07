using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01020 : Weapon
    {
        protected override int ResourceStarting => 0;
        protected override int ResourceCost => 0;
        protected override int ActionsCost => 1;
        protected override int Damage =>
            ((CardEnemy)enemyToFight.CardLogic).InvestigatorEnganged == ThisCard.VisualOwner
            && GameControl.AllCardComponents.FindAll(c => c.CardLogic is CardEnemy cardEnemy && cardEnemy.InvestigatorEnganged == ThisCard.VisualOwner).Count == 1 ?
            ThisCard.VisualOwner.AttackDamage + 1 : ThisCard.VisualOwner.AttackDamage;
        protected override int Bonus => 1;
    }
}