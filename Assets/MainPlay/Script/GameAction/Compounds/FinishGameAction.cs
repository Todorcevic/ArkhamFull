using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ArkhamShared;

namespace ArkhamGamePlay
{
    public class FinishGameAction : GameAction, IButtonClickable
    {
        bool readyButtonIsClicked;
        readonly Effect resolution;
        List<CardComponent> locationsVictory =>
            GameControl.AllCardComponents.FindAll(c => c.CardType == CardType.Location
            && c.IsInPlay
            && c.VictoryPoint > 0
            && c.CluesToken.Amount == 0
            && ((CardLocation)c.CardLogic).IsRevealed);

        /*****************************************************************************************/
        public FinishGameAction(Effect resolution) => this.resolution = resolution;

        /*****************************************************************************************/
        protected override IEnumerator ActionLogic()
        {
            GameControl.GameIsStarted = false;
            yield return AddindLocationsVictory();
            yield return new EffectAction(resolution).RunNow();
            AllComponents.PanelCampaign.SetPanelCampaign();
            yield return AllComponents.PanelCampaign.SelectThisPanel();
            SetButton();
            yield return new WaitUntil(() => readyButtonIsClicked);
            GameData.Instance.IsAFinishGame = true;
            JsonDataManager.SaveJsonInvestigatorData();
            JsonDataManager.SaveJsonGameData();
            SceneManager.LoadScene(0);
        }

        IEnumerator AddindLocationsVictory()
        {
            foreach (CardComponent location in locationsVictory)
            {
                yield return new AddVictoryCardAction(location,
                    location.Info.Name + " otorga " + location.VictoryPoint + " puntos de experiencia a cada investigador.",
                    location.VictoryPoint).RunNow();
            }
        }

        public void SetButton() => AllComponents.PanelCampaign.SetReadyButton(this, state: ButtonState.Ready);

        public void ReadyClicked(ReadyButton button) => readyButtonIsClicked = true;
    }
}