using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01132 : CardLocation
    {
        List<CardComponent> allysAssets = new List<CardComponent>();
        List<CardComponent> oldAllysAssets = new List<CardComponent>();
        public override LocationSymbol MySymbol => LocationSymbol.Moon;
        public override LocationSymbol MovePosibilities => LocationSymbol.Circle | LocationSymbol.Triangle;

        /*****************************************************************************************/
        protected override void EndGameAction(GameAction gameAction)
        {
            base.EndGameAction(gameAction);
            if (gameAction is MoveCardAction)
                BuffEffect();
        }

        public void BuffEffect()
        {
            allysAssets = GameControl.AllCardComponents.FindAll(c => c.CardType == CardType.Asset
            && c.KeyWords.Contains("Ally")
            && c.VisualOwner?.CurrentLocation == ThisCard
            && c.CurrentZone?.ZoneType == Zones.Hand);

            foreach (CardComponent ally in oldAllysAssets.Except(allysAssets))
            {
                ((CardAsset)ally.CardLogic).CostBonus += 2;
                ally.CardTools.HideBuff(ThisCard.UniqueId.ToString());
            }

            foreach (CardComponent ally in allysAssets.Except(oldAllysAssets))
            {
                ((CardAsset)ally.CardLogic).CostBonus -= 2;
                ally.CardTools.ShowBuff(ThisCard);
            }

            oldAllysAssets = allysAssets;
        }
    }
}
