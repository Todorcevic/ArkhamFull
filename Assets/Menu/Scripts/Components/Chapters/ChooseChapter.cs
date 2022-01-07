using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using ArkhamShared;

namespace ArkhamMenu
{
    public class ChooseChapter : MonoBehaviour
    {
        public static string chapterTempChoosen;
        [SerializeField] TextMeshProUGUI nameChapter;
        [SerializeField] TextMeshProUGUI modalDescription;
        [SerializeField] PanelManager panelManager;

        public void Clicking(string jsonChapterName)
        {
            chapterTempChoosen = jsonChapterName;
            modalDescription.GetComponent<TextMeshProUGUI>().SetText("¿Estás seguro que deseás jugar " + nameChapter.text + "? Esto eliminará cualquier progreso anterior.");
        }

        public void SelectChapter()
        {
            GameData.Instance.Chapter = chapterTempChoosen;
            GameData.Instance.Scenario = "Scenario1";
            ResetGameData();
            panelManager.GoToInvestigatorPanel();
        }

        void ResetGameData()
        {
            JsonDataManager.DeleteInvestigatorsData();
            JsonDataManager.LoadInvestigatorsData();
            GameData.Instance.Chapter = "Core";
            GameData.Instance.Scenario = "Scenario1";
            GameData.Instance.ChapterData = new Chapter1Data();
            panelManager.RestartingInvestigator();
        }
    }
}
