using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using ArkhamShared;

namespace ArkhamGamePlay
{
    public class ShowHideChooseCard : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] Sprite show;
        [SerializeField] Sprite hide;
        [SerializeField] Image image;
        [SerializeField] Button button;
        [SerializeField] AudioSource audioSource;
        [SerializeField] AudioClip click;
        InteractableCenterShow centerShowableAction;

        public InteractableCenterShow ICenterShowableAction
        {
            get => centerShowableAction;
            set
            {
                button.interactable = value != null;
                centerShowableAction = value;
            }
        }

        /*****************************************************************************************/
        public void OnPointerClick(PointerEventData eventData)
        {
            if (centerShowableAction != null && !DOTween.IsTweening("MoveFast"))
            {
                if (centerShowableAction.CardsInPreview) StartCoroutine(centerShowableAction.ShowTable());
                else StartCoroutine(centerShowableAction.ShowPreviewCards());
                audioSource.PlayOneShot(click);
                DOTween.Sequence().Append(transform.DOScale(1.3f, GameData.INTERACTIVE_TIME_DEFAULT).SetEase(Ease.OutCubic))
                    .Append(transform.DOScale(1f, GameData.INTERACTIVE_TIME_DEFAULT));
            }
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (centerShowableAction != null) transform.DOScale(1.2f, GameData.INTERACTIVE_TIME_DEFAULT);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (centerShowableAction != null) transform.DOScale(1f, GameData.INTERACTIVE_TIME_DEFAULT);
        }

        public void SetSprite(bool arrowToTable)
        {
            image.sprite = arrowToTable ? show : hide;
        }
    }
}