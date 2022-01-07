using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01033 : CardAsset, IBuffable
    {
        protected override void EndGameAction(GameAction gameAction)
        {
            base.EndGameAction(gameAction);
            if (gameAction is SkillTestActionResolution skillTestResolution && ActivateEffect(skillTestResolution))
                skillTestResolution.ChooseCardOptionalAction.ChosenCardEffects.Add(new CardEffect(
                    card: ThisCard,
                    effect: TakingResource,
                    animationEffect: TakingResourceAnimation,
                    type: EffectType.Reaction,
                    name: "Obtener recursos",
                    investigatorImageCardInfoOwner: ThisCard.VisualOwner)
                    );
        }

        bool ActivateEffect(SkillTestActionResolution skillTestResolution)
        {
            if (!ThisCard.IsInPlay) return false;
            if (ThisCard.VisualOwner != GameControl.ActiveInvestigator) return false;
            if (skillTestResolution.SkillTest.SkillTestType != SkillTestType.Investigate) return false;
            if (!skillTestResolution.SkillTest.IsWin) return false;
            return true;
        }

        IEnumerator TakingResourceAnimation() =>
            new AnimationCardAction(ThisCard, audioClip: ThisCard.Effect1).RunNow();

        IEnumerator TakingResource() =>
            new AddTokenAction(ThisCard.VisualOwner.InvestigatorCardComponent.ResourcesToken, 1).RunNow();


        bool IBuffable.ActiveBuffCondition(GameAction gameAction) => ThisCard.IsInPlay;

        void IBuffable.BuffEffect()
        {
            ThisCard.VisualOwner.InvestigatorCardComponent.CardTools.ShowBuff(ThisCard);
            ThisCard.VisualOwner.IntellectBonus++;
        }

        void IBuffable.DeBuffEffect()
        {
            ThisCard.VisualOwner.InvestigatorCardComponent.CardTools.HideBuff(ThisCard.UniqueId.ToString());
            ThisCard.VisualOwner.IntellectBonus--;
        }
    }
}