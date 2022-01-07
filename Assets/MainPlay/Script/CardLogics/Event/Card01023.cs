using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01023 : CardEvent
    {
        EnemyAttackAction enemyAttackAction;
        protected override string nameCardEffect => "Esquivar este ataque";
        protected override bool IsFast => true;

        /*****************************************************************************************/
        protected override void BeginGameAction(GameAction gameAction)
        {
            if (gameAction is EnemyAttackAction enemyAttackAction && PlayCard(enemyAttackAction))
            {
                this.enemyAttackAction = enemyAttackAction;
                enemyAttackAction.ChooseCardOptionalAction.ChosenCardEffects.Add(playFromHand = PlayFromHand);
            }
        }

        bool PlayCard(EnemyAttackAction enemyAttackAction)
        {
            if (!CheckPlayedFromHand()) return false;
            if (enemyAttackAction.Investigator.CurrentLocation != ThisCard.VisualOwner.CurrentLocation) return false;
            return true;
        }

        protected override IEnumerator LogicEffect()
        {
            enemyAttackAction.IsActionCanceled = true;
            yield return null;
        }

        protected override bool CheckFilterToCancel() => enemyAttackAction?.IsActionCanceled ?? false;
    }
}