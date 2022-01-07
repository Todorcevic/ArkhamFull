using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace ArkhamGamePlay
{
    public class Card01008 : CardAsset, IBuffable
    {
        readonly int extraHandSlots = 2;
        int tomesAmount;
        CheckControlAssetSlotsAction checkAsset;
        int TomesAmount => ThisCard.VisualOwner.ListingCardsInMyZone().FindAll(c => c.IsInPlay && c.KeyWords.Contains("Tome") && c.Info.Slot.Contains("Hand")).Count;

        /*****************************************************************************************/
        protected override void BeginGameAction(GameAction gameAction)
        {
            base.BeginGameAction(gameAction);
            if (gameAction is CheckDiscardAssetsAction choosediscard && CheckSlotsAddTomeAssets(choosediscard))
                if (choosediscard.CardToPlay.KeyWords.Contains("Tome"))
                    choosediscard.NeedThisAmountSlots -= extraHandSlots;
                else
                {
                    int tomesAmount = ThisCard.VisualOwner.Assets.ListCards.FindAll(c => c.Info.Slot != null && c.Info.Slot.Contains("Hand") && c.KeyWords.Contains("Tome")).Count
                        + ThisCard.VisualOwner.Threat.ListCards.FindAll(c => c.Info.Slot != null && c.Info.Slot.Contains("Hand") && c.KeyWords.Contains("Tome")).Count;
                    choosediscard.NeedThisAmountSlots -= tomesAmount > extraHandSlots ? extraHandSlots : tomesAmount;
                    if (tomesAmount <= extraHandSlots)
                    {
                        choosediscard.AllAssetsForThisSlot = ThisCard.Owner.Assets.ListCards.FindAll(c => c.Info.Slot != null && c.Info.Slot.Contains("Hand") && !c.KeyWords.Contains("Tome"));
                        choosediscard.AllAssetsForThisSlot.AddRange(ThisCard.Owner.Threat.ListCards.FindAll(c => c.Info.Slot != null && c.Info.Slot.Contains("Hand") && !c.KeyWords.Contains("Tome")));
                    }
                }

            if (gameAction is CheckControlAssetSlotsAction checkControlAsset && CheckSlots(checkControlAsset))
            {
                tomesAmount = TomesAmount;
                ThisCard.VisualOwner.Slots.HandSlot += tomesAmount;
                checkAsset = checkControlAsset;
            }
        }

        protected override void EndGameAction(GameAction gameAction)
        {
            base.BeginGameAction(gameAction);
            if (gameAction is CheckControlAssetSlotsAction checkControlAsset && checkAsset == checkControlAsset)
                ThisCard.VisualOwner.Slots.HandSlot -= TomesAmount;
        }

        bool CheckSlotsAddTomeAssets(CheckDiscardAssetsAction choosediscard)
        {
            if (!ThisCard.IsInPlay) return false;
            if (choosediscard.Investigator != ThisCard.Owner) return false;
            if (choosediscard.SlotType != "Hand") return false;
            return true;
        }

        bool CheckSlots(CheckControlAssetSlotsAction checkControlAsset)
        {
            if (!ThisCard.IsInPlay) return false;
            if (checkControlAsset.Investigator != ThisCard.VisualOwner) return false;
            return true;
        }

        public bool ActiveBuffCondition(GameAction gameAction) => ThisCard.IsInPlay;

        public void BuffEffect() => ThisCard.VisualOwner.InvestigatorCardComponent.CardTools.ShowBuff(ThisCard);

        public void DeBuffEffect() => ThisCard.VisualOwner.InvestigatorCardComponent.CardTools.HideBuff(ThisCard.UniqueId.ToString());
    }
}