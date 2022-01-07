using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace ArkhamGamePlay
{
    public class Card01060 : Weapon
    {
        bool attackingWithThiscard;
        protected override int ResourceStarting => 4;
        protected override int ResourceCost => 1;
        protected override int ActionsCost => 1;
        protected override int Damage => ThisCard.VisualOwner.AttackDamage + 1;
        protected override int Bonus => 0;

        /*****************************************************************************************/
        protected override void EndGameAction(GameAction gameAction)
        {
            base.EndGameAction(gameAction);
            if (gameAction is RevealChaosTokenAction revealChaosT && RevealChaosTokenExtraEffect(revealChaosT))
                new EffectAction(TakingHorror, TakeHorrorAnimation).AddActionTo();
        }
        bool RevealChaosTokenExtraEffect(RevealChaosTokenAction revealChaosTokenAction)
        {
            if (!attackingWithThiscard) return false;
            attackingWithThiscard = false;
            if (!((ChaosTokenType.Cultist | ChaosTokenType.Kthulu | ChaosTokenType.Skull | ChaosTokenType.Tablet | ChaosTokenType.Fail).HasFlag(revealChaosTokenAction.Token.Type))) return false;
            return true;
        }

        IEnumerator TakeHorrorAnimation() => new AnimationCardAction(ThisCard, audioClip: ThisCard.Effect2).RunNow();

        IEnumerator TakingHorror() => new AssignDamageHorror(GameControl.ActiveInvestigator, horrorAmount: 1).RunNow();

        protected override void SettingFight(CardComponent enemy)
        {
            base.SettingFight(enemy);
            attackingWithThiscard = true;
            attackToenemy.SkillTest.SkillType = Skill.Willpower;
        }

        protected override IEnumerator CancelCardEffect()
        {
            attackingWithThiscard = false;
            yield return base.CancelCardEffect();
        }
    }
}