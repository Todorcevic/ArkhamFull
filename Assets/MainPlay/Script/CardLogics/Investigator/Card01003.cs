using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01003 : CardInvestigator
    {
        bool effectUsed;
        public override int ChaosTokenWinValue() => 2;

        /*****************************************************************************************/
        protected override void BeginGameAction(GameAction gameAction)
        {
            base.BeginGameAction(gameAction);
            if (gameAction is InteractableAction interactableAction && CheckPayExtraTurn(interactableAction))
                interactableAction.CardEffects.Add(new CardEffect(
                    card: ThisCard,
                    effect: ExtraAction,
                    animationEffect: ExtraActionAnimation,
                    type: EffectType.Instead,
                    name: "Tomar 1 acción adicional",
                    resourceCost: 2,
                    investigatorImageCardInfoOwner: ThisCard.Owner));
        }

        protected override void EndGameAction(GameAction gameAction)
        {
            base.EndGameAction(gameAction);
            if (gameAction is UpkeepPhase) effectUsed = false;
        }

        bool CheckPayExtraTurn(InteractableAction interactableAction)
        {
            if (GameControl.TurnInvestigator != ThisCard.Owner) return false;
            if (!interactableAction.CanPlayFastAction) return false;
            if (ThisCard.Owner.Resources < 2) return false;
            if (effectUsed) return false;
            return true;
        }

        IEnumerator ExtraActionAnimation() => new AnimationCardAction(ThisCard, audioClip: ThisCard.Effect4).RunNow();

        IEnumerator ExtraAction()
        {
            yield return new UpdateActionsLeft(ThisCard.Owner, 1).RunNow();
            effectUsed = true;
        }

        public override IEnumerator ChaosTokenWinEffect()
        {
            CardEffect cardEffect = new CardEffect(
                card: ThisCard,
                effect: TakingResources,
                animationEffect: TakingResourcesAnimation,
                type: EffectType.Choose,
                name: "Obtener recursos",
                investigatorImageCardInfoOwner: ThisCard.Owner);
            GameControl.CurrentSkillTestAction.SkillTest.WinEffect.Add(cardEffect);
            yield return null;

            IEnumerator TakingResourcesAnimation() => new AnimationCardAction(ThisCard, audioClip: ThisCard.Effect5).RunNow();

            IEnumerator TakingResources() => new AddTokenAction(ThisCard.ResourcesToken, 2).RunNow();
        }
    }
}