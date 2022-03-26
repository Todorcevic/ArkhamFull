using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Linq;
using System;
using System.IO;
using ArkhamShared;

namespace ArkhamGamePlay
{
    public class Loader : MonoBehaviour
    {
        [SerializeField] bool isTestMode;
        [SerializeField] AllComponents allComponents;

        void Awake()
        {
            QualitySettings.vSyncCount = 0;  // VSync must be disabled
            Application.targetFrameRate = 45;


            GameControl.IsTestMode = isTestMode;
            SetResolution.SettingResolution();
            JsonDataManager.LoadDataCards();
            allComponents.BuildingComponents();
            AllComponents.CardBuilder.Initializer();
            AllComponents.Table.SettingTableZones();
            AllComponents.PanelHistory.SettingPanelPosition();
            AllComponents.TokenStacks.SettingTokenStack();
            AllComponents.InvestigatorManagerComponent.BuildInvestigators();
            AllComponents.CardBuilder.BuildScenario();
        }

        void Start()
        {
            GameControl.CurrentAction = new VoidAction();
            if (isTestMode) new TestAction().AddActionTo();
            else new SettingGame().AddActionTo();
            StartCoroutine(GameControl.CurrentAction.RunNow());
        }

        public void Quit() => Application.Quit();
    }
}