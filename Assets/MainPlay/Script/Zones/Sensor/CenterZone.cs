using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using ArkhamShared;

namespace ArkhamGamePlay
{
    public class CenterZone : ZoneBehaviour
    {
        public override float yOffSet => 0.03f;
        [SerializeField] Transform[] invisibleCards;

        public override void PointEnterCardAnimation(CardComponent card)
        {
            card.CardTools.PlaySoundEnterCard();
            DOTween.Kill(card.UniqueId + "ExitCard");
            this.StackOrder(card.transform);
            card.transform.DOScale(1.1f, GameData.INTERACTIVE_TIME_DEFAULT).SetId(card.UniqueId + "EnterCard").SetEase(enterCardEase);
        }

        public override void PointExitCardAnimation(CardComponent card)
        {
            card.CardTools.PlaySoundExitCard();
            DOTween.Kill(card.UniqueId + "EnterCard");
            if (cardSelected != null && cardSelected.CardSensor.CurrentBehaviourZone == this) this.StackOrder(cardSelected.transform);
            card.transform.DOScale(1f, GameData.INTERACTIVE_TIME_DEFAULT).SetId(card.UniqueId + "ExitCard");
        }

        public override void PointEnterPreview(CardComponent card) { }

        public override void PointExitPreview(CardComponent card) { }

        public override void ClickCard(CardComponent card)
        {

            if (cardSelected != null && cardSelected.CardSensor.CurrentBehaviourZone == this)
                cardSelected.transform.DOScale(1f, GameData.INTERACTIVE_TIME_DEFAULT);
            if (card == cardSelected)
            {
                card.CardTools.PlaySoundDeselectCard();
                cardSelected = null;
            }
            else
            {
                card.CardTools.PlaySoundSelectCard();
                card.transform.DOScale(1.1f, GameData.INTERACTIVE_TIME_DEFAULT);
                cardSelected = card;
            }
        }

        public override void PreCardMove()
        {
            StayCard.localPosition = new Vector3(StayCard.localPosition.x, transform.localPosition.y + yOffSet * transform.childCount, StayCard.localPosition.z);
            StartCoroutine(this.HorizontalOrder(invisibleCards));
        }

        public override void PostCardMove()
        {
            if (transform.childCount > 0)
                this.StackOrder(transform.GetChild(transform.childCount - 1));
        }
    }
}