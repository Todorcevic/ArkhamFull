using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01051 : CardEvent
    {
        protected override void BeginGameAction(GameAction gameAction)
        {
            if (gameAction is InvestigatorTurn investigatorturn && PlayingCard(investigatorturn))
            {
                playFromHand = PlayFromHand;
                playFromHand.Type = EffectType.Play | EffectType.Fight;
                investigatorturn.CardEffects.Add(playFromHand);
            }
        }

        protected override bool PlayingCard(InvestigatorTurn investigatorturn)
        {
            if (!CheckPlayedFromHand()) return false;
            if (investigatorturn.ActiveInvestigator != ThisCard.VisualOwner) return false;
            if (ThisCard.VisualOwner.AllEnemiesInMyLocation.Count < 1) return false;
            return true;
        }

        protected override IEnumerator LogicEffect() => new FightWithWeaponAction(ref playFromHand, Fight).RunNow();

        IEnumerator Fight(CardComponent enemy)
        {
            SettingInvestigatorAttack attackToenemy = new SettingInvestigatorAttack(enemy, ThisCard.VisualOwner.AttackDamage + 2, playFromHand.IsCancelable);
            attackToenemy.SkillTest.SkillType = Skill.Agility;
            attackToenemy.SkillTest.ExtraCard = ThisCard;
            yield return attackToenemy.RunNow();
            playFromHand.IsCancel = !attackToenemy.SkillTest.IsComplete ?? false;
        }
    }
}