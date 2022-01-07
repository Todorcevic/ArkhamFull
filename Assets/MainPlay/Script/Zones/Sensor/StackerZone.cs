using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using ArkhamShared;

namespace ArkhamGamePlay
{
    public class StackerZone : ZoneBehaviour
    {
        [SerializeField] Transform[] invisibleCards;
        public ZoneBehaviour ParentZone { get; set; }
        public CardComponent ParentCard { get; set; }

        public override void PointEnterCardAnimation(CardComponent card)
        {
            card.transform.DOScale(1.05f, GameData.INTERACTIVE_TIME_DEFAULT).SetEase(enterCardEase);
            ParentZone.PointEnterCardAnimation(ParentCard);
            this.StackOrder(card.transform);
        }

        public override void PointExitCardAnimation(CardComponent card)
        {
            card.transform.DOScale(1f, GameData.INTERACTIVE_TIME_DEFAULT);
            ParentZone.PointExitCardAnimation(ParentCard);
        }

        public override void PreCardMove()
        {
            StayCard.localPosition = new Vector3(StayCard.localPosition.x, yOffSet * transform.childCount, StayCard.localPosition.z);
            StartCoroutine(this.HorizontalOrder(invisibleCards));
        }

        public override void PostCardMove()
        {
            if (transform.childCount > 0) this.StackOrder(transform.GetChild(transform.childCount - 1));
        }
    }
}