using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01027 : CardAsset, IBuffable
    {
        protected override void BeginGameAction(GameAction gameAction)
        {
            base.BeginGameAction(gameAction);
            if (gameAction is InteractableAction interactableAction && CheckPlayCardInGame(interactableAction))
                interactableAction.CardEffects.Add(new CardEffect(
                    card: ThisCard,
                    effect: ExtraTurns,
                    animationEffect: ExtraTurnAnimation,
                    payEffect: PayCostEffect,
                    cancelEffect: CancelCardEffect,
                    type: EffectType.Instead,
                    name: "Activar " + ThisCard.Info.Name,
                    investigatorImageCardInfoOwner: ThisCard.VisualOwner)
                    );
        }

        bool CheckPlayCardInGame(InteractableAction interactableAction)
        {
            if (!interactableAction.CanPlayFastAction) return false;
            if (!ThisCard.IsInPlay) return false;
            if (!GameControl.TurnInvestigator) return false;
            return true;
        }

        IEnumerator ExtraTurns() => new UpdateActionsLeft(GameControl.TurnInvestigator, 2).RunNow();

        IEnumerator ExtraTurnAnimation()
        {
            ThisCard.CardTools.PlayOneShotSound(ThisCard.Effect1);
            yield return new WaitWhile(() => ThisCard.CardTools.AudioSource.isPlaying);
        }

        IEnumerator PayCostEffect() => new DiscardAction(ThisCard).RunNow();

        IEnumerator CancelCardEffect() => new MoveCardAction(ThisCard, ThisCard.VisualOwner.Assets).RunNow();

        bool IBuffable.ActiveBuffCondition(GameAction gameAction) => ThisCard.IsInPlay;

        void IBuffable.BuffEffect()
        {
            ThisCard.VisualOwner.InvestigatorCardComponent.CardTools.ShowBuff(ThisCard);
            ThisCard.VisualOwner.WillpowerBonus++;
        }

        void IBuffable.DeBuffEffect()
        {
            ThisCard.VisualOwner.InvestigatorCardComponent.CardTools.HideBuff(ThisCard.UniqueId.ToString());
            ThisCard.VisualOwner.WillpowerBonus--;
        }
    }
}