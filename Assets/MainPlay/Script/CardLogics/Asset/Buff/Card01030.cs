using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01030 : CardAsset, IBuffable
    {
        protected override bool IsFast => true;

        /*****************************************************************************************/
        bool IBuffable.ActiveBuffCondition(GameAction gameAction) =>
            ThisCard.IsInPlay &&
            GameControl.ActiveInvestigator == ThisCard.VisualOwner &&
            GameControl.CurrentSkillTestAction?.SkillTest.SkillTestType == SkillTestType.Investigate;

        void IBuffable.BuffEffect()
        {
            ThisCard.VisualOwner.InvestigatorCardComponent.CardTools.ShowBuff(ThisCard);
            ThisCard.VisualOwner.IntellectBonus++;
        }

        void IBuffable.DeBuffEffect()
        {
            ThisCard.VisualOwner.InvestigatorCardComponent.CardTools.HideBuff(ThisCard.UniqueId.ToString());
            ThisCard.VisualOwner.IntellectBonus--;
        }
    }
}