using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01137 : CardEnemy, ISpecialAttack
    {
        CardComponent DownTown1 => GameControl.GetCard("01130");
        CardComponent DownTown2 => GameControl.GetCard("01131");
        public override List<CardComponent> SpawnLocation => new List<CardComponent>() { DownTown1.IsInPlay ? DownTown1 : DownTown2 };

        /*****************************************************************************************/
        IEnumerator ISpecialAttack.Attack()
        {
            if (ThisCard.HealthToken.Amount > 0)
                yield return new AddTokenAction(ThisCard.HealthToken, -1).RunNow();
        }
    }
}
