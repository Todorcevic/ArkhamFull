using System.Collections;
using System.Collections.Generic;
using System;

namespace ArkhamGamePlay
{
    public class SettingInvestigatorAttack : GameAction
    {
        public override GameActionType GameActionType => GameActionType.Compound;
        InvestigatorComponent InvestigatorEnganged => ((CardEnemy)Enemy.CardLogic).InvestigatorEnganged;
        bool IsDangerAttack => ((CardEnemy)Enemy.CardLogic).IsEnganged && InvestigatorEnganged != GameControl.ActiveInvestigator;
        public int Damage { get; set; }
        public int Bonus { get; set; }
        public SkillTest SkillTest { get; }
        public CardComponent Enemy { get; set; }
        public Effect WinEffect { get; set; }
        public Effect LoseEffect { get; set; }

        /*****************************************************************************************/
        public SettingInvestigatorAttack(CardComponent enemy, int damage, bool isCancelabe, int bonus = 0)
        {
            Enemy = enemy;
            Damage = damage;
            Bonus = bonus;
            WinEffect = WinEffectDefault;

            SkillTest = new SkillTest
            {
                Title = "Atacando a " + Enemy.Info.Name,
                SkillType = Skill.Combat,
                SkillTestType = SkillTestType.Attack,
                CardToTest = Enemy,
                TestValue = (int)Enemy.Info.Enemy_fight,
                IsOptional = isCancelabe
            };
        }

        IEnumerator WinEffectDefault() => new DamageEnemyAction(Enemy, Damage).RunNow();
        IEnumerator LoseEffectDefault() => new AssignDamageHorror(InvestigatorEnganged, damageAmount: Damage).RunNow();

        /*****************************************************************************************/
        protected override IEnumerator ActionLogic()
        {
            SkillTest.WinEffect.Add(new CardEffect(
                card: Enemy,
                effect: WinEffect,
                type: EffectType.Choose,
                name: "Hacer el daño"));
            if (IsDangerAttack)
                SkillTest.LoseEffect.Add(new CardEffect(
                    card: InvestigatorEnganged.InvestigatorCardComponent,
                    effect: LoseEffectDefault,
                    type: EffectType.Choose,
                    name: "Hacer el daño"));
            GameControl.ActiveInvestigator.CombatBonus += Bonus;
            yield return new InvestigatorAttackAction(SkillTest).RunNow();
            GameControl.ActiveInvestigator.CombatBonus -= Bonus;
        }
    }
}