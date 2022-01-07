using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace ArkhamGamePlay
{
    public abstract class BasicTalent : CardAsset
    {
        protected abstract Skill SkillToTest { get; }
        AudioClip skillSound;

        /*****************************************************************************************/
        protected override void BeginGameAction(GameAction gameAction)
        {
            base.BeginGameAction(gameAction);
            if (gameAction is SkillTestAction skillTestAction && CheckPlayCardInGame(skillTestAction))
            {
                skillSound = TakeSoundEffect(skillTestAction.SkillTest.SkillType);
                skillTestAction.CardEffects.Add(new CardEffect(
                    card: ThisCard,
                    effect: SkillTestBonusEffect,
                    animationEffect: AnimationSkillTestBonusEffect,
                    type: EffectType.Instead,
                    name: "Activar " + ThisCard.Info.Name,
                    investigatorImageCardInfoOwner: ThisCard.VisualOwner,
                    resourceCost: 1
                    ));
            }
        }

        bool CheckPlayCardInGame(SkillTestAction skillTestAction)
        {
            if (!ThisCard.IsInPlay) return false;
            if (GameControl.ActiveInvestigator != ThisCard.VisualOwner) return false;
            if (!SkillToTest.HasFlag(skillTestAction.SkillTest.SkillType)) return false;
            return true;
        }

        IEnumerator AnimationSkillTestBonusEffect() =>
            new AnimationCardAction(ThisCard, audioClip: skillSound).RunNow();

        IEnumerator SkillTestBonusEffect()
        {
            AllComponents.PanelSkillTest.AddModifier(ThisCard, 1);
            yield return null;
        }

        AudioClip TakeSoundEffect(Skill skill)
        {
            switch (skill)
            {
                case Skill.Willpower: return ThisCard.Effect1;
                case Skill.Intellect: return ThisCard.Effect2;
                case Skill.Combat: return ThisCard.Effect3;
                case Skill.Agility: return ThisCard.Effect4;
                default: return null;
            }
        }
    }
}