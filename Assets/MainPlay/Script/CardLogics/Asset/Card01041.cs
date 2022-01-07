using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01041 : CardAsset
    {
        protected override void EndGameAction(GameAction gameAction)
        {
            base.EndGameAction(gameAction);
            if (gameAction is SpawnEnemyAction spawnEnemy && ReactionEffect(spawnEnemy))
                spawnEnemy.ChooseCardOptionalAction.ChosenCardEffects.Add(new CardEffect(
                    card: ThisCard,
                    effect: () => DiscardEnemy(spawnEnemy),
                    animationEffect: () => null,
                    payEffect: PayEffect,
                    checkFilterToCancel: () => CheckFilterToCancel(spawnEnemy),
                    type: EffectType.Reaction,
                    name: "Descartar a " + spawnEnemy.Enemy.Info.Name,
                    investigatorImageCardInfoOwner: ThisCard.VisualOwner));
        }
        bool ReactionEffect(SpawnEnemyAction spawnEnemy)
        {
            if (!ThisCard.IsInPlay) return false;
            if (((CardEnemy)spawnEnemy.Enemy.CardLogic).CurrentLocation != ThisCard.VisualOwner.CurrentLocation) return false;
            if (spawnEnemy.Enemy.KeyWords.Contains("Elite")) return false;
            return true;
        }

        IEnumerator DiscardEnemyAnimation() => new AnimationCardAction(ThisCard, audioClip: ThisCard.Effect1).RunNow();

        IEnumerator DiscardEnemy(SpawnEnemyAction spawnEnemy) => new DiscardAction(spawnEnemy.Enemy).RunNow();

        IEnumerator PayEffect()
        {
            yield return new AnimationCardAction(ThisCard, audioClip: ThisCard.Effect1).RunNow();
            yield return new DiscardAction(ThisCard).RunNow();
        }

        bool CheckFilterToCancel(SpawnEnemyAction spawnEnemy) =>
            ((CardEnemy)spawnEnemy.Enemy.CardLogic).CurrentLocation != ThisCard.VisualOwner.CurrentLocation;
    }
}