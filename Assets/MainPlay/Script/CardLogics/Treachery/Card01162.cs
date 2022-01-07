using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01162 : CardTreachery
    {
        SkillTest skillTest;

        /*****************************************************************************************/
        public override IEnumerator Revelation()
        {
            skillTest = new SkillTest
            {
                Title = "Evitando " + ThisCard.Info.Name,
                SkillType = Skill.Agility,
                CardToTest = ThisCard,
                TestValue = 3
            };
            skillTest.LoseEffect.Add(new CardEffect(
                card: ThisCard,
                effect: LoseEffect,
                animationEffect: LoseEffectAnimation,
                type: EffectType.Choose,
                name: "Recibir daño"));
            yield return new SkillTestAction(skillTest).RunNow();
        }

        IEnumerator LoseEffectAnimation() => new AnimationCardAction(ThisCard, audioClip: ThisCard.Effect1).RunNow();

        IEnumerator LoseEffect()
        {
            int damage = skillTest.TotalTestValue - skillTest.TotalInvestigatorValue;
            yield return new AssignDamageHorror(GameControl.ActiveInvestigator, damageAmount: damage).RunNow();
        }
    }
}