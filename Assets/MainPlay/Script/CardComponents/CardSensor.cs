using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using System.Linq;
using System;
using UnityEngine.EventSystems;

namespace ArkhamGamePlay
{
    public class CardSensor : MonoBehaviour
    {
        public static CardSensor onMouseOverThisCard;
        readonly DoubleClick doubleClick = new DoubleClick();
        ZoneBehaviour currentBehaviourZone;
        [SerializeField] CardComponent cardComponent;
        [SerializeField] StackerZone stackerZone;
        [SerializeField] BoxCollider boxCollider;

        public bool IsInteractable
        {
            get
            {
                if (GameControl.FlowIsRunning) return false;
                if (AllComponents.Table.CenterPreview.ListCards.Count > 0
                    && !CardComponent.IsInCenterPreview) return false;
                if (DOTween.IsTweening("MoveTo") || DOTween.IsTweening("Preview") || DOTween.IsTweening("IsBack") || DOTween.IsTweening("MoveFast")) return false;
                if (!currentBehaviourZone) return false;
                return true;
            }
        }

        public ZoneBehaviour CurrentBehaviourZone
        {
            get => currentBehaviourZone;
            set
            {
                if (currentBehaviourZone == value)
                    return;

                currentBehaviourZone = value;
                StackerZone.ParentZone = CurrentBehaviourZone;
                StackerZone.CardPreview = currentBehaviourZone.CardPreview;
                StackerZone.ParentCard = cardComponent;
            }
        }

        public CardComponent CardComponent { get => cardComponent; set => cardComponent = value; }
        public StackerZone StackerZone { get => stackerZone; set => stackerZone = value; }

        /************************************************************************************************/


        void Update()
        {
            if (Input.GetKeyDown("a") && CardComponent.CardTools.TokensBox.activeSelf)
                CardComponent.CardTools.TokensBox.SetActive(false);
            if (Input.GetKeyUp("a") && !CardComponent.CardTools.TokensBox.activeSelf)
                CardComponent.CardTools.TokensBox.SetActive(true);
            if (Input.GetKeyDown("q") && StackerZone.gameObject.activeSelf && !CardComponent.IsInCenterPreview)
                StackerZone.gameObject.SetActive(false);
            if (Input.GetKeyUp("q") && !StackerZone.gameObject.activeSelf && !CardComponent.IsInCenterPreview)
                StackerZone.gameObject.SetActive(true);
        }

        public void OnMouseEnter()
        {
            onMouseOverThisCard = this;
            if (IsInteractable && !EventSystem.current.IsPointerOverGameObject())
            {
                currentBehaviourZone.PointEnterCardAnimation(cardComponent);
                cardComponent.CardTools.PrintCardActions(GameControl.CurrentInteractableAction is InteractableCenterShow iCenterShow ? iCenterShow.ChosenCardEffects : GameControl.CurrentInteractableAction?.CardEffects);
                currentBehaviourZone.PointEnterPreview(cardComponent);
            }
        }

        public void OnMouseExit()
        {
            onMouseOverThisCard = null;
            if (IsInteractable)
            {
                if (!CardComponent.IsInCenterPreview)
                    CardComponent.CardTools.ShowInfoBox(string.Empty);
                currentBehaviourZone.PointExitCardAnimation(CardComponent);
                currentBehaviourZone.PointExitPreview(CardComponent);
            }
        }

        public void OnMouseUpAsButton()
        {
            if (IsInteractable && !EventSystem.current.IsPointerOverGameObject())
            {
                if (cardComponent.CardType == CardType.PlayCard)
                    AllComponents.InvestigatorManagerComponent.SelectInvestigatorCoroutine(cardComponent.Owner);
                if (CardComponent.CanBePlayedNow)
                {

                    if (doubleClick.IsDetected(Time.time))
                    {
                        cardComponent.CardTools.PlaySoundCard();
                        OnMouseExit();
                        GameControl.CurrentInteractableAction.CardPlay(CardComponent);
                    }
                    else
                    {
                        currentBehaviourZone.ClickCard(cardComponent);
                        GameControl.CurrentInteractableAction.CardSelected(CardComponent);
                    }
                }
            }
        }
    }
}