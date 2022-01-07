using System.Collections;
using System;

namespace ArkhamGamePlay
{
    public class InvestigateLocation : GameAction
    {
        public override GameActionType GameActionType => GameActionType.Compound;
        public SkillTest SkillTest { get; private set; }
        public CardComponent Location { get; set; }
        public Effect WinEffect { get; set; }

        /*****************************************************************************************/
        public InvestigateLocation(CardComponent location, bool isCancelable)
        {
            Location = location;
            WinEffect = () => new DiscoverCluesAction(GameControl.ActiveInvestigator, 1).RunNow();
            SkillTest = new SkillTest
            {
                Title = "Investigando " + Location.Info.Name,
                SkillType = Skill.Intellect,
                SkillTestType = SkillTestType.Investigate,
                CardToTest = Location,
                TestValue = ((CardLocation)Location.CardLogic).Shroud,
                IsOptional = isCancelable,
            };
        }

        /*****************************************************************************************/
        protected override IEnumerator ActionLogic()
        {
            SkillTest.WinEffect.Add(new CardEffect(
                card: Location,
                effect: WinEffect,
                type: EffectType.Choose,
                name: "Obtener pista"));
            yield return new SkillTestAction(SkillTest).RunNow();
        }

        protected override IEnumerator Animation() =>
            new AnimationCardAction(Location, audioClip: Location.ClipType1).RunNow();
    }
}