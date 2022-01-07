using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01058 : CardAsset
    {
        protected override void BeginGameAction(GameAction gameAction)
        {
            base.BeginGameAction(gameAction);
            if (gameAction is InteractableAction interactableAction && CheckPlayCardInGame(interactableAction))
                interactableAction.CardEffects.Add(new CardEffect(
                    card: ThisCard,
                    effect: AddindResources,
                    animationEffect: AddingResourcesAnimation,
                    payEffect: PayCostEffect,
                    type: EffectType.Instead,
                    name: "Activar " + ThisCard.Info.Name,
                    investigatorImageCardInfoOwner: ThisCard.VisualOwner,
                    needExhaust: true)
                    );
        }

        protected override void EndGameAction(GameAction gameAction)
        {
            base.EndGameAction(gameAction);
            if (gameAction is AddTokenAction && CheckDiscardSecrets())
                new EffectAction(DiscardByNoResources, DiscardByNoResourceAnimation).AddActionTo();
        }

        bool CheckPlayCardInGame(InteractableAction interactableAction)
        {
            if (!interactableAction.CanPlayFastAction) return false;
            if (!ThisCard.IsInPlay) return false;
            return true;
        }

        bool CheckDiscardSecrets()
        {
            if (!ThisCard.IsInPlay) return false;
            if (ThisCard.ResourcesToken.Amount > 0) return false;
            if (ThisCard.IsDiscarting) return false;
            return true;
        }

        IEnumerator PayCostEffect() => new AssignDamageHorror(ThisCard.VisualOwner, horrorAmount: 1).RunNow();

        IEnumerator AddingResourcesAnimation() => new AnimationCardAction(ThisCard, audioClip: ThisCard.Effect1).RunNow();

        IEnumerator AddindResources() => new AddTokenAction(ThisCard.VisualOwner.InvestigatorCardComponent.ResourcesToken, 1, from: ThisCard.ResourcesToken).RunNow();

        IEnumerator DiscardByNoResourceAnimation()
        {
            ThisCard.CardTools.PlayOneShotSound(ThisCard.Effect2);
            yield return new WaitWhile(() => ThisCard.CardTools.AudioSource.isPlaying);
        }

        IEnumerator DiscardByNoResources() => new DiscardAction(ThisCard).RunNow();

        protected override IEnumerator PlayCardFromHand()
        {
            yield return base.PlayCardFromHand();
            yield return new AddTokenAction(ThisCard.ResourcesToken, 4).RunNow();
        }
    }
}