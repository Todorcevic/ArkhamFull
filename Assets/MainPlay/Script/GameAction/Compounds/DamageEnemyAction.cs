using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class DamageEnemyAction : GameAction
    {
        public CardComponent Enemy { get; set; }
        public int Amount { get; set; }

        /*****************************************************************************************/
        public DamageEnemyAction(CardComponent enemy, int amount)
        {
            Enemy = enemy;
            Amount = amount;
        }

        /*****************************************************************************************/
        protected override IEnumerator ActionLogic()
        {
            if (!Enemy.IsInPlay) yield break;
            yield return new AddTokenAction(Enemy.HealthToken, Amount).RunNow();
        }
    }
}