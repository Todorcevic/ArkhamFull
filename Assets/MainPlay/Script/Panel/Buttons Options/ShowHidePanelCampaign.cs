using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using ArkhamShared;

namespace ArkhamGamePlay
{
    public class ShowHidePanelCampaign : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] Image image;
        [SerializeField] Button button;
        [SerializeField] AudioSource audioSource;
        [SerializeField] AudioClip click;

        /*****************************************************************************************/
        public void OnPointerClick(PointerEventData eventData)
        {
            if (GameControl.CurrentPanelActive == null && GameControl.GameIsStarted)
            {
                AllComponents.PanelCampaign.SetPanelCampaign();
                AllComponents.PanelCampaign.SetButton();
                StartCoroutine(AllComponents.PanelCampaign.SelectThisPanel());
                ClickingUI();
            }
            if (GameControl.CurrentPanelActive == AllComponents.PanelCampaign && GameControl.GameIsStarted)
            {
                AllComponents.PanelCampaign.ReadyClicked(null);
                ClickingUI();
            }
        }

        void ClickingUI()
        {
            audioSource.PlayOneShot(click);
            DOTween.Sequence().Append(transform.DOScale(1.3f, GameData.INTERACTIVE_TIME_DEFAULT).SetEase(Ease.OutCubic))
                    .Append(transform.DOScale(1f, GameData.INTERACTIVE_TIME_DEFAULT));
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if ((GameControl.CurrentPanelActive == null || GameControl.CurrentPanelActive == AllComponents.PanelCampaign) && GameControl.GameIsStarted)
                transform.DOScale(1.2f, GameData.INTERACTIVE_TIME_DEFAULT);
            else button.interactable = false;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            transform.DOScale(1f, GameData.INTERACTIVE_TIME_DEFAULT);
            button.interactable = true;
        }
    }
}