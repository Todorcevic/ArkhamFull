using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using System.Linq;
using ArkhamShared;

namespace ArkhamGamePlay
{
    public class HandZone : ZoneBehaviour
    {
        [SerializeField] Transform[] invisibleCards;

        public override void PointEnterCardAnimation(CardComponent card)
        {
            card.CardTools.PlaySoundEnterCard();
            DOTween.Kill(card.UniqueId + "ExitCard");
            this.StackOrder(card.transform, 0);
            DOTween.Sequence().Append(card.transform.DOScale(1.5f, GameData.INTERACTIVE_TIME_DEFAULT))
                .Join(card.transform.DOLocalMoveZ(0.66f, GameData.INTERACTIVE_TIME_DEFAULT))
                .SetId(card.UniqueId + "EnterCard").SetEase(enterCardEase);
            int index = card.transform.GetSiblingIndex();
            invisibleCards[index].gameObject.SetActive(false);
            invisibleCards[index].GetComponent<LayoutElement>().preferredWidth = 300 + (transform.childCount * 50);
            StartCoroutine(this.HorizontalOrder(invisibleCards));
        }

        public override void PointExitCardAnimation(CardComponent card)
        {
            card.CardTools.PlaySoundExitCard();
            DOTween.Kill(card.UniqueId + "EnterCard");
            DOTween.Sequence().Append(card.transform.DOScale(1f, GameData.INTERACTIVE_TIME_DEFAULT))
            .Join(card.transform.DOLocalMoveZ(0f, GameData.INTERACTIVE_TIME_DEFAULT))
            .SetId(card.UniqueId + "ExitCard");
            int index = card.transform.GetSiblingIndex();
            invisibleCards[index].gameObject.SetActive(false);
            invisibleCards[index].GetComponent<LayoutElement>().preferredWidth = 300;
            StartCoroutine(this.HorizontalOrder(invisibleCards));
        }

        public override void PointEnterPreview(CardComponent card) { }
        public override void PointExitPreview(CardComponent card) { }

        public override void PreCardMove()
        {
            StayCard.localPosition = new Vector3(StayCard.localPosition.x, transform.localPosition.y + yOffSet * transform.childCount, StayCard.localPosition.z);
            StartCoroutine(this.HorizontalOrder(invisibleCards));
        }

        public override void PostCardMove()
        {
            if (transform.childCount > 0)
                this.StackOrder(transform.GetChild(transform.childCount - 1), timeAnimation: GameData.ANIMATION_TIME_DEFAULT);
        }
    }
}