using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using ArkhamShared;

namespace ArkhamGamePlay
{
    public class DeckZone : ZoneBehaviour
    {
        CardComponent topCard;
        bool IsFakeDeck => !topCard.IsBack;
        public override void PointEnterCardAnimation(CardComponent card)
        {
            if (card == topCard)
            {
                card.CardTools.PlaySoundEnterCard();
                DOTween.Kill(topCard.UniqueId + "ExitCard");
                DOTween.Sequence().Append(topCard.transform.DOMoveZ(ShowCard.position.z, GameData.INTERACTIVE_TIME_DEFAULT))
                    .Join(topCard.transform.DOMoveY(ShowCard.position.y + GameData.CARD_THICK * transform.childCount, GameData.INTERACTIVE_TIME_DEFAULT))
                    .Join(topCard.transform.DORotate(ShowCard.rotation.eulerAngles, GameData.INTERACTIVE_TIME_DEFAULT))
                    .SetId(topCard.UniqueId + "EnterCard").SetEase(enterCardEase);
            }
        }

        public override void PointExitCardAnimation(CardComponent card)
        {
            if (card == topCard)
                base.PointExitCardAnimation(topCard);
        }

        public override void PointEnterPreview(CardComponent card)
        {
            if (IsFakeDeck) base.PointEnterPreview(topCard);
        }

        public override void PointExitPreview(CardComponent card)
        {
            if (IsFakeDeck) base.PointExitPreview(topCard);
        }

        public override void PreCardMove()
        {
            StayCard.position = new Vector3(StayCard.position.x, GameData.CARD_THICK * transform.childCount, StayCard.position.z);
            if (transform.childCount > 0) topCard = transform.GetChild(transform.childCount - 1).GetComponent<CardComponent>();
        }

        public override void PostCardMove()
        {
            int i = 0;
            foreach (Transform card in transform)
                card.localPosition = new Vector3(card.localPosition.x, GameData.CARD_THICK * i++, card.localPosition.z);
        }
    }
}