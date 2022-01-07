using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using ArkhamShared;

namespace ArkhamGamePlay
{
    public class BuffSprite : MonoBehaviour
    {
        public string id;
        public SpriteRenderer spriteBuff;
        public CardComponent parentCard;
        public CardComponent selfCard;

        void OnMouseEnter()
        {
            if (!parentCard.CardSensor.IsInteractable) return;
            transform.DOScale(1.2f, GameData.INTERACTIVE_TIME_DEFAULT);
            parentCard.CardSensor.CurrentBehaviourZone.PointEnterPreview(selfCard);
            parentCard.CardSensor.CurrentBehaviourZone.PointEnterCardAnimation(parentCard);
        }

        void OnMouseExit()
        {
            if (!parentCard.CardSensor.IsInteractable) return;
            transform.DOScale(1f, GameData.INTERACTIVE_TIME_DEFAULT);
            parentCard.CardSensor.CurrentBehaviourZone.PointExitPreview(selfCard);
            parentCard.CardSensor.CurrentBehaviourZone.PointExitCardAnimation(parentCard);
        }
    }
}
