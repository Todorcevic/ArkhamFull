using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class CardSkill : CardLogic
    {
        protected override void BeginGameAction(GameAction gameAction)
        {
            if (gameAction is SkillTestActionComplete skillTestComplete && CheckSkillTestWin(skillTestComplete))
                skillTestComplete.SkillTest.WinEffect.Add(new CardEffect(
                    card: ThisCard,
                    effect: ThisCardEffect,
                    animationEffect: ThisCardAnimation,
                    type: EffectType.Choose,
                    name: "Activar este efecto",
                    investigatorImageCardInfoOwner: ThisCard.Owner));
        }

        protected virtual bool CheckSkillTestWin(SkillTestActionComplete skillTestComplete)
        {
            if (!skillTestComplete.SkillTest.Modifiers.Contains(ThisCard)) return false;
            if (!skillTestComplete.SkillTest.IsWin) return false;
            return true;
        }

        protected virtual IEnumerator ThisCardAnimation() => new AnimationCardAction(ThisCard, audioClip: ThisCard.Effect1).RunNow();

        protected virtual IEnumerator ThisCardEffect() => null;
    }
}