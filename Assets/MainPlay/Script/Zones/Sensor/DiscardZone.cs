using DG.Tweening;
using UnityEngine;
using System.Collections;
using ArkhamShared;

namespace ArkhamGamePlay
{
    public class DiscardZone : ZoneBehaviour
    {
        [SerializeField] float offSetExpandX;
        float offSetExpandZ = 0.1f;

        public override void PointEnterCardAnimation(CardComponent card)
        {
            card.CardTools.PlaySoundEnterCard();
            DOTween.Kill(transform.name + "Contrain");
            card.transform.DOLocalMoveX(offSetExpandX, GameData.INTERACTIVE_TIME_DEFAULT).SetEase(enterCardEase);
            DOTween.Sequence().Append(transform.DOLocalMoveY(ShowCard.localPosition.y, GameData.INTERACTIVE_TIME_DEFAULT))
                .Join(transform.DOLocalMoveZ(ShowCard.localPosition.z, GameData.INTERACTIVE_TIME_DEFAULT))
                .Join(transform.DOLocalRotate(ShowCard.localRotation.eulerAngles, GameData.INTERACTIVE_TIME_DEFAULT))
                .SetId(transform.name + "Expand").SetEase(enterCardEase).OnComplete(Expand);
            void Expand()
            {
                int n = 0;
                for (int i = transform.childCount - 1; i >= 0; i--)
                    transform.GetChild(i).DOLocalMoveZ(offSetExpandZ * n++, GameData.INTERACTIVE_TIME_DEFAULT);
            }
        }

        public override void PointExitCardAnimation(CardComponent card)
        {
            card.CardTools.PlaySoundExitCard();
            DOTween.Kill(transform.name + "Expand");
            card.transform.DOLocalMoveX(0f, GameData.INTERACTIVE_TIME_DEFAULT);


            for (int i = transform.childCount - 1; i >= 0; i--)
                transform.GetChild(i).DOLocalMoveZ(0f, GameData.INTERACTIVE_TIME_DEFAULT).SetId(transform.name + "Contrain");
            DOTween.Sequence().Append(transform.DOLocalMoveY(0, GameData.INTERACTIVE_TIME_DEFAULT))
                .Join(transform.DOLocalMoveZ(StayCard.localPosition.z, GameData.INTERACTIVE_TIME_DEFAULT))
                .Join(transform.DOLocalRotate(StayCard.localRotation.eulerAngles, GameData.INTERACTIVE_TIME_DEFAULT))
                .SetId(transform.name + "Contrain");
        }

        public override void PreCardMove()
        {
            StayCard.position = new Vector3(StayCard.position.x, GameData.CARD_THICK * (transform.childCount + 1), StayCard.position.z);
        }

        public override void PostCardMove()
        {
            int i = 0;
            foreach (Transform card in transform)
                card.localPosition = new Vector3(card.localPosition.x, GameData.CARD_THICK * i++, card.localPosition.z);
        }
    }
}