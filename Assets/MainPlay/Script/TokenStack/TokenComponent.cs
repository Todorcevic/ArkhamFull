using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System;
using ArkhamShared;

namespace ArkhamGamePlay
{
    public class TokenComponent : MonoBehaviour
    {
        [SerializeField] SpriteRenderer buttonUp;
        [SerializeField] SpriteRenderer buttonDown;
        [SerializeField] TMP_Text textValue;
        [SerializeField] MeshRenderer visualModel;
        [SerializeField] CardComponent cardOwner;
        [SerializeField] CardTokenType tokenType;
        [SerializeField] AudioSource audioSource;
        [SerializeField] AudioClip TokenMoveUp;
        [SerializeField] AudioClip TokenButton;
        [SerializeField] AudioClip TokenExit;
        [SerializeField] AudioClip TokenEnter;
        public int Amount { get; set; }
        public CardComponent TokenInThisCard { get => cardOwner; set => cardOwner = value; }
        public CardTokenType TokenType { get => tokenType; set => tokenType = value; }
        public int AssignValue { get; set; }

        /*****************************************************************************************/
        public IEnumerator AddAmountFrom(int amount, TokenComponent fromToken)
        {
            if (amount != 0)
            {
                yield return new SelectInvestigatorAction(fromToken.TokenInThisCard.VisualOwner).RunNow();
                for (int i = 0; i < amount; i++)
                    TokenAnimationUp(fromToken, i);
                yield return new WaitWhile(() => DOTween.IsTweening("StartTokenMove"));
                yield return new SelectInvestigatorAction(TokenInThisCard.VisualOwner).RunNow();
                for (int i = 0; i < amount; i++)
                    TokenAnimationDown(i);
            }
            yield return new WaitWhile(() => DOTween.IsTweening("FinishTokenMove"));
            ShowAmount();
            AllComponents.CardBuilder.Tokens.FindAll(t => t.visualModel.gameObject.activeSelf)
                .ForEach(t => t.ShowToken(false));
        }

        Tween TokenAnimationUp(TokenComponent fromToken, int amount)
        {
            Amount++;
            fromToken.Amount--;
            fromToken.ShowAmount();
            AllComponents.CardBuilder.Tokens[amount].visualModel.sharedMaterial = visualModel.sharedMaterial;
            AllComponents.CardBuilder.Tokens[amount].ShowToken(true);
            AllComponents.CardBuilder.Tokens[amount].transform.position = fromToken.transform.position;
            audioSource.PlayOneShot(TokenMoveUp);
            Vector3 randomPosition = (UnityEngine.Random.insideUnitSphere * 0.5f) + new Vector3(0, UnityEngine.Random.Range(-0.1f, 0.1f), 0);
            return DOTween.Sequence()
                .AppendCallback(() => audioSource.PlayOneShot(TokenExit))
                .Append(AllComponents.CardBuilder.Tokens[amount].transform.DOMove(AllComponents.CenterPreview.position + randomPosition, GameData.ANIMATION_TIME_DEFAULT * 1.5f).SetEase(Ease.OutCubic))
               .Join(AllComponents.CardBuilder.Tokens[amount].transform.DOScale(2f, GameData.ANIMATION_TIME_DEFAULT))
               .Join(AllComponents.CardBuilder.Tokens[amount].transform.DORotate(AllComponents.CenterPreview.rotation.eulerAngles + new Vector3(180, 0, 0), GameData.ANIMATION_TIME_DEFAULT))
            .SetDelay((amount + UnityEngine.Random.Range(0, 2f)) * GameData.DELAY_TIME_DEFAULT).SetId("StartTokenMove");
        }

        Tween TokenAnimationDown(int amount)
        {
            return DOTween.Sequence().Append(AllComponents.CardBuilder.Tokens[amount].transform.DOMove(transform.position, GameData.ANIMATION_TIME_DEFAULT * 1.5f).SetEase(Ease.InCubic))
               .Join(AllComponents.CardBuilder.Tokens[amount].transform.DOScale(1f, GameData.ANIMATION_TIME_DEFAULT))
               .Join(AllComponents.CardBuilder.Tokens[amount].transform.DORotate(transform.rotation.eulerAngles, GameData.ANIMATION_TIME_DEFAULT))
               .InsertCallback(GameData.ANIMATION_TIME_DEFAULT * 1.25f, () => audioSource.PlayOneShot(TokenEnter))
               .SetDelay((amount + UnityEngine.Random.Range(0, 2f)) * GameData.DELAY_TIME_DEFAULT).SetId("FinishTokenMove");
        }

        public void ShowAmount()
        {
            ShowToken(Amount > 0);
            textValue.text = Amount > 1 ? Amount.ToString() : string.Empty;
        }

        public void TokenText(string text) => textValue.text = text;

        public void ButtonUpActive(bool isActive)
        {
            if (buttonUp.gameObject.activeSelf == isActive) return;
            if (isActive) buttonUp.color = Color.green;
            buttonUp.gameObject.SetActive(isActive);
        }

        public void ButtonDownActive(bool isActive)
        {
            if (buttonDown.gameObject.activeSelf == isActive) return;
            if (isActive) buttonDown.color = Color.green;
            buttonDown.gameObject.SetActive(isActive);
        }

        public void ShowToken(bool isActive) => visualModel.gameObject.SetActive(isActive);

        public void HideButtons()
        {
            buttonUp.gameObject.SetActive(false);
            buttonDown.gameObject.SetActive(false);
        }

        public void PlaySoundButton() => audioSource.PlayOneShot(TokenButton);
    }
}