using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01046 : CardAsset
    {
        protected override void EndGameAction(GameAction gameAction)
        {
            base.EndGameAction(gameAction);
            if (gameAction is EvadeEnemyAction evadeEnemyAction && CheckEnemy())
                evadeEnemyAction.ChooseCardOptionalAction.ChosenCardEffects.Add(new CardEffect(
                    card: ThisCard,
                    effect: ReactionEffect,
                    animationEffect: ReactionAnimation,
                    type: EffectType.Reaction,
                    name: "Robar carta",
                    investigatorImageCardInfoOwner: ThisCard.VisualOwner,
                    needExhaust: true)
                    );
        }

        bool CheckEnemy()
        {
            if (!ThisCard.IsInPlay) return false;
            if (ThisCard.VisualOwner != GameControl.ActiveInvestigator) return false;
            return true;
        }

        IEnumerator ReactionAnimation() => new AnimationCardAction(ThisCard, audioClip: ThisCard.Effect1).RunNow();

        IEnumerator ReactionEffect() => new DrawAction(ThisCard.VisualOwner).RunNow();

    }
}