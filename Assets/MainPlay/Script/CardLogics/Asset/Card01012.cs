using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01012 : CardAsset
    {
        protected override void EndGameAction(GameAction gameAction)
        {
            if (gameAction is ExecuteCardAction playCardAction && CheckCanPlay(playCardAction))
                playCardAction.ChooseCardOptionalAction.ChosenCardEffects.Add(new CardEffect(
                    card: ThisCard,
                    effect: Drawing,
                    animationEffect: DrawingAnimation,
                    type: EffectType.Reaction,
                    name: "Robar una carta",
                    investigatorImageCardInfoOwner: ThisCard.VisualOwner));
        }

        bool CheckCanPlay(ExecuteCardAction playCardaction)
        {
            if (ThisCard.CurrentZone != playCardaction.Investigator.Assets) return false;
            if (!playCardaction.CardEffect.Type.HasFlag(EffectType.Play) && !playCardaction.CardEffect.Type.HasFlag(EffectType.Fast)) return false;
            if (!playCardaction.CardEffect.Card.KeyWords.Contains("Spell")) return false;
            return true;
        }

        IEnumerator DrawingAnimation() => new AnimationCardAction(ThisCard, audioClip: ThisCard.Effect1).RunNow();

        IEnumerator Drawing() => new DrawAction(ThisCard.VisualOwner).RunNow();
    }
}