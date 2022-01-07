using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using System.Linq;
using DG.Tweening;

namespace ArkhamGamePlay
{
    public class Card01036 : CardEvent, IBuffable
    {
        protected override bool IsFast => true;

        /*****************************************************************************************/
        protected override void BeginGameAction(GameAction gameAction)
        {
            base.BeginGameAction(gameAction);
            if (gameAction is SkillTestAction skillTestAction && ChangeSkill(skillTestAction))
            {
                skillTestAction.ChooseCardOptionalAction.ChosenCardEffects.Add(new CardEffect(
                    card: ThisCard,
                    effect: () => ChangeSkillTest(skillTestAction),
                    animationEffect: ChangeSkillAnimation,
                    type: EffectType.Choose,
                    name: "Cambiar habilidad a esta prueba"));
            }
        }

        protected override void EndGameAction(GameAction gameAction)
        {
            if (gameAction is UpkeepPhase) BuffActive = false;
        }

        bool ChangeSkill(SkillTestAction skillTestAction)
        {
            if (!BuffActive) return false;
            if (GameControl.CurrentSkillTestAction == skillTestAction) return false;
            if (ThisCard.VisualOwner != skillTestAction.ActiveInvestigator) return false;
            if (!((Skill.Agility | Skill.Combat).HasFlag(skillTestAction.SkillTest.SkillType))) return false;
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

        IEnumerator ChangeSkillAnimation() => new AnimationCardAction(ThisCard, isBuffEvent: true, audioClip: ThisCard.Effect2).RunNow();

        IEnumerator ChangeSkillTest(SkillTestAction skillTestAction)
        {
            skillTestAction.SkillTest.SkillType = Skill.Intellect;
            yield return null;
        }

        bool IBuffable.ActiveBuffCondition(GameAction gameAction) => BuffActive;

        void IBuffable.BuffEffect() => ThisCard.VisualOwner.InvestigatorCardComponent.CardTools.ShowBuff(ThisCard);

        void IBuffable.DeBuffEffect() => ThisCard.VisualOwner.InvestigatorCardComponent.CardTools.HideBuff(ThisCard.UniqueId.ToString());
    }
}