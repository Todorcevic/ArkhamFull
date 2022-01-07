using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class InvestigatorSlots
    {
        readonly InvestigatorComponent investigator;
        int AccessoryFreeSlot => AccessorySlot
            - investigator.Assets.ListCards.FindAll(c => c.Info.Slot == "Accessory").Count
            - investigator.Threat.ListCards.FindAll(c => c.Info.Slot == "Accessory").Count;
        int BodyFreeSlot => BodySlot
            - investigator.Assets.ListCards.FindAll(c => c.Info.Slot == "Body").Count
            - investigator.Threat.ListCards.FindAll(c => c.Info.Slot == "Body").Count;
        int AllyFreeSlot => AllySlot
            - investigator.Assets.ListCards.FindAll(c => c.Info.Slot == "Ally").Count
            - investigator.Threat.ListCards.FindAll(c => c.Info.Slot == "Ally").Count;
        int HandFreeSlot => HandSlot
            - investigator.Assets.ListCards.FindAll(c => c.Info.Slot == "Hand").Count
            - (investigator.Assets.ListCards.FindAll(c => c.Info.Slot == "Hand x2").Count * 2)
            - investigator.Threat.ListCards.FindAll(c => c.Info.Slot == "Hand").Count
            - (investigator.Threat.ListCards.FindAll(c => c.Info.Slot == "Hand x2").Count * 2);
        int ArcaneFreeSlot => ArcaneSlot
            - investigator.Assets.ListCards.FindAll(c => c.Info.Slot == "Arcane").Count
            - (investigator.Assets.ListCards.FindAll(c => c.Info.Slot == "Arcane x2").Count * 2)
            - investigator.Threat.ListCards.FindAll(c => c.Info.Slot == "Arcane").Count
            - (investigator.Threat.ListCards.FindAll(c => c.Info.Slot == "Arcane x2").Count * 2);
        public int AccessorySlot { get; set; } = 1;
        public int BodySlot { get; set; } = 1;
        public int AllySlot { get; set; } = 1;
        public int HandSlot { get; set; } = 2;
        public int ArcaneSlot { get; set; } = 2;

        /************************************************************************************************/
        public InvestigatorSlots(InvestigatorComponent investigator) => this.investigator = investigator;

        /************************************************************************************************/
        public int AmountFreeSlot(string slot)
        {
            switch (slot)
            {
                case "Hand": return HandFreeSlot;
                case "Ally": return AllyFreeSlot;
                case "Arcane": return ArcaneFreeSlot;
                case "Body": return BodyFreeSlot;
                case "Accessory": return AccessoryFreeSlot;
                case "Hand x2": return HandFreeSlot;
                case "Arcane x2": return ArcaneFreeSlot;
                default: return 100;
            }
        }
    }
}