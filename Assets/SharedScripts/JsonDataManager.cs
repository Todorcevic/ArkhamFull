using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using ArkhamGamePlay;

namespace ArkhamShared
{
    public static class JsonDataManager
    {
        public static void SaveJsonInvestigatorData()
        {
            var jsonString = JsonConvert.SerializeObject(InvestigatorData.AllInvestigatorsData, Formatting.Indented);
            File.WriteAllText(GameFiles.StartedGameFilePath, jsonString);
        }

        public static void SaveJsonGameData()
        {
            var jsonString = JsonConvert.SerializeObject(GameData.Instance, Formatting.Indented);
            File.WriteAllText(GameFiles.GameDataFilePath, jsonString);
        }

        public static void LoadJsonGameData()
        {
            if (File.Exists(GameFiles.GameDataFilePath))
            {
                string fileJson = File.ReadAllText(GameFiles.GameDataFilePath);
                GameData.Instance = JsonConvert.DeserializeObject<GameData>(fileJson);
            }
        }

        public static void LoadInvestigatorsData()
        {
            string fileJson;
            if (GameControl.IsTestMode) fileJson = Resources.Load<TextAsset>(GameFiles.RESOURCE_PATH + GameFiles.TEST_INVESTIGATORS_FILE).text;
            else if (File.Exists(GameFiles.StartedGameFilePath)) fileJson = File.ReadAllText(GameFiles.StartedGameFilePath);
            else fileJson = Resources.Load<TextAsset>(GameFiles.RESOURCE_PATH + GameFiles.NEW_INVESTIGATORS_FILE).text;
            InvestigatorData.AllInvestigatorsData = JsonConvert.DeserializeObject<List<InvestigatorData>>(fileJson);
        }

        public static void DeleteInvestigatorsData()
        {
            if (File.Exists(GameFiles.StartedGameFilePath)) File.Delete(GameFiles.StartedGameFilePath);
            InvestigatorData.AllInvestigatorsData = new List<InvestigatorData>();
        }

        public static T CreateListFromJson<T>(string pathAndNameJsonFile)
        {
            TextAsset JsonText = Resources.Load<TextAsset>(pathAndNameJsonFile);
            return JsonConvert.DeserializeObject<T>(JsonText.text);
        }

        public static void LoadDataCards()
        {
            if (Card.DataCard != null) return;
            Card.DataCard = CreateListFromJson<Card[]>(GameFiles.RESOURCE_PATH + GameFiles.ALL_CARD_DATA_JSON);
            Card.DataCardDictionary = Card.DataCard.ToDictionary(c => c.Code);
            Card.MultiplyX2CoreSetQuantity();
        }
    }
}