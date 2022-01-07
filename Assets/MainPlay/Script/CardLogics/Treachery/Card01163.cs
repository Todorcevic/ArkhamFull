using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01163 : CardTreachery
    {
        SkillTest skillTest;

        /*****************************************************************************************/
        public override IEnumerator Revelation()
        {
            skillTest = new SkillTest
            {
                Title = "Soportando " + ThisCard.Info.Name,
                SkillType = Skill.Willpower,
                CardToTest = ThisCard,
                TestValue = 3
            };
            skillTest.LoseEffect.Add(new CardEffect(
                card: ThisCard,
                effect: LoseEffect,
                animationEffect: LoseEffectAnimation,
                type: EffectType.Choose,
                name: "Recibir horror"));
            yield return new SkillTestAction(skillTest).RunNow();
        }

        IEnumerator LoseEffectAnimation() => new AnimationCardAction(ThisCard, audioClip: ThisCard.Effect1).RunNow();

        IEnumerator LoseEffect()
        {
            int horror = skillTest.TotalTestValue - skillTest.TotalInvestigatorValue;
            yield return new AssignDamageHorror(GameControl.ActiveInvestigator, horrorAmount: horror).RunNow();
        }
    }
}