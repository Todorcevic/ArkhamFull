using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace ArkhamGamePlay
{
    public class ChooseMultiCast : ChooseCardAction
    {
        public ChooseMultiCast(List<CardEffect> cardEffects, bool isOptionalChoice) :
            base(cardEffects, isOptionalChoice)
        { }

        /*****************************************************************************************/
        protected override void MoveToOriginalzone(CardComponent card)
        {
            card.CurrentZone.ZoneBehaviour = realZone[card.CurrentZone];
            if (CardPlayed != null && CardPlayed != card) card.MoveFast(AllComponents.CardBuilder.Zone);
            else card.MoveFast(card.CurrentZone);
        }
    }
}