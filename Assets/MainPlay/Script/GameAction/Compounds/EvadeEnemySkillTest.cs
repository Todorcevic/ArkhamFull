using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class EvadeEnemySkillTest : GameAction
    {
        readonly CardComponent enemy;
        public override GameActionType GameActionType => GameActionType.Compound;
        public SkillTest SkillTest { get; set; }

        /*****************************************************************************************/
        public EvadeEnemySkillTest(CardComponent enemy, bool isCancelable)
        {
            this.enemy = enemy;

            SkillTest = new SkillTest
            {
                Title = "Evadiendo " + enemy.Info.Name,
                SkillType = Skill.Agility,
                SkillTestType = SkillTestType.Evade,
                CardToTest = enemy,
                TestValue = (int)enemy.Info.Enemy_evade,
                IsOptional = isCancelable
            };
        }

        /*****************************************************************************************/
        protected override IEnumerator ActionLogic()
        {
            SkillTest.WinEffect.Add(new CardEffect(enemy, WinEffect, EffectType.Choose, name: "Evadir"));
            yield return new SkillTestAction(SkillTest).RunNow();
        }

        IEnumerator WinEffect() => new EvadeEnemyAction(enemy).RunNow();
    }
}