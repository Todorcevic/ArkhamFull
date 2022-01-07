using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01047 : Weapon
    {
        protected override int ResourceStarting => 3;
        protected override int ResourceCost => 1;
        protected override int ActionsCost => 1;
        protected override int Damage => ThisCard.VisualOwner.AttackDamage;
        protected override int Bonus => 2;

        /*****************************************************************************************/
        protected override void SettingFight(CardComponent enemy)
        {
            base.SettingFight(enemy);
            attackToenemy.WinEffect = new ActionsTools().JoinEffects(ExtraDamage, attackToenemy.WinEffect);

            IEnumerator ExtraDamage()
            {
                if (attackToenemy.SkillTest.TotalInvestigatorValue - attackToenemy.SkillTest.TotalTestValue > 1)
                    attackToenemy.Damage++;
                yield return null;
            }
        }
    }
}