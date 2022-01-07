using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01085 : CardEvent, IBuffable
    {
        protected override bool IsFast => true;

        /*****************************************************************************************/
        protected override void BeginGameAction(GameAction gameAction)
        {
            base.BeginGameAction(gameAction);
            if (gameAction is RevealChaosTokenAction revealChaosToken && NoChaosToken())
                new EffectAction(() => CancelRevealchaosToken(revealChaosToken)).AddActionTo();
        }

        protected override void EndGameAction(GameAction gameAction)
        {
            if (gameAction is EndInvestigatorTurnAction) BuffActive = false;
        }

        bool NoChaosToken()
        {
            if (!BuffActive) return false;
            if (GameControl.ActiveInvestigator != ThisCard.VisualOwner) return false;
            return true;
        }

        protected override bool CheckPlayedFromHand()
        {
            if (!(GameControl.CurrentPhase is InvestigationPhase)) return false;
            if (GameControl.TurnInvestigator != ThisCard.VisualOwner) return false;
            return base.CheckPlayedFromHand();
        }

        protected override IEnumerator LogicEffect()
        {
            BuffActive = true;
            yield return null;
        }

        IEnumerator CancelRevealchaosToken(RevealChaosTokenAction revealChaosToken)
        {
            yield return new AnimationCardAction(ThisCard, isBuffEvent: true, audioClip: ThisCard.Effect2).RunNow();
            revealChaosToken.IsActionCanceled = true;
            revealChaosToken.SkillTest.IsWin = revealChaosToken.SkillTest.TotalInvestigatorValue >= revealChaosToken.SkillTest.TotalTestValue;
        }

        bool IBuffable.ActiveBuffCondition(GameAction gameAction) => BuffActive;

        void IBuffable.BuffEffect() => ThisCard.VisualOwner.InvestigatorCardComponent.CardTools.ShowBuff(ThisCard);

        void IBuffable.DeBuffEffect() => ThisCard.VisualOwner.InvestigatorCardComponent.CardTools.HideBuff(ThisCard.UniqueId.ToString());
    }
}