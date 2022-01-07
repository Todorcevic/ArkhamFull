using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01098 : InvestigatorTreachery, IBuffable
    {
        public override bool IsDiscarted => false;

        /*****************************************************************************************/
        protected override void BeginGameAction(GameAction gameAction)
        {
            if (gameAction is InvestigatorTurn investigatorTurn && Discard(investigatorTurn))
                investigatorTurn.CardEffects.Add(new CardEffect(
                    card: ThisCard,
                    effect: () => new DiscardAction(ThisCard, withTopUp: false).RunNow(),
                    animationEffect: DiscardAnimation,
                    type: EffectType.Activate,
                    name: "Descartar " + ThisCard.Info.Name,
                    actionCost: 2));
        }

        bool Discard(InvestigatorTurn investigatorTurn)
        {
            if (!ThisCard.IsInPlay) return false;
            if (Investigator.CurrentLocation != investigatorTurn.ActiveInvestigator.CurrentLocation) return false;
            return true;
        }

        IEnumerator DiscardAnimation() =>
            new AnimationCardAction(ThisCard, withReturn: false, audioClip: ThisCard.Effect1).RunNow();

        public override IEnumerator Revelation() => new MoveCardAction(ThisCard, Investigator.Threat, withPreview: false).RunNow();

        bool IBuffable.ActiveBuffCondition(GameAction gameAction) => ThisCard.IsInPlay;

        void IBuffable.BuffEffect()
        {
            Investigator.InvestigatorCardComponent.CardTools.ShowBuff(ThisCard);
            Investigator.AgilityBonus--;
            Investigator.WillpowerBonus--;
            Investigator.CombatBonus--;
            Investigator.IntellectBonus--;
        }

        void IBuffable.DeBuffEffect()
        {
            Investigator.InvestigatorCardComponent.CardTools.HideBuff(ThisCard.UniqueId.ToString());
            Investigator.AgilityBonus++;
            Investigator.WillpowerBonus++;
            Investigator.CombatBonus++;
            Investigator.IntellectBonus++;
        }
    }
}