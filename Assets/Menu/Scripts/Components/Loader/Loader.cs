using ArkhamMenu;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using ArkhamShared;
using System.Linq;
using ArkhamGamePlay;
using Michsky.UI.Dark;

namespace ArkhamMenu
{
    public class Loader : MonoBehaviour
    {
        bool IsGameStarted => File.Exists(GameFiles.StartedGameFilePath);
        [SerializeField] bool isTestMode;
        [SerializeField] Button continueButton;
        [SerializeField] ImageLoader imageLoader;
        [SerializeField] CardFactory cardFactory;
        [SerializeField] PanelManager panelManager;
        [SerializeField] MainPanelManager mainPanelManager;

        void Awake()
        {
            GameControl.IsTestMode = isTestMode;
            SetResolution.SettingResolution();
            JsonDataManager.LoadDataCards();
            JsonDataManager.LoadJsonGameData();
            imageLoader.CreateImageCards();
            Zones.CreateDictionaryZones();
            cardFactory.BuildCards();
        }

        void Start()
        {
            continueButton.SwitchON(IsGameStarted);
            if (GameData.Instance.IsAFinishGame)
            {
                GameData.Instance.IsAFinishGame = false;
                GoToInvestigatorPanel();
            }
        }

        void GoToInvestigatorPanel()
        {
            mainPanelManager.PanelAnim(2);
            panelManager.ContinueGame();
        }

        public void ExitAplication()
        {
            JsonDataManager.SaveJsonGameData();
            Application.Quit();
        }
    }
}
