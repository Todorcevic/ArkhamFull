using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class Card01038 : CardEvent
    {
        protected override Zone ZoneToPlayFromHand => ThisCard.VisualOwner.CurrentLocation.MyOwnZone;

        /*****************************************************************************************/
        protected override void BeginGameAction(GameAction gameAction)
        {
            base.BeginGameAction(gameAction);
            if (gameAction is MoveCardAction moveCardAction && CheckingToDiscard(moveCardAction))
                new DiscardAction(ThisCard).AddActionTo();
            if (gameAction is MoveHunterEnemy moveHunter && EnemyTryEnter(moveHunter))
                new EffectAction(() => EnemyCantEnter(moveHunter)).AddActionTo();
        }

        protected override void EndGameAction(GameAction gameAction)
        {
            if (gameAction is MoveCardAction moveCardAction && EnemyEnganged(moveCardAction))
                new EffectAction(() => Desenganged(moveCardAction)).AddActionTo();
        }

        bool CheckingToDiscard(MoveCardAction moveCardAction)
        {
            if (!ThisCard.IsInPlay) return false;
            if (moveCardAction.ThisCard.CardType != CardType.PlayCard) return false;
            if (moveCardAction.ThisCard.CurrentZone != ThisCard.CurrentZone) return false;
            return true;
        }

        bool EnemyTryEnter(MoveHunterEnemy moveHunter)
        {
            if (!ThisCard.IsInPlay) return false;
            if (moveHunter.Enemy.KeyWords.Contains("Elite")) return false;
            return true;
        }

        bool EnemyEnganged(MoveCardAction moveCardAction)
        {
            if (!ThisCard.IsInPlay) return false;
            if (moveCardAction.ThisCard.CardType != CardType.PlayCard) return false;
            if (moveCardAction.Zone != ThisCard.CurrentZone) return false;
            if (!moveCardAction.ThisCard.Owner.IsEnganged) return false;
            return true;
        }

        IEnumerator EnemyCantEnter(MoveHunterEnemy moveHunter)
        {
            moveHunter.AllLocationsAllowed.Remove(ThisCard.CurrentZone.ThisCard);
            yield return null;
        }

        IEnumerator Desenganged(MoveCardAction moveCardAction)
        {
            foreach (CardComponent enemy in moveCardAction.ThisCard.Owner.Threat.ListCards.FindAll(c => c.CardType == CardType.Enemy))
                yield return new MoveCardAction(enemy, moveCardAction.OldZone, isFast: true).RunNow();
            yield return new WaitWhile(() => DOTween.IsTweening("MoveCard"));
        }
    }
}