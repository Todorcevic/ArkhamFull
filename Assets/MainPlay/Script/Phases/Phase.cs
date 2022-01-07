using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;
using ArkhamShared;

namespace ArkhamGamePlay
{
    public class Phase : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        readonly float delayedIn = 0.18f;
        readonly float delayedOut = 0f;
        readonly float textDisplacement = 200;
        Transform currentSite;
        [SerializeField] TextMeshProUGUI info;
        [SerializeField] Transform bigSite;
        [SerializeField] AudioSource audioSource;
        [SerializeField] AudioClip EnterHover;
        [SerializeField] AudioClip ExitHover;
        [SerializeField] AudioClip Drop;
        [SerializeField] AudioClip Return;

        /*****************************************************************************************/
        public void Moving(Transform site)
        {
            currentSite = site;
            transform.DOMove(site.position, GameData.INTERACTIVE_TIME_DEFAULT);
            transform.DOScale(site.localScale, GameData.INTERACTIVE_TIME_DEFAULT);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (currentSite == bigSite && !DOTween.IsTweening("ChangingPhase"))
            {
                audioSource.PlayOneShot(EnterHover);
                audioSource.clip = Drop;
                audioSource.PlayDelayed(delayedIn);
                DOTween.Kill("HideInfo");
                transform.DOScale(currentSite.localScale * 1.1f, GameData.INTERACTIVE_TIME_DEFAULT);
                info.transform.DOLocalMoveY(0, GameData.ANIMATION_TIME_DEFAULT * 2).SetEase(Ease.OutBounce).SetId("ShowInfo");
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (currentSite == bigSite && !DOTween.IsTweening("ChangingPhase"))
            {
                audioSource.PlayOneShot(ExitHover);
                audioSource.clip = Return;
                audioSource.PlayDelayed(delayedOut);
                DOTween.Kill("ShowInfo");
                transform.DOScale(currentSite.localScale, GameData.INTERACTIVE_TIME_DEFAULT);
                info.transform.DOLocalMoveY(textDisplacement, GameData.ANIMATION_TIME_DEFAULT).SetEase(Ease.OutCubic).SetId("HideInfo");
            }
        }
    }
}