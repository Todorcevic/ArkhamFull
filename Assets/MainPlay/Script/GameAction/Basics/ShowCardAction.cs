using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using ArkhamShared;

namespace ArkhamGamePlay
{
    public class ShowCardAction : GameAction, IButtonClickable
    {
        bool readyButtonIsClicked;
        readonly bool withReturn;
        readonly bool isBack;
        readonly AudioClip audioClip;
        public override GameActionType GameActionType => GameActionType.Basic;
        public CardComponent ThisCard { get; set; }

        /*****************************************************************************************/
        public ShowCardAction(CardComponent card, bool isBack = false, bool withReturn = false, AudioClip audioClip = null)
        {
            ThisCard = card;
            this.isBack = isBack;
            this.withReturn = withReturn;
            this.audioClip = audioClip ?? isBack ? ThisCard.ShowCardBack : ThisCard.ShowCardFront;
        }

        /*****************************************************************************************/
        protected override IEnumerator ActionLogic()
        {
            if (ThisCard.IsEliminated) yield break;
            yield return new SelectInvestigatorAction(ThisCard.VisualOwner).RunNow();
            if (ThisCard.MyOwnZone.ListCards.Count > 0)
                ThisCard.CardTools.ShowCardsMyOwnZone(false);
            yield return DOTween.Sequence()
                .Append(ThisCard.Preview())
                .Join(ThisCard.TurnDown(isBack))
                .WaitForCompletion();
            ThisCard.Idle();
            AllComponents.Table.CenterPreview.ListCards.Add(ThisCard);
            if (audioClip != null) ThisCard.CardTools.PlayOneShotSound(audioClip);
            SetButton();
            yield return new WaitUntil(() => readyButtonIsClicked);
            if (audioClip != null)
            {
                ThisCard.CardTools.AudioSource.DOFade(0, GameData.ANIMATION_TIME_DEFAULT * 4).OnComplete(ResetAudioSource);
                void ResetAudioSource()
                {
                    ThisCard.CardTools.AudioSource.Stop();
                    ThisCard.CardTools.AudioSource.volume = 1;
                }
            }
            ThisCard.CardTools.ShowCardsMyOwnZone(true);
            AllComponents.ReadyButton.State = ButtonState.Off;
            DOTween.Kill("Idle");
            if (withReturn) yield return ThisCard.MoveTo(ThisCard.CurrentZone).WaitForCompletion();
        }

        public void SetButton() => AllComponents.ReadyButton.SetReadyButton(this, state: ButtonState.Ready);
        public void ReadyClicked(ReadyButton button) => readyButtonIsClicked = true;
    }
}