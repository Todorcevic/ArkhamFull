using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace ArkhamMenu
{
    public class PreviewComponent : MonoBehaviour
    {
        [SerializeField] float timeAnimation;
        [SerializeField] float previewScale;
        [SerializeField] float delay;
        [SerializeField] Image imageCardPreview;
        [SerializeField] Image imageCardPreviewBack;
        [SerializeField] Image background;
        [SerializeField] Image background2;
        [SerializeField] Transform cardsSelected;
        [SerializeField] Transform investigatorCard;

        public void ShowingPreviewCard(CardBaseComponent card)
        {
            transform.localScale = Vector2.zero;
            transform.position = card.transform.position;
            imageCardPreview.sprite = card.ImageComponent.sprite;
            background.enabled = true;
            Vector2 positionPreview = new Vector2(card is RowCardComponent ? -1.5f : card.transform.position.x - 3.8f, 0f);
            transform.DOMove(positionPreview, timeAnimation).SetDelay(delay);
            transform.DOScale(previewScale, timeAnimation).SetDelay(delay);
            background.transform.DORotate(new Vector3(0, 0, 360), 160, mode: RotateMode.FastBeyond360).SetLoops(-1, LoopType.Yoyo).SetId("backdroundLooping").Play();

            if (card.ImageBack != null)
            {
                imageCardPreviewBack.sprite = card.ImageBack;
                background2.enabled = true;
                background2.transform.DORotate(new Vector3(0, 0, 360), 160, mode: RotateMode.FastBeyond360).SetLoops(-1, LoopType.Yoyo).SetId("backdroundLooping").Play();
            }
        }

        public void HidePreviewCard(CardBaseComponent card)
        {
            DOTween.Pause("backdroundLooping");
            transform.DOKill();
            imageCardPreview.sprite = null;
            imageCardPreviewBack.sprite = null;
            background.enabled = false;
            background2.enabled = false;
        }

        public void ChooseInvestigator(CardBaseComponent card)
        {
            transform.DOScale(0, timeAnimation);
            transform.DOMove(investigatorCard.position, timeAnimation);
        }

        public void ChooseCard(CardBaseComponent card)
        {
            transform.DOScale(0, timeAnimation);
            transform.DOMove(cardsSelected.transform.position, timeAnimation);
        }

        public void DeselectCard(CardBaseComponent card)
        {
            transform.DOScale(0, timeAnimation);
            transform.DOMove(card.transform.position, timeAnimation);
        }
    }
}