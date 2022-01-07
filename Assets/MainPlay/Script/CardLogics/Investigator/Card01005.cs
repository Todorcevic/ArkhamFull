using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace ArkhamGamePlay
{
    public class Card01005 : CardInvestigator
    {
        bool effectUsed;
        CardEffect newTokenCardEffect;
        CardComponent WendyAmulet => GameControl.GetCard("01014");
        public override int ChaosTokenWinValue() => 0;

        /*****************************************************************************************/
        protected override void EndGameAction(GameAction gameAction)
        {
            base.EndGameAction(gameAction);
            if (gameAction is RevealChaosTokenAction revealChaosT && ChangeToken())
                revealChaosT.ChooseCardOptionalAction.ChosenCardEffects.Add(newTokenCardEffect = new CardEffect(
                    card: ThisCard,
                    effect: () => ChangingToken(revealChaosT),
                    animationEffect: ChangingTokenAnimation,
                    payEffect: PayCostEffect,
                    type: EffectType.Reaction,
                    name: "Revelar una nuevo token",
                    investigatorImageCardInfoOwner: ThisCard.Owner));
            if (gameAction is SkillTestAction) effectUsed = false;
        }

        bool ChangeToken()
        {
            if (GameControl.ActiveInvestigator != ThisCard.Owner) return false;
            if (ThisCard.Owner.Hand.ListCards.FindAll(c => !c.IsWeakness).Count < 1) return false;
            if (effectUsed) return false;
            return true;
        }

        IEnumerator PayCostEffect()
        {
            ThisCard.MoveFast(ThisCard.CurrentZone);
            InvestigatorDiscardHand investigatorDiscard = new InvestigatorDiscardHand(ThisCard.Owner, isOptional: true);
            yield return investigatorDiscard.RunNow();
            newTokenCardEffect.IsCancel = investigatorDiscard.IsCanceled;
        }

        IEnumerator ChangingTokenAnimation() =>
            new AnimationCardAction(ThisCard, audioClip: ThisCard.Effect4).RunNow();

        IEnumerator ChangingToken(RevealChaosTokenAction revealChaosToken)
        {
            effectUsed = true;
            yield return new ReturnChaosTokenAction(revealChaosToken.Token).RunNow();
            SkillTest skillTest = revealChaosToken.SkillTest;
            yield return new RevealChaosTokenAction(ref skillTest).RunNow();
        }

        public override IEnumerator ChaosTokenWinEffect()
        {
            if (WendyAmulet.IsInPlay) GameControl.CurrentSkillTestAction.SkillTest.AutoResultWinLose = true;
            yield return null;
        }
    }
}