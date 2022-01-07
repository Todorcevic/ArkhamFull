using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class AddVictoryCardAction : GameAction
    {
        CardComponent card;
        string text;
        int xp;

        public AddVictoryCardAction(CardComponent card, string text = null, int xp = 0)
        {
            this.card = card;
            this.text = text;
            this.xp = xp;
        }

        /*****************************************************************************************/
        protected override IEnumerator ActionLogic()
        {
            AllComponents.PanelCampaign.AddVictoryCard(card, text, xp);
            yield return null;
        }
    }
}