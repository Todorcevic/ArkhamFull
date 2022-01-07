using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01076 : CardAsset
    {
        CardEffect cardEffect;

        /*****************************************************************************************/
        protected override void BeginGameAction(GameAction gameAction)
        {
            base.BeginGameAction(gameAction);
            if (gameAction is InteractableAction interactableAction && CheckPlayCardInGame(interactableAction))
                interactableAction.CardEffects.Add(cardEffect = new CardEffect(
                    card: ThisCard,
                    effect: AutoEvadeEffect,
                    animationEffect: AutoEvadeEffectAnimation,
                    payEffect: PayCostEffect,
                    type: EffectType.Instead,
                    name: "Activar " + ThisCard.Info.Name,
                    investigatorImageCardInfoOwner: ThisCard.VisualOwner)
                    );
        }

        bool CheckPlayCardInGame(InteractableAction interactableAction)
        {
            if (!interactableAction.CanPlayFastAction) return false;
            if (!ThisCard.IsInPlay) return false;
            if (ThisCard.VisualOwner.AllEnemiesInMyLocation.FindAll(c => ((CardEnemy)c.CardLogic).CanBeEvaded && !c.KeyWords.Contains("Elite")).Count < 1) return false;
            return true;
        }

        IEnumerator AutoEvadeEffectAnimation()
        {
            ThisCard.CardTools.PlayOneShotSound(ThisCard.Effect1);
            yield return new WaitWhile(() => ThisCard.CardTools.AudioSource.isPlaying);
        }

        IEnumerator AutoEvadeEffect()
        {
            List<CardEffect> cardEffects = new List<CardEffect>();
            foreach (CardComponent enemy in ThisCard.VisualOwner.AllEnemiesInMyLocation.FindAll(c => ((CardEnemy)c.CardLogic).CanBeEvaded && !c.KeyWords.Contains("Elite")))
                cardEffects.Add(new CardEffect(
                    card: enemy,
                    effect: () => Evade(enemy),
                    animationEffect: ((CardEnemy)enemy.CardLogic).BadEffectForEnemyAnimation,
                    type: EffectType.Choose,
                    name: "Evita a " + enemy.Info.Name,
                    investigatorImageCardInfoOwner: ThisCard.VisualOwner));
            yield return new ChooseCardAction(cardEffects, cancelableCardEffect: cardEffect).RunNow();
        }

        IEnumerator EvadeAnimation(CardComponent enemy) => new AnimationCardAction(enemy, audioClip: enemy.Effect7).RunNow();

        IEnumerator Evade(CardComponent enemy) => new EvadeEnemyAction(enemy).RunNow();

        IEnumerator PayCostEffect() => new DiscardAction(ThisCard).RunNow();
    }
}