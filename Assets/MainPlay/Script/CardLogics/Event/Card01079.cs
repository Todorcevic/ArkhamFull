using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01079 : CardEvent
    {
        protected override bool IsFast => true;
        protected override string nameCardEffect => "Descubrir pistas";

        /*****************************************************************************************/
        protected override void BeginGameAction(GameAction gameAction) { }

        protected override void EndGameAction(GameAction gameAction)
        {
            if (gameAction is SkillTestActionComplete skillTestComplete && CheckSkillTest(skillTestComplete))
                skillTestComplete.ChooseCardOptionalAction.ChosenCardEffects.Add(playFromHand = PlayFromHand);
        }

        bool CheckSkillTest(SkillTestActionComplete skillTestComplete)
        {
            if (!CheckPlayedFromHand()) return false;
            if (GameControl.ActiveInvestigator != ThisCard.VisualOwner) return false;
            if (skillTestComplete.SkillTest.SkillTestType != SkillTestType.Investigate) return false;
            if (!skillTestComplete.SkillTest.IsComplete ?? true) return false;
            if (skillTestComplete.SkillTest.IsWin) return false;
            if (skillTestComplete.SkillTest.TotalInvestigatorValue - skillTestComplete.SkillTest.TotalTestValue < -2) return false;
            if (ThisCard.VisualOwner.CurrentLocation.CluesToken.Amount < 1) return false;
            return true;
        }

        protected override IEnumerator LogicEffect() =>
            new DiscoverCluesAction(ThisCard.VisualOwner, 2).RunNow();

        protected override bool CheckFilterToCancel() => ThisCard.VisualOwner.CurrentLocation.CluesToken.Amount < 1;
    }
}