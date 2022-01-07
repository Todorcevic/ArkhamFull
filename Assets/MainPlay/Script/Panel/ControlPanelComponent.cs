using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using ArkhamShared;

namespace ArkhamGamePlay
{
    public class ControlPanelComponent : MonoBehaviour
    {
        [SerializeField] Transform visualPosition;
        [SerializeField] Transform outPosition;
        [SerializeField] ReadyButton readyButton;
        public ReadyButton ReadyButton => readyButton;

        /*****************************************************************************************/
        public void SettingPanelPosition()
        {
            AllComponents.ReadyButton.State = ButtonState.Off;
            AllComponents.PanelHistory.LoadHistories();
            GameControl.CurrentPanelActive = null;
        }

        public IEnumerator SelectThisPanel()
        {
            if (this != GameControl.CurrentPanelActive)
            {
                yield return GameControl.CurrentPanelActive?.HideThisPanel();
                yield return DOTween.Sequence()
                    .AppendCallback(readyButton.EnterPanelSound)
                    .Append(transform.DOMove(visualPosition.position, GameData.ANIMATION_TIME_DEFAULT).SetEase(Ease.OutBack, 1.5f))
                    .WaitForCompletion();
                GameControl.CurrentPanelActive = this;
            }
        }

        public IEnumerator HideThisPanel()
        {
            GameControl.CurrentPanelActive = null;
            yield return DOTween.Sequence()
                .AppendCallback(readyButton.ExitPanelSound)
                .Append(transform.DOMove(outPosition.position, GameData.ANIMATION_TIME_DEFAULT).SetEase(Ease.InOutCubic))
                .WaitForCompletion();
        }

        public void SetReadyButton(IButtonClickable buttonClickableAction, ButtonState state = ButtonState.Off) =>
            readyButton.SetReadyButton(buttonClickableAction, state: state);
    }
}