using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using ArkhamShared;

namespace ArkhamGamePlay
{
    public class SkillTestImageSensor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public CardComponent Card { get; set; }

        /*****************************************************************************************/
        public void OnPointerEnter(PointerEventData eventData)
        {
            transform.DOScale(1.2f, GameData.INTERACTIVE_TIME_DEFAULT);
            Card.CardSensor.CurrentBehaviourZone.PointEnterCardAnimation(Card);
            Card.CardTools.PrintCardActions(GameControl.CurrentInteractableAction?.CardEffects);
            Card.CardSensor.CurrentBehaviourZone.PointEnterPreview(Card);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            transform.DOScale(1f, GameData.INTERACTIVE_TIME_DEFAULT);
            Card.CardSensor.CurrentBehaviourZone.PointExitCardAnimation(Card);
            Card.CardSensor.CurrentBehaviourZone.PointExitPreview(Card);
        }
    }
}