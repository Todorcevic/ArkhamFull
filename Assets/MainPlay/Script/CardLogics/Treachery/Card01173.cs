using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01173 : CardTreachery
    {
        SkillTest skillTest;
        InvestigatorComponent investigator => GameControl.ActiveInvestigator;

        /*****************************************************************************************/
        public override IEnumerator Revelation()
        {
            skillTest = new SkillTest
            {
                Title = "Soportando " + ThisCard.Info.Name,
                SkillType = Skill.Agility,
                CardToTest = ThisCard,
                TestValue = 4
            };
            skillTest.LoseEffect.Add(new CardEffect(
                card: ThisCard,
                effect: LoseEffect,
                animationEffect: LoseEffectAnimation,
                type: EffectType.Choose,
                name: "Recibir efecto de " + ThisCard.Info.Name));
            yield return new SkillTestAction(skillTest).RunNow();
        }

        IEnumerator LoseEffectAnimation() => new AnimationCardAction(ThisCard, audioClip: ThisCard.Effect1).RunNow();

        IEnumerator LoseEffect()
        {
            CardComponent centralLocation = GameControl.GetCard("01125"); // Realy need a ChooseCardAction?
            List<CardComponent> enemiesEnganged = investigator.AllEnemiesEnganged.FindAll(e => !e.KeyWords.Contains("Nightgaunt"));
            yield return new AssignDamageHorror(investigator, damageAmount: 1, horrorAmount: 1).RunNow();
            foreach (CardComponent enemy in enemiesEnganged)
                enemy.MoveTo(investigator.PlayCard.CurrentZone);
            yield return new MoveCardAction(GameControl.ActiveInvestigator.PlayCard, centralLocation.MyOwnZone).RunNow();
            foreach (CardComponent enemy in enemiesEnganged)
                yield return new MoveCardAction(enemy, enemy.CurrentZone, withPreview: false).RunNow();
        }
    }
}
