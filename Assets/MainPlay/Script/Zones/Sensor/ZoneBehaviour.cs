using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using ArkhamShared;

namespace ArkhamGamePlay
{
    public class ZoneBehaviour : MonoBehaviour
    {
        protected static CardComponent cardSelected;
        [SerializeField] Transform stayCard;
        [SerializeField] Transform showCard;
        [SerializeField] CardPreview cardPreview;
        public virtual float yOffSet => 0.003f;
        protected float originalScale = 1.2f;
        protected Ease enterCardEase = Ease.OutCubic;
        public Transform StayCard { get => stayCard; set => stayCard = value; }
        public Transform ShowCard { get => showCard; set => showCard = value; }
        public CardPreview CardPreview { get => cardPreview; set => cardPreview = value; }
        public AudioSource AudioSource { get; private set; }

        /*****************************************************************************************/
        void Start() => AudioSource = gameObject.AddComponent<AudioSource>();

        public virtual void PointEnterCardAnimation(CardComponent card)
        {
            card.CardTools.PlaySoundEnterCard(); ;
            DOTween.Kill(card.UniqueId + "ExitCard");
            DOTween.Sequence().Append(card.transform.DOLocalMoveY(ShowCard.localPosition.y, GameData.INTERACTIVE_TIME_DEFAULT))
                 .Join(card.transform.DOLocalMoveZ(ShowCard.localPosition.z, GameData.INTERACTIVE_TIME_DEFAULT))
                 .Join(card.transform.DOLocalRotate(ShowCard.localRotation.eulerAngles, GameData.INTERACTIVE_TIME_DEFAULT))
                 .SetId(card.UniqueId + "EnterCard").SetEase(enterCardEase);
        }

        public virtual void PointExitCardAnimation(CardComponent card)
        {
            card.CardTools.PlaySoundExitCard();
            DOTween.Kill(card.UniqueId + "EnterCard");
            DOTween.Sequence().Append(card.transform.DOMove(StayCard.position, GameData.INTERACTIVE_TIME_DEFAULT))
                .Join(card.transform.DORotate(StayCard.rotation.eulerAngles, GameData.INTERACTIVE_TIME_DEFAULT))
                .SetId(card.UniqueId + "ExitCard");
        }

        public virtual void PointEnterPreview(CardComponent card)
        {
            cardPreview.Active(card);
            cardPreview.transform.DOScale(originalScale, GameData.INTERACTIVE_TIME_DEFAULT)
                .SetEase(Ease.InOutExpo).SetId(card.UniqueId + "EnterPreview");
        }

        public virtual void PointExitPreview(CardComponent card)
        {
            cardPreview.transform.localScale = Vector3.zero;
            cardPreview.Desactive();
            DOTween.Kill(card.UniqueId + "EnterPreview");
        }

        public virtual void PreCardMove() { }
        public virtual void PostCardMove() { }
        public virtual void ClickCard(CardComponent card)
        {
            if (card == cardSelected)
            {
                card.CardTools.PlaySoundDeselectCard();
                cardSelected = null;
            }
            else
            {
                card.CardTools.PlaySoundSelectCard();
                cardSelected = card;
            }
        }
    }
}