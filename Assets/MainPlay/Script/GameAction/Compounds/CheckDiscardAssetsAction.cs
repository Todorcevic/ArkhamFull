using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class CheckDiscardAssetsAction : GameAction
    {
        readonly bool isConsult;
        public InvestigatorComponent Investigator { get; }
        public string SlotType { get; }
        public CardComponent CardToPlay { get; set; }
        public int NeedThisAmountSlots { get; set; } = 1;
        public bool NoFreeSlot { get; set; }
        public List<CardComponent> AllAssetsForThisSlot { get; set; }

        /*****************************************************************************************/
        public CheckDiscardAssetsAction(InvestigatorComponent investigator, CardComponent cardToPlay, bool isConsult = false)
        {
            this.isConsult = isConsult;
            CardToPlay = cardToPlay;
            Investigator = investigator;
            SlotType = cardToPlay.Info.Slot ?? string.Empty;
            if (SlotType == "Hand x2")
            {
                NeedThisAmountSlots = 2;
                SlotType = "Hand";
            }
            if (SlotType == "Arcane x2")
            {
                NeedThisAmountSlots = 2;
                SlotType = "Arcane";
            }
            AllAssetsForThisSlot = investigator.Assets.ListCards.FindAll(c => c.Info.Slot != null && c.Info.Slot.Contains(SlotType));
            AllAssetsForThisSlot.AddRange(investigator.Threat.ListCards.FindAll(c => c.Info.Slot != null && c.Info.Slot.Contains(SlotType)));
        }

        /*****************************************************************************************/
        protected override IEnumerator ActionLogic()
        {
            if (Investigator.IsDefeat) yield break;
            NoFreeSlot = Investigator.Slots.AmountFreeSlot(CardToPlay.Info.Slot) < NeedThisAmountSlots;
            if (NoFreeSlot && !isConsult)
            {
                CardToPlay.MoveFast(CardToPlay.CurrentZone);
                List<CardEffect> cardEffects = new List<CardEffect>();
                foreach (CardComponent assetToDiscard in AllAssetsForThisSlot.FindAll(c => c.CanBeDiscard))
                    cardEffects.Add(new CardEffect(
                        card: assetToDiscard,
                        effect: () => Discard(assetToDiscard),
                        type: EffectType.Choose,
                        name: "Descartar: " + assetToDiscard.Info.Name,
                        investigatorImageCardInfoOwner: assetToDiscard.VisualOwner));
                yield return new ChooseCardAction(cardEffects, isOptionalChoice: false, withPreview: cardEffects.Count > 1).RunNow();

                IEnumerator Discard(CardComponent assetToDiscard)
                {
                    yield return new DiscardAction(assetToDiscard).RunNow();
                    yield return new CheckDiscardAssetsAction(Investigator, CardToPlay).RunNow();
                }
            }
        }
    }
}