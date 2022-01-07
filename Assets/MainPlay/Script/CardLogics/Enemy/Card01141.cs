using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01141 : CardEnemy
    {
        CardComponent Hospital => GameControl.GetCard("01128");
        public override List<CardComponent> SpawnLocation => new List<CardComponent>() { Hospital };

        /*****************************************************************************************/

        protected override void EndGameAction(GameAction gameAction)
        {
            base.EndGameAction(gameAction);
            if (gameAction is EvadeEnemyAction evaneEnemyAction && CheckThisCard(evaneEnemyAction))
                new EffectAction(AddingVictory, AddingVictoryAnimation).AddActionTo();
        }

        bool CheckThisCard(EvadeEnemyAction evadeEnemyAction)
        {
            if (evadeEnemyAction.Enemy != ThisCard) return false;
            return true;
        }

        IEnumerator AddingVictoryAnimation() => new AnimationCardAction(ThisCard, audioClip: ThisCard.Effect1, withReturn: false).RunNow();

        IEnumerator AddingVictory()
        {
            yield return new DiscardAction(ThisCard, victoryZone: true, withTopUp: false).RunNow();
            yield return new AddVictoryCardAction(
                ThisCard,
                ThisCard.Info.Name + " otorga " + ThisCard.VictoryPoint + " puntos de experiencia a cada investigador.",
                ThisCard.VictoryPoint).RunNow();
        }
    }
}
