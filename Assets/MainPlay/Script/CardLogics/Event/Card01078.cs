using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01078 : CardEvent
    {
        protected override void BeginGameAction(GameAction gameAction)
        {
            if (gameAction is InvestigatorTurn investigatorturn && PlayingCard(investigatorturn))
            {
                playFromHand = PlayFromHand;
                playFromHand.Type = EffectType.Play | EffectType.Evade;
                investigatorturn.CardEffects.Add(playFromHand);
            }
        }

        protected override bool PlayingCard(InvestigatorTurn investigatorturn)
        {
            if (!CheckPlayedFromHand()) return false;
            if (GameControl.ActiveInvestigator != ThisCard.VisualOwner) return false;
            if (ThisCard.VisualOwner.AllEnemiesInMyLocation.FindAll(c => ((CardEnemy)c.CardLogic).CanBeEvaded).Count < 1) return false;
            return true;
        }

        protected override IEnumerator LogicEffect()
        {
            foreach (CardComponent enemy in ThisCard.VisualOwner.AllEnemiesInMyLocation.FindAll(c => ((CardEnemy)c.CardLogic).CanBeEvaded))
                yield return new EvadeEnemyAction(enemy).RunNow();
        }
    }
}