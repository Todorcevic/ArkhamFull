using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01018 : CardAsset, IBuffable
    {
        int healthTokensLeft;
        CardEffect activateEffect;
        protected virtual bool NeedExhaust => false;

        /*****************************************************************************************/
        protected override void BeginGameAction(GameAction gameAction)
        {
            base.BeginGameAction(gameAction);
            if (gameAction is InteractableAction interactableAction && CheckPlayCardInGame(interactableAction))
                interactableAction.CardEffects.Add(activateEffect = new CardEffect(
                    card: ThisCard,
                    effect: ChooseEnemy,
                    animationEffect: ChooseEnemyAnimation,
                    payEffect: PayCostEffect,
                    cancelEffect: CancelCardEffect,
                    needExhaust: NeedExhaust,
                    type: EffectType.Instead,
                    name: "Activar " + ThisCard.Info.Name,
                    investigatorImageCardInfoOwner: ThisCard.VisualOwner)
                    );
        }

        protected virtual bool CheckPlayCardInGame(InteractableAction interactableAction)
        {
            if (!interactableAction.CanPlayFastAction) return false;
            if (!ThisCard.IsInPlay) return false;
            if (ThisCard.VisualOwner.AllEnemiesInMyLocation.Count < 1) return false;
            return true;
        }

        protected virtual IEnumerator ChooseEnemyAnimation()
        {
            ThisCard.CardTools.PlayOneShotSound(ThisCard.Effect1);
            yield return new WaitWhile(() => ThisCard.CardTools.AudioSource.isPlaying);
        }

        IEnumerator ChooseEnemy()
        {
            List<CardEffect> cardEffects = new List<CardEffect>();
            foreach (CardComponent enemy in ThisCard.VisualOwner.AllEnemiesInMyLocation)
                cardEffects.Add(new CardEffect(
                    card: enemy,
                    effect: () => DoDamage(enemy),
                    animationEffect: ((CardEnemy)enemy.CardLogic).BadEffectForEnemyAnimation,
                    type: EffectType.Choose,
                    name: "Hacer daño a " + enemy.Info.Name,
                    investigatorImageCardInfoOwner: ThisCard.VisualOwner)
                    );
            yield return new ChooseCardAction(cardEffects, cancelableCardEffect: activateEffect).RunNow();
        }

        protected virtual IEnumerator PayCostEffect()
        {
            healthTokensLeft = ThisCard.HealthToken.Amount;
            yield return new DiscardAction(ThisCard).RunNow();
        }

        protected virtual IEnumerator CancelCardEffect()
        {
            yield return new MoveCardAction(ThisCard, ThisCard.VisualOwner.Assets).RunNow();
            yield return new AddTokenAction(ThisCard.HealthToken, healthTokensLeft).RunNow();
        }

        IEnumerator DoDamage(CardComponent enemy) => new DamageEnemyAction(enemy, 1).RunNow();

        bool IBuffable.ActiveBuffCondition(GameAction gameAction) => ThisCard.IsInPlay;

        void IBuffable.BuffEffect()
        {
            ThisCard.VisualOwner.InvestigatorCardComponent.CardTools.ShowBuff(ThisCard);
            ThisCard.VisualOwner.CombatBonus++;
        }

        void IBuffable.DeBuffEffect()
        {
            ThisCard.VisualOwner.InvestigatorCardComponent.CardTools.HideBuff(ThisCard.UniqueId.ToString());
            ThisCard.VisualOwner.CombatBonus--;
        }
    }
}