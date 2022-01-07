using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01075 : CardAsset
    {
        protected override void BeginGameAction(GameAction gameAction)
        {
            base.BeginGameAction(gameAction);
            if (gameAction is SkillTestActionComplete skillTestComplete && ActivateEffect(skillTestComplete))
                skillTestComplete.ChooseCardOptionalAction.ChosenCardEffects.Add(new CardEffect(
                    card: ThisCard,
                    effect: Drawing,
                    animationEffect: DrawingAnimation,
                    type: EffectType.Reaction,
                    needExhaust: true,
                    name: "Robar carta",
                    investigatorImageCardInfoOwner: ThisCard.VisualOwner));
        }

        bool ActivateEffect(SkillTestActionComplete skillTestComplete)
        {
            if (!ThisCard.IsInPlay) return false;
            if (ThisCard.VisualOwner != GameControl.ActiveInvestigator) return false;
            if (!skillTestComplete.SkillTest.IsComplete ?? true) return false;
            if (skillTestComplete.SkillTest.IsWin) return false;
            return true;
        }

        IEnumerator DrawingAnimation() => new AnimationCardAction(ThisCard, audioClip: ThisCard.Effect1).RunNow();

        IEnumerator Drawing() => new DrawAction(ThisCard.VisualOwner).RunNow();
    }
}