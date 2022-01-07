using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01029 : Weapon
    {
        protected override int ResourceStarting => 2;
        protected override int ResourceCost => 1;
        protected override int ActionsCost => 1;
        protected override int Damage => 0;
        protected override int Bonus => 3;

        /*****************************************************************************************/
        protected override void SettingFight(CardComponent enemy)
        {
            base.SettingFight(enemy);
            attackToenemy.WinEffect = new ActionsTools().JoinEffects(WinExtraDamage, attackToenemy.WinEffect);
            attackToenemy.LoseEffect = new ActionsTools().JoinEffects(LoseExtraDamage, attackToenemy.LoseEffect);

            IEnumerator WinExtraDamage()
            {
                attackToenemy.Damage = attackToenemy.SkillTest.TotalInvestigatorValue - attackToenemy.SkillTest.TotalTestValue;
                attackToenemy.Damage = attackToenemy.Damage > 5 ? 5 : attackToenemy.Damage;
                yield return null;
            }

            IEnumerator LoseExtraDamage()
            {
                attackToenemy.Damage = attackToenemy.SkillTest.TotalTestValue - attackToenemy.SkillTest.TotalInvestigatorValue;
                attackToenemy.Damage = attackToenemy.Damage > 5 ? 5 : attackToenemy.Damage;
                yield return null;
            }
        }
    }
}