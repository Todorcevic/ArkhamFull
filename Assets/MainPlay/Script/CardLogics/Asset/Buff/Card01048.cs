using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01048 : CardAsset, IBuffable
    {
        bool IBuffable.ActiveBuffCondition(GameAction gameAction) => ThisCard.IsInPlay && GameControl.TurnInvestigator == ThisCard.VisualOwner;

        void IBuffable.BuffEffect()
        {
            ThisCard.VisualOwner.InvestigatorCardComponent.CardTools.ShowBuff(ThisCard);
            new UpdateActionsLeft(ThisCard.VisualOwner, 1).AddActionTo();
        }

        void IBuffable.DeBuffEffect()
        {
            ThisCard.VisualOwner.InvestigatorCardComponent.CardTools.HideBuff(ThisCard.UniqueId.ToString());
            new UpdateActionsLeft(ThisCard.VisualOwner, -1).AddActionTo();
        }
    }
}