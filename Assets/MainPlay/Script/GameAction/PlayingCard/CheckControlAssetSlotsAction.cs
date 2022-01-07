using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class CheckControlAssetSlotsAction : GameAction
    {
        public InvestigatorComponent Investigator { get; set; }
        List<string> allSlots = new List<string> { "Hand", "Ally", "Arcane", "Body", "Accessory" };

        /*****************************************************************************************/
        public CheckControlAssetSlotsAction(InvestigatorComponent investigator) => Investigator = investigator;

        /*****************************************************************************************/
        protected override IEnumerator ActionLogic()
        {
            if (Investigator.IsDefeat) yield break;
            foreach (string slotType in allSlots.FindAll(s => Investigator.Slots.AmountFreeSlot(s) < 0))
            {
                List<CardComponent> allAssetsForThisSlot = Investigator.Assets.ListCards.FindAll(c => c.Info.Slot != null && c.Info.Slot.Contains(slotType));
                allAssetsForThisSlot.AddRange(Investigator.Threat.ListCards.FindAll(c => c.Info.Slot != null && c.Info.Slot.Contains(slotType)));
                List<CardEffect> cardEffects = new List<CardEffect>();
                foreach (CardComponent assetToDiscard in allAssetsForThisSlot.FindAll(c => c.CanBeDiscard))
                    cardEffects.Add(new CardEffect(
                        card: assetToDiscard,
                        effect: () => new DiscardAction(assetToDiscard).RunNow(),
                        type: EffectType.Choose,
                        name: "Descartar: " + assetToDiscard.Info.Name));
                if (cardEffects.Count > 0) yield return new ChooseCardAction(cardEffects, isOptionalChoice: false).RunNow();
            }
        }
    }
}