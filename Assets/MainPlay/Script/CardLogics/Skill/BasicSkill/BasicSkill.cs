using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class BasicSkill : CardSkill
    {
        protected override void BeginGameAction(GameAction gameAction)
        {
            base.BeginGameAction(gameAction);
            if (gameAction is CardEffectsFilterAction cardEffectFilter && CheckOnlyOneCard())
                cardEffectFilter.CardEffects.RemoveAll(c => c.Card.ID == ThisCard.ID && !GameControl.CurrentSkillTestAction.SkillTest.Modifiers.Contains(c.Card));
        }

        bool CheckOnlyOneCard()
        {
            if (!(GameControl.CurrentInteractableAction is SkillTestAction skillTestAction)) return false;
            if (!(skillTestAction.SkillTest.Modifiers.Exists(c => c.ID == ThisCard.ID))) return false;
            return true;
        }

        protected override IEnumerator ThisCardEffect()
        {
            yield return base.ThisCardEffect();
            yield return new DrawAction(ThisCard.Owner).RunNow();
        }
    }
}