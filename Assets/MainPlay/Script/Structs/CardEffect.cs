using System;

namespace ArkhamGamePlay
{
    public class CardEffect
    {
        public string Name { get; set; }
        public int ActionCost { get; set; }
        public int ResourceCost { get; set; }
        public bool IsCancel { get; set; }
        public bool IsCancelable { get; set; } = true;
        public bool NeedExhaust { get; set; }
        public bool NeedSpendClues { get; set; }
        public bool IsWithAnimation => AnimationEffect != null;
        public Func<bool> CheckFilterToCancel { get; set; }
        public CardComponent Card { get; set; }
        public Effect PayCostEffect { get; set; }
        public Effect CancelEffect { get; set; }
        public Effect Effect { get; set; }
        public Effect AnimationEffect { get; set; }
        public EffectType Type { get; set; }
        public InvestigatorComponent PlayOwner { get; set; }
        public InvestigatorComponent RealVisualOwner { get; set; }

        public CardEffect(CardComponent card, Effect effect, EffectType type, string name = null, int actionCost = 0, int resourceCost = 0, bool needExhaust = false, bool needSpendClues = false, InvestigatorComponent investigatorImageCardInfoOwner = null, InvestigatorComponent investigatorRealOwner = null, Effect payEffect = null, Effect cancelEffect = null, Effect animationEffect = null, Func<bool> checkFilterToCancel = null)
        {
            Card = card;
            Effect = effect;
            PayCostEffect = payEffect;
            CancelEffect = cancelEffect;
            Type = type;
            Name = name;
            ActionCost = actionCost;
            ResourceCost = resourceCost;
            NeedExhaust = needExhaust;
            NeedSpendClues = needSpendClues;
            AnimationEffect = animationEffect;
            PlayOwner = investigatorImageCardInfoOwner ?? GameControl.ActiveInvestigator ?? GameControl.LeadInvestigator;
            RealVisualOwner = investigatorRealOwner ?? (PlayOwner != card.VisualOwner ? card.VisualOwner : null);
            CheckFilterToCancel = checkFilterToCancel;
        }

        public string TakeEffectTypeIcon()
        {
            string icon = string.Empty;
            if (Type.HasFlag(EffectType.Activate)) icon = "\u00fd";
            else if ((EffectType.Play | EffectType.Draw | EffectType.Engange | EffectType.Evade | EffectType.Fight | EffectType.Investigate | EffectType.Move | EffectType.Resource).HasFlag(Type))
                icon = "(\u00fd)";
            else if (Type.HasFlag(EffectType.Instead)) icon = "\u00fe";
            else if (Type.HasFlag(EffectType.Fast)) icon = "(\u00fe)";
            else if (Type.HasFlag(EffectType.Reaction)) icon = "\u00ff";
            for (int i = 1; i < ActionCost; i++)
                icon += icon;
            return "<b>" + icon + "</b> ";
        }
    }
}