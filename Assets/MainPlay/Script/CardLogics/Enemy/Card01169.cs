using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01169 : CardEnemy
    {
        public override List<CardComponent> SpawnLocation =>
            GameControl.AllCardComponents.FindAll(c => c.IsInPlay && c.CardLogic is CardLocation location && location.IsEmpty);

        /*****************************************************************************************/
        protected override void EndGameAction(GameAction gameAction)
        {
            base.EndGameAction(gameAction);
            if (gameAction is SpawnEnemyAction spawnAction && CheckSpawnAcolite(spawnAction))
                new AddTokenAction(ThisCard.DoomToken, 1).AddActionTo();
        }

        bool CheckSpawnAcolite(SpawnEnemyAction spawnAction)
        {
            if (spawnAction.Enemy != ThisCard) return false;
            if (!ThisCard.IsInPlay) return false;
            return true;
        }
    }
}
