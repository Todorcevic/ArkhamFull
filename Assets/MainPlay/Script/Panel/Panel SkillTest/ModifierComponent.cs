using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using ArkhamShared;

namespace ArkhamGamePlay
{
    public class ModifierComponent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler//, IPointerClickHandler
    {
        [SerializeField] CardComponent card;
        [SerializeField] Image imageCard;

        public CardComponent Card { get => card; }

        /*****************************************************************************************/

        public void Active(CardComponent card)
        {
            this.card = card;
            if (card) imageCard.sprite = AllComponents.CardBuilder.GetSprite(card.ID);
            gameObject.SetActive(card);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!Card.CardSensor.IsInteractable) return;
            transform.DOScale(1.2f, GameData.INTERACTIVE_TIME_DEFAULT);
            Card.CardSensor.CurrentBehaviourZone.PointEnterCardAnimation(Card);
            Card.CardSensor.CurrentBehaviourZone.PointEnterPreview(Card);

        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!Card.CardSensor.IsInteractable) return;
            transform.DOScale(1f, GameData.INTERACTIVE_TIME_DEFAULT);
            Card.CardSensor.CurrentBehaviourZone.PointExitCardAnimation(Card);
            Card.CardSensor.CurrentBehaviourZone.PointExitPreview(Card);
        }
    }
}