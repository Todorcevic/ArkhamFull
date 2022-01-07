using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01053 : CardSkill
    {
        bool effectIsActive;

        protected override void BeginGameAction(GameAction gameAction)
        {
            if (gameAction is CardEffectsFilterAction cardEffectFilter && CheckOnlyYourTest())
                cardEffectFilter.CardEffects.RemoveAll(c => c.Card.ID == ThisCard.ID);
            if (gameAction is SkillTestActionComplete skillTestComplete && CheckSkillTestWin(skillTestComplete))
                effectIsActive = true;
            if (gameAction is DiscardAction discardAction && CheckReturnCard(discardAction))
                new EffectAction(() => ReturnCard(discardAction), ReturnCardAnimation).AddActionTo();
        }

        bool CheckOnlyYourTest()
        {
            if (!(GameControl.CurrentInteractableAction is SkillTestAction)) return false;
            if (ThisCard.VisualOwner == GameControl.ActiveInvestigator) return false;
            return true;
        }

        bool CheckReturnCard(DiscardAction discardAction)
        {
            if (!effectIsActive) return false;
            if (discardAction.ThisCard != ThisCard) return false;
            return true;
        }

        protected override bool CheckSkillTestWin(SkillTestActionComplete skillTestComplete)
        {
            if (skillTestComplete.SkillTest.TotalInvestigatorValue - skillTestComplete.SkillTest.TotalTestValue < 3) return false;
            return base.CheckSkillTestWin(skillTestComplete);
        }

        IEnumerator ReturnCardAnimation() => new AnimationCardAction(ThisCard, withReturn: false, audioClip: ThisCard.Effect1).RunNow();

        IEnumerator ReturnCard(DiscardAction discardAction)
        {
            discardAction.IsActionCanceled = true;
            effectIsActive = false;
            yield return new MoveCardAction(ThisCard, ThisCard.Owner.Hand, withPreview: false).RunNow();
        }
    }
}