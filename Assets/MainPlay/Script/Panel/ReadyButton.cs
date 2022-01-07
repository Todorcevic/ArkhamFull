using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using DG.Tweening;
using ArkhamShared;

namespace ArkhamGamePlay
{
    public class ReadyButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        ButtonState state;
        IButtonClickable buttonClickableAction;
        [SerializeField] bool withButtonEffects;
        [SerializeField] Button readyButton;
        [SerializeField] Image glow;
        [SerializeField] AudioSource audioSource;
        [SerializeField] TextMeshProUGUI textButton;
        [SerializeField] AudioClip EnterHover;
        [SerializeField] AudioClip ExitHover;
        [SerializeField] AudioClip Click;
        [SerializeField] AudioClip Click2;
        [SerializeField] AudioClip EnterHover2;
        [SerializeField] AudioClip ExitHover2;
        [SerializeField] AudioClip EnterPanel;
        [SerializeField] AudioClip ExitPanel;

        public ButtonState State
        {
            get => state;
            set
            {
                state = value;
                switch (state)
                {
                    case ButtonState.Ready:
                        readyButton.interactable = true;
                        glow.enabled = true;
                        glow.material.SetColor("_Color", Color.white);
                        ColorBlock colorBlockGreen = readyButton.colors;
                        colorBlockGreen.highlightedColor = new Color(0.55f, 0.75f, 0.55f);
                        readyButton.colors = colorBlockGreen;
                        textButton.color = new Color(0.9f, 0.9f, 0.7f, 1);
                        textButton.text = "Listo";
                        break;
                    case ButtonState.StandBy:
                        readyButton.interactable = true;
                        glow.enabled = true;
                        glow.material.SetColor("_Color", new Color(0.5f, 0.3f, 0.3f));
                        ColorBlock colorBlockRed = readyButton.colors;
                        colorBlockRed.highlightedColor = new Color(0.55f, 0.4f, 0.4f);
                        readyButton.colors = colorBlockRed;
                        textButton.color = new Color(0.9f, 0.9f, 0.7f, 1);
                        break;
                    case ButtonState.Off:
                        readyButton.interactable = false;
                        glow.enabled = false;
                        textButton.color = Color.grey;
                        textButton.text = "...";
                        break;
                }
            }
        }

        public AudioSource AudioSource => audioSource;

        /*****************************************************************************************/
        public void OnPointerClick(PointerEventData eventData)
        {
            if (readyButton.interactable)
            {
                DOTween.Sequence().Append(transform.DOScale(1.1f, GameData.INTERACTIVE_TIME_DEFAULT / 2))
                    .Append(transform.DOScale(1f, GameData.INTERACTIVE_TIME_DEFAULT / 2));
                audioSource.PlayOneShot(State == ButtonState.Ready ? Click : Click2);
                buttonClickableAction.ReadyClicked(this);
            }
            textButton.transform.DOScale(1f, GameData.INTERACTIVE_TIME_DEFAULT);
            glow.transform.DOScale(1f, GameData.INTERACTIVE_TIME_DEFAULT);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (State == ButtonState.Ready)
            {
                audioSource.PlayOneShot(EnterHover);
                textButton.transform.DOScale(1.2f, GameData.INTERACTIVE_TIME_DEFAULT);
                if (withButtonEffects)
                    glow.transform.DOScale(1.2f, GameData.INTERACTIVE_TIME_DEFAULT);
            }
            else if (State == ButtonState.StandBy)
            {
                audioSource.PlayOneShot(EnterHover2);
                glow.transform.DOScale(1.1f, GameData.INTERACTIVE_TIME_DEFAULT);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (State == ButtonState.Ready)
            {
                audioSource.PlayOneShot(ExitHover);
                textButton.transform.DOScale(1f, GameData.INTERACTIVE_TIME_DEFAULT);
                if (withButtonEffects)
                    glow.transform.DOScale(1f, GameData.INTERACTIVE_TIME_DEFAULT);
            }
            else if (State == ButtonState.StandBy)
            {
                audioSource.PlayOneShot(ExitHover2);
                glow.transform.DOScale(1f, GameData.INTERACTIVE_TIME_DEFAULT);
            }
        }

        public void SetReadyButton(IButtonClickable buttonClickableAction, ButtonState state = ButtonState.Off)
        {
            this.buttonClickableAction = buttonClickableAction;
            State = state;
        }

        public void ChangeButtonText(string text) => textButton.text = text;

        public void EnterPanelSound() => audioSource.PlayOneShot(EnterPanel);

        public void ExitPanelSound() => audioSource.PlayOneShot(ExitPanel);

        public void SkillTestButtonGlowColor(float glowColor) =>
            glow.material.SetFloat("_HitEffectBlend", glowColor);

        public void SelectButton() => readyButton.Select();
    }
}