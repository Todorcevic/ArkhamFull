using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01080 : CardEvent
    {
        SkillTestResultAction skillTestResult;
        protected override bool IsFast => true;
        protected override string nameCardEffect => "Obtener bonificador";

        /*****************************************************************************************/
        protected override void BeginGameAction(GameAction gameAction)
        {
            if (gameAction is SkillTestResultAction skillTestResult && CheckSkillTest(skillTestResult))
                skillTestResult.ChooseCardOptionalAction.ChosenCardEffects.Add(playFromHand = PlayFromHand);
        }

        bool CheckSkillTest(SkillTestResultAction skillTestResult)
        {
            if (!CheckPlayedFromHand()) return false;
            if (GameControl.ActiveInvestigator != ThisCard.VisualOwner) return false;
            if (skillTestResult.SkillTest.TotalInvestigatorValue >= skillTestResult.SkillTest.TotalTestValue) return false;
            this.skillTestResult = skillTestResult;
            return true;
        }

        protected override IEnumerator LogicEffect()
        {
            AllComponents.PanelSkillTest.AddModifier(ThisCard, 2);
            yield return null;
        }

        protected override bool CheckFilterToCancel() => skillTestResult.SkillTest.TotalInvestigatorValue >= skillTestResult.SkillTest.TotalTestValue;
    }
}