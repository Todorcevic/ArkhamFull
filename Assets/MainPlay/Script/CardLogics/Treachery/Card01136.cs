using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01136 : CardTreachery
    {
        bool isSurge;
        SkillTest skillTest;
        InvestigatorComponent investigator => GameControl.ActiveInvestigator;
        public override bool IsSurge => isSurge;

        /*****************************************************************************************/
        public override IEnumerator Revelation()
        {
            CardEffect lossing = new CardEffect(
                card: ThisCard,
                effect: LossingClues,
                animationEffect: LossingCluesAnimation,
                type: EffectType.Choose,
                name: "Soltar pistas"
                );

            skillTest = new SkillTest
            {
                Title = "Resolviendo " + ThisCard.Info.Name,
                SkillType = Skill.Intellect,
                CardToTest = ThisCard,
                TestValue = 4,
                IsOptional = false
            };
            skillTest.LoseEffect.Add(lossing);

            if (investigator.Clues < 1) isSurge = true;
            else yield return new SkillTestAction(skillTest).RunNow();
        }

        IEnumerator LossingCluesAnimation() => new AnimationCardAction(ThisCard, audioClip: ThisCard.Effect1).RunNow();

        IEnumerator LossingClues()
        {
            int cluesToLose = skillTest.TotalTestValue - skillTest.TotalInvestigatorValue;
            cluesToLose = investigator.Clues < cluesToLose ? investigator.Clues : cluesToLose;
            yield return new AddTokenAction(investigator.CurrentLocation.CluesToken, cluesToLose, from: investigator.InvestigatorCardComponent.CluesToken).RunNow();
        }
    }
}
