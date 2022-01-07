using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using System.Linq;
using ArkhamShared;

namespace ArkhamGamePlay
{
    public class MoveDeck : GameAction
    {
        readonly bool withMoveUp;
        readonly bool isBack;
        readonly AudioClip moveDeckClip;
        public override GameActionType GameActionType => GameActionType.Compound;
        public Zone MoveZone { get; }
        public List<CardComponent> ListCards { get; }


        /*****************************************************************************************/
        public MoveDeck(List<CardComponent> listCards, Zone zone, bool isBack = false, bool withMoveUp = false)
        {
            ListCards = listCards;
            MoveZone = zone;
            this.isBack = isBack;
            this.withMoveUp = withMoveUp;
            moveDeckClip = AllComponents.AudioComponent.LoadClips("MoveDeck").clip1;
        }

        /*****************************************************************************************/
        protected override IEnumerator ActionLogic()
        {
            if (!withMoveUp)
            {
                MoveZone.ZoneBehaviour.AudioSource.clip = moveDeckClip;
                MoveZone.ZoneBehaviour.AudioSource.loop = true;
                MoveZone.ZoneBehaviour.AudioSource.PlayDelayed(0.7f);
            }
            foreach (CardComponent card in ListCards.ToArray())
            {
                Sequence movingDeck = DOTween.Sequence()
                .Append(card.TurnDown(isBack, withAudio: withMoveUp))
                .Join(card.MoveFast(MoveZone, withAudio: null))
                .SetId("MovingDeck")
                .SetEase(Ease.Linear);
                if (withMoveUp) movingDeck.Prepend(card.transform.DOLocalMoveY(card.transform.localPosition.y + 0.85f, GameData.ANIMATION_TIME_DEFAULT));
                yield return movingDeck.WaitForPosition(0.02f);
            }
            yield return new WaitWhile(() => DOTween.IsTweening("MovingDeck"));
            MoveZone.ZoneBehaviour.AudioSource.Stop();
            MoveZone.ZoneBehaviour.AudioSource.loop = false;
        }
    }
}