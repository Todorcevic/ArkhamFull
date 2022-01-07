using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01040 : Card01030
    {
        protected override void BeginGameAction(GameAction gameAction)
        {
            base.BeginGameAction(gameAction);
            if (gameAction is InteractableAction interactableAction && CheckPlayCardInGame(interactableAction))
                interactableAction.CardEffects.Add(new CardEffect(
                    card: ThisCard,
                    effect: ReturnGlass,
                    animationEffect: ReturnGlassAnimation,
                    type: EffectType.Instead,
                    name: "Activar " + ThisCard.Info.Name,
                    investigatorImageCardInfoOwner: ThisCard.VisualOwner));
        }

        bool CheckPlayCardInGame(InteractableAction interactableAction)
        {
            if (!interactableAction.CanPlayFastAction) return false;
            if (!ThisCard.IsInPlay) return false;
            if (ThisCard.VisualOwner.CurrentLocation.CardLogic.ThisCard.CluesToken.Amount > 0) return false;
            return true;
        }

        IEnumerator ReturnGlassAnimation() => new AnimationCardAction(ThisCard, withReturn: false, audioClip: ThisCard.Effect1).RunNow();

        IEnumerator ReturnGlass() => new MoveCardAction(ThisCard, ThisCard.VisualOwner.Hand, withPreview: false).RunNow();
    }
}