using System.Collections;
using UnityEngine;
using System;

namespace ArkhamGamePlay
{
    public class SkillTestActionResolution : GameAction, IButtonClickable
    {
        bool anyIsClicked;
        SkillTest skillTest;
        public override GameActionType GameActionType => GameActionType.Compound;
        public SkillTest SkillTest => skillTest;

        /*****************************************************************************************/
        public SkillTestActionResolution(SkillTest skillTest) => this.skillTest = skillTest;

        /*****************************************************************************************/
        protected override sealed IEnumerator ActionLogic()
        {
            SetButton();
            yield return new SelectInvestigatorAction(GameControl.ActiveInvestigator).RunNow();
            yield return new RevealChaosTokenAction(ref skillTest).RunNow(); //PH.3
            if (SkillTest.CardToTest.IsInFullPlay && !GameControl.ActiveInvestigator.IsDefeat)
                yield return new SkillTestChaosTokenEffect(skillTest.TokenThrow).RunNow();//PH.4 
            if (SkillTest.CardToTest.IsInFullPlay && !GameControl.ActiveInvestigator.IsDefeat)
            {
                skillTest.UpdateModifier(skillTest.TokenThrow?.Value ?? 0);
                yield return new SkillTestResultAction(ref skillTest).RunNow();//PH.6
                AllComponents.PanelSkillTest.ShowResult(SkillTest);
                yield return new WaitUntil(() => anyIsClicked);
            }
            SetButton();
        }

        public void SetButton() => AllComponents.PanelSkillTest.SetReadyButton(this, state: ButtonState.Off);

        public void ReadyClicked(ReadyButton button)
        {
            anyIsClicked = true;
        }
    }
}