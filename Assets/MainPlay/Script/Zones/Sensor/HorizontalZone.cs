using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using ArkhamShared;

namespace ArkhamGamePlay
{
    public class HorizontalZone : ZoneBehaviour
    {
        [SerializeField] Transform[] invisibleCards;

        public override void PointExitCardAnimation(CardComponent card)
        {
            card.CardTools.PlaySoundExitCard(); ;
            DOTween.Kill(card.UniqueId + "EnterCard");
            DOTween.Sequence().Append(card.transform.DOMoveY(transform.position.y + (card.transform.GetSiblingIndex() * yOffSet), GameData.INTERACTIVE_TIME_DEFAULT))
                .Join(card.transform.DOMoveZ(transform.position.z, GameData.INTERACTIVE_TIME_DEFAULT))
                .Join(card.transform.DOLocalRotate(StayCard.localRotation.eulerAngles, GameData.INTERACTIVE_TIME_DEFAULT))
                .SetId(card.UniqueId + "ExitCard").SetEase(enterCardEase);
        }

        public override void PointEnterPreview(CardComponent card)
        {
            float xOffSet = card.transform.localPosition.x < 0 ? 0.6f : -0.6f;
            float xPosition = (card.transform.localPosition.x / 3f) + xOffSet;
            CardPreview.transform.localPosition = new Vector3(xPosition, CardPreview.transform.localPosition.y, CardPreview.transform.localPosition.z);
            base.PointEnterPreview(card);
        }

        public override void PreCardMove()
        {
            StayCard.localPosition = new Vector3(StayCard.localPosition.x, transform.localPosition.y + yOffSet * transform.childCount, StayCard.localPosition.z);
            StartCoroutine(this.HorizontalOrder(invisibleCards));
        }
    }
}