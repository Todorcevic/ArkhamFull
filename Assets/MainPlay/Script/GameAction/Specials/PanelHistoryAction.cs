using System.Collections;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class PanelHistoryAction : GameAction, IButtonClickable
    {
        bool readyButtonIsClicked;
        readonly string idHistory;
        public override GameActionType GameActionType => GameActionType.History;

        /*****************************************************************************************/
        public PanelHistoryAction(string idHistory) => this.idHistory = idHistory;

        /*****************************************************************************************/
        protected override IEnumerator ActionLogic()
        {
            AllComponents.PanelHistory.SetPanel(idHistory);
            yield return AllComponents.PanelHistory.SelectThisPanel();
            AllComponents.PanelHistory.PlayAtmos(idHistory);
            SetButton();
            yield return new WaitUntil(() => readyButtonIsClicked);
            AllComponents.PanelHistory.StopAtmos(); ;
            AllComponents.PanelHistory.ReadyButton.State = ButtonState.Off;
            yield return AllComponents.PanelHistory.HideThisPanel();
        }

        public void SetButton() => AllComponents.PanelHistory.SetReadyButton(this, state: ButtonState.Ready);

        public void ReadyClicked(ReadyButton button) => readyButtonIsClicked = true;
    }
}