using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01021 : CardAsset
    {
        SettingEnemyAttackAction currentEnemyAttackAction;
        int currentDogDamage;

        protected override void BeginGameAction(GameAction gameAction)
        {
            base.BeginGameAction(gameAction);
            if (gameAction is SettingEnemyAttackAction enemyAttack && CheckToDamage(enemyAttack))
            {
                currentEnemyAttackAction = enemyAttack;
                currentDogDamage = ThisCard.HealthToken.Amount;
            }
        }

        protected override void EndGameAction(GameAction gameAction)
        {
            base.EndGameAction(gameAction);
            if (gameAction is SettingEnemyAttackAction enemyAttack && ReactionEffect(enemyAttack))
                enemyAttack.ChooseCardOptionalAction.ChosenCardEffects.Add(new CardEffect(
                    card: ThisCard,
                    effect: DoDogDamage,
                    animationEffect: DogDamageAnimation,
                    type: EffectType.Reaction,
                    name: "Dañar a " + enemyAttack.Enemy.Info.Name,
                    investigatorImageCardInfoOwner: ThisCard.VisualOwner));
        }

        bool CheckToDamage(SettingEnemyAttackAction enemyAttack)
        {
            if (!ThisCard.IsInPlay) return false;
            if (enemyAttack.Investigator != ThisCard.VisualOwner) return false;
            return true;
        }

        bool ReactionEffect(SettingEnemyAttackAction enemyAttack)
        {
            if (!enemyAttack.Equals(currentEnemyAttackAction)) return false;
            if (ThisCard.IsInPlay && currentDogDamage >= ThisCard.HealthToken.Amount) return false;
            return true;
        }

        IEnumerator DogDamageAnimation() => new AnimationCardAction(ThisCard, audioClip: ThisCard.Effect1).RunNow();

        IEnumerator DoDogDamage() => new DamageEnemyAction(currentEnemyAttackAction.Enemy, 1).RunNow();
    }
}