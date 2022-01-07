using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01074 : Weapon
    {
        SkillTest skillTestThisFight;
        protected override int ResourceStarting => 0;
        protected override int ResourceCost => 0;
        protected override int ActionsCost => 1;
        protected override int Damage => ThisCard.VisualOwner.AttackDamage + 1;
        protected override int Bonus => 2;

        /*****************************************************************************************/
        protected override void EndGameAction(GameAction gameAction)
        {
            base.EndGameAction(gameAction);
            if (gameAction is RevealChaosTokenAction revealChaosToken && CheckBateIsBroken(revealChaosToken))
                new EffectAction(BreakingBate, BreakingBateAnimation).AddActionTo();
        }

        bool CheckBateIsBroken(RevealChaosTokenAction revealChaosToken)
        {
            if (revealChaosToken.SkillTest != skillTestThisFight) return false;
            if (!(ChaosTokenType.Skull | ChaosTokenType.Fail).HasFlag(revealChaosToken.Token.Type)) return false;
            return true;
        }

        protected override void SettingFight(CardComponent enemy)
        {
            base.SettingFight(enemy);
            skillTestThisFight = attackToenemy.SkillTest;
        }

        IEnumerator BreakingBateAnimation() => new AnimationCardAction(ThisCard, audioClip: ThisCard.Effect2).RunNow();

        IEnumerator BreakingBate()
        {
            skillTestThisFight.WinEffect.Add(new CardEffect(
                    card: ThisCard,
                    effect: () => new DiscardAction(ThisCard).RunNow(),
                    animationEffect: BreakingBateAnimation,
                    type: EffectType.Choose,
                    name: "Descartar " + ThisCard.name + " roto")
                    );
            yield return null;
        }
    }
}