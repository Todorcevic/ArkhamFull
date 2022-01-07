using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class EvadeEnemyAction : GameAction
    {
        public override GameActionType GameActionType => GameActionType.Compound;
        public CardComponent Enemy { get; }

        /*****************************************************************************************/
        public EvadeEnemyAction(CardComponent enemy) => Enemy = enemy;

        /*****************************************************************************************/
        protected override IEnumerator ActionLogic()
        {
            if (Enemy.IsOutGame || !Enemy.IsInPlay) yield break;
            yield return new ExaustCardAction(Enemy).RunNow();
            yield return new MoveCardAction(Enemy, ((CardEnemy)Enemy.CardLogic).CurrentLocation.MyOwnZone, withPreview: false).RunNow();
        }
    }
}