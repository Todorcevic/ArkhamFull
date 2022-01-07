using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using ArkhamShared;

namespace ArkhamGamePlay
{
    public class TokenStackZone : ZoneBehaviour
    {
        [SerializeField] AudioClip enterHover;
        [SerializeField] AudioClip exitHover;
        [SerializeField] Ease easeDrop;

        public override void PointEnterCardAnimation(CardComponent card)
        {
            card.CardTools.PlayOneShotSound(enterHover);
            DOTween.Sequence().Append(card.transform.DOMove(ShowCard.transform.position, GameData.INTERACTIVE_TIME_DEFAULT))
                .Join(card.transform.DORotate(ShowCard.transform.rotation.eulerAngles, GameData.INTERACTIVE_TIME_DEFAULT))
                .SetEase(enterCardEase);
        }

        public override void PointExitCardAnimation(CardComponent card)
        {
            card.CardTools.PlayOneShotSound(exitHover);
            DOTween.Sequence().Append(card.transform.DOMove(StayCard.transform.position, GameData.INTERACTIVE_TIME_DEFAULT))
                .Join(card.transform.DORotate(StayCard.transform.rotation.eulerAngles, GameData.INTERACTIVE_TIME_DEFAULT))
                .SetEase(easeDrop);
        }

        public override void PointEnterPreview(CardComponent card) { }
        public override void PointExitPreview(CardComponent card) { }
    }
}