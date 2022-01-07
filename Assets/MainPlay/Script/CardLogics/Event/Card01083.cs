using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01083 : CardEvent
    {
        CardComponent enemy;
        protected override bool IsFast => true;
        /*****************************************************************************************/
        protected override void BeginGameAction(GameAction gameAction) { }

        protected override void EndGameAction(GameAction gameAction)
        {
            base.EndGameAction(gameAction);
            if (gameAction is EvadeEnemyAction evadeEnemyAction && CheckEnemy(evadeEnemyAction))
            {
                playFromHand = PlayFromHand;
                playFromHand.Name = "Mandar a " + evadeEnemyAction.Enemy.Info.Name + " al mazo de encuentros";
                evadeEnemyAction.ChooseCardOptionalAction.ChosenCardEffects.Add(playFromHand);
            }
        }

        bool CheckEnemy(EvadeEnemyAction evadeEnemyAction)
        {
            if (!CheckPlayedFromHand()) return false;
            if (((CardEnemy)evadeEnemyAction.Enemy.CardLogic).CurrentLocation != ThisCard.VisualOwner.CurrentLocation) return false;
            if (evadeEnemyAction.Enemy.IsWeakness) return false;
            if (evadeEnemyAction.Enemy.KeyWords.Contains("Elite")) return false;
            enemy = evadeEnemyAction.Enemy;
            return true;
        }

        protected override IEnumerator LogicEffect()
        {
            foreach (TokenComponent token in enemy.AllTokens.FindAll(t => t.Amount > 0))
                yield return new AddTokenAction(token, -token.Amount).RunNow();
            yield return new MoveCardAction(enemy, AllComponents.Table.EncounterDeck, withPreview: true, isBack: true).RunNow();
            yield return new ShuffleAction(AllComponents.Table.EncounterDeck).RunNow();
        }

        protected override bool CheckFilterToCancel() => ((CardEnemy)enemy.CardLogic).CurrentLocation != ThisCard.VisualOwner.CurrentLocation;
    }
}